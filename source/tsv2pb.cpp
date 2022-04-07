//
// Created by liuyi on 2021/6/23.
//
#include "tsv2pb.h"
#include <google/protobuf/dynamic_message.h>
#include <filesystem>
#include <fstream>
#include <regex>
#include "proto/tsv2pb.pb.h"
#include "util/CommonUtil.h"
#include "util/logger.h"
#include "xlsxcheck.h"

using namespace google::protobuf;

class MyErrorCollector: public google::protobuf::compiler::MultiFileErrorCollector
{
    virtual void AddError(const std::string & filename, int line, int column, const std::string & message){
        // define import error collector
        LOG_ERROR("[协议错误][格式错误] 行[" << line << "] 列[" << column << "] proto文件[" << filename << "] 错误信息["<< message << "]");
    }
};

struct ReflectionMessageData {
    string tsv_name;
    string proto_name;
    string merge_key;
    Map<string, const FieldDescriptor*> field_map;
    const google::protobuf::Descriptor* descriptor_array;
};

tsv2pb::tsv2pb() {
}

tsv2pb::~tsv2pb() {
    delete importer_;
}

void tsv2pb::ParseProto(const std::string& proto_path) {
    // 准备配置好文件系统
    google::protobuf::compiler::DiskSourceTree sourceTree;
    // 将当前路径映射为项目根目录
    sourceTree.MapPath("", proto_path);
    // 将raw_proto映射为项目根目录
    sourceTree.MapPath("", std::filesystem::current_path().append("raw_proto").generic_string());

    // 配置动态编译器
    MyErrorCollector errorCollector;
    importer_ = new google::protobuf::compiler::Importer(&sourceTree, &errorCollector);

    // 动态编译proto源文件 & 收集所有message
    for (const auto &p:std::filesystem::recursive_directory_iterator(proto_path)) {
        if (p.path().extension() == ".proto") {
            auto relative_name = relative(p.path(), proto_path).generic_string();
            LOG_INFO(relative_name);
            const FileDescriptor *fileDescriptor = importer_->Import(relative_name);
            if (fileDescriptor == nullptr) {
                LOG_ERROR("[协议错误][导入报错] proto文件[" << relative_name << "]");
                has_error_ = true;
                return;
            }
            if (relative_name.find("google/") == std::string::npos) {
                LOG_INFO("ParseProto[" << relative_name << "] start");
                // message
                for (int i = 0; i < fileDescriptor->message_type_count(); ++i) {
                    ParseMessage(fileDescriptor->message_type(i), relative_name);
                }
                // enum
                for (int i = 0; i < fileDescriptor->enum_type_count(); ++i) {
                    ParseEnum(fileDescriptor->enum_type(i), relative_name);
                }
                LOG_INFO("ParseProto[" << relative_name << "] end");
            }
        }
    }
}

void tsv2pb::ParseMessage(const google::protobuf::Descriptor* descriptor, const string& relative_name) {
    LOG_INFO(descriptor->full_name());
    if (descriptor->options().HasExtension(mapping)) {
        auto descriptor_array = importer_->pool()->FindMessageTypeByName(descriptor->full_name() + "Array");
        if (!descriptor_array) {
            LOG_ERROR("[协议错误] proto文件[" << relative_name << "] 缺失Message[" << descriptor->full_name() << "Array]");
            has_error_ = true;
            return;
        }

        auto value_list = descriptor->options().GetRepeatedExtension(mapping);
        for (auto value : value_list) {
            auto list = CommonUtil::stringSplit(value, "#");
            if (list.size() != 2) {
                LOG_ERROR("[协议错误] proto文件[" << relative_name << "] msg_name[" << descriptor->full_name() << "] mapping字段格式错误[" << value << "]");
                has_error_ = true;
                return;
            }

            ReflectionMessageData data;
            data.tsv_name = list[0];
            data.proto_name = list[1];
            data.descriptor_array = descriptor_array;

            for (int i = 0; i < descriptor->field_count(); ++i) {
                auto field_descriptor = descriptor->field(i);
                auto field_value_list = field_descriptor->options().GetRepeatedExtension(field);
                for (auto field_value : field_value_list) {
                    if (CommonUtil::stringContain(field_value, "#")) {
                        auto field_list = CommonUtil::stringSplit(field_value, "#");
                        if (field_list[1] == data.proto_name) {
                            data.field_map[field_list[0]] = field_descriptor;
                            LOG_INFO(field_descriptor->name() << " = " << field_value);
                            break;
                        }
                    } else {
                        data.field_map[field_value] = field_descriptor;
                        LOG_INFO(field_descriptor->name() << " = " << field_value);
                    }
                }
            }

            if (descriptor->options().HasExtension(merge_key)) {
                data.merge_key = descriptor->options().GetExtension(merge_key);
            }
            proto_message_data_.push_back(data);
        }
    } else {
        ReflectionMessageData data;
        for (int i = 0; i < descriptor->field_count(); ++i) {
            auto field_descriptor = descriptor->field(i);
            auto field_value_list = field_descriptor->options().GetRepeatedExtension(field);
            for (auto field_value : field_value_list) {
                data.field_map[field_value] = field_descriptor;
                LOG_INFO(field_descriptor->name() << " = " << field_value);
            }
        }
        proto_sub_msg_data_[descriptor->full_name()] = data;
    }
}

void tsv2pb::ParseEnum(const google::protobuf::EnumDescriptor* descriptor, const string& relative_name) {
    LOG_INFO(descriptor->full_name());

    map<string, int> data;
    for (int i = 0; i < descriptor->value_count(); ++i) {
        auto enum_value_descriptor = descriptor->value(i);
        auto enum_str = enum_value_descriptor->options().GetExtension(name);
        auto enum_value = enum_value_descriptor->number();
        data[enum_str] = enum_value;
        LOG_INFO(enum_str << " = " << enum_value);
    }
    proto_enum_data_[descriptor->full_name()] = data;
}

//
// 二次处理tsv数据
//
void tsv2pb::PreProcessValue(xlsxcheck& xlsx_check) {
    // 1.根据message的option字段merge_key预处理数据
    for (auto data : proto_message_data_) {
        if (data.merge_key.empty()) {
            continue;
        }

        vector<XlsxValueMap*> tmp_list;
        auto* xlxs_data = xlsx_check.xlsx_data_map()[data.tsv_name];
        for (int i = 0; i < xlxs_data->processed_value_list_size(); ++i) {
            auto item = xlxs_data->mutable_processed_value_list(i);
            if (!item->value_map().contains(data.merge_key)) {
                LOG_ERROR(data.merge_key);
                continue;
            }
            auto keyValue = item->value_map().at(data.merge_key).value(0); // 0 merge_key不可能有多个value
            auto exist = false;
            XlsxValueMap* targetMap;

            for (auto item_tmp : tmp_list) {
                auto cacheKeyValue = item_tmp->value_map().at(data.merge_key).value(0);
                if (keyValue == cacheKeyValue) {
                    exist = true;
                    targetMap = item_tmp;
                }
            }

            if (exist) {
                // 合并数据
                auto* target = targetMap->mutable_value_map();
                for (const auto& [k ,v] : item->value_map()) {
                    if ((*target)[k].value_size() > 0) {
                        // 普通字段
                        if (v.value_size() > 0) {
                            for (const auto &value : v.value()) {
                                (*target)[k].add_value(value);
                            }
                        }
                    } else {
                        // 结构体字段
                        if (v.struct_value_size() > 0) {
                            for (const auto & [k_struct, v_struct] : v.struct_value()) {
                                if (!(*target)[k].struct_value().contains(k_struct)) {
                                    XlsxValue* structValue = new XlsxValue();
                                    structValue->MergeFrom(v_struct);
                                    (*(*target)[k].mutable_struct_value())[k_struct] = *structValue;
                                } else {
                                    XlsxValue* structValue = (XlsxValue*) &(*target)[k].struct_value().at(k_struct);
                                    for (const auto &value : v_struct.value()) {
                                        structValue->add_value(value);
                                    }
                                }
                            }
                        }
                    }
                }
            } else {
                tmp_list.push_back(item);
            }
        }

        // clear前缓存数据
        vector<XlsxValueMap> tmp1;
        for (auto item : tmp_list) {
            tmp1.push_back(*item);
        }

        xlxs_data->clear_processed_value_list();
        for (auto item : tmp1) {
            auto processed_value_map = xlxs_data->add_processed_value_list();
            processed_value_map->CopyFrom(item);
        }
    }
}

//
// 反射创建实例，用tsv数据填充，导出pb二进制文件
//
void tsv2pb::ReflectionMessage(const string &tsv_path, const string &store_path, const string &store_suffix, xlsxcheck& xlsx_check) {
    // 先删除store_path下文件
    if (filesystem::exists(store_path)) {
        filesystem::remove_all(store_path);
    }
    filesystem::create_directory(store_path);

    auto factory  = new google::protobuf::DynamicMessageFactory(importer_->pool());
    for (auto data : proto_message_data_) {
        LOG_INFO("START: "<<  data.tsv_name);
        auto xlsx_data_map = xlsx_check.xlsx_data_map();
        if (xlsx_data_map.find(data.tsv_name) == xlsx_data_map.end()) {
            LOG_ERROR("[表名错误] 表[" << data.tsv_name << "]");
            has_error_ = true;
            return;
        }
        auto xlxs_data = xlsx_data_map[data.tsv_name];
        auto prototype_array = factory->GetPrototype(data.descriptor_array);
        auto message_array = prototype_array->New();
        auto reflecter_array = message_array->GetReflection();
        auto field_array = data.descriptor_array->FindFieldByName("items");

        for (int i = 0; i < xlxs_data->processed_value_list_size(); ++i) {
            auto message = reflecter_array->AddMessage(message_array, field_array);
            for (auto const& [key, val] : data.field_map) {
                if (!xlxs_data->processed_value_list(i).value_map().contains(key)) {
                    LOG_ERROR("[字段错误] 表[" << data.tsv_name << "] 缺少 field[" << key << "]");
                    has_error_ = true;
                    return;
                }
                auto data_ = xlxs_data->processed_value_list(i).value_map().at(key);
                if (data_.value_size() > 0) {
                    for (auto const &value : data_.value()) {
                        SetMessageValue(message, val, value, i, key, data.tsv_name);
                    }

                } else {
                    if (val->message_type() == NULL) {
                        LOG_ERROR("[字段错误] 表[" << data.tsv_name << "] field[" << key << "]");
                        return;
                    }

                    // 结构消息
                    // map转成vector
                    vector<map<string, string>> list;
                    for (auto const&[sub_key, sub_val] : data_.struct_value()) {
                        for (int j = 0; j < sub_val.value_size(); ++j) {
                            if (list.size() <= j) {
                                map<string, string> sub_map;
                                list.push_back(sub_map);
                            }
                            list[j][sub_key] = sub_val.value(j);
                        }
                    }

                    auto sub_data = proto_sub_msg_data_[val->message_type()->full_name()];
                    for (auto const &value : list) {
                        // 去掉空的字段
                        auto isEmpty = true;
                        for (auto const&[sub_key, sub_val] : value) {
                            if (!sub_val.empty()) {
                                isEmpty = false;
                                break;
                            }
                        }
                        if (isEmpty) continue;

                        Message *sub_msg;
                        if (val->is_repeated()) {
                            sub_msg = message->GetReflection()->AddMessage(message, val);
                        } else {
                            sub_msg = message->GetReflection()->MutableMessage(message, val);
                        }
                        for (auto const&[sub_key, sub_val] : value) {
                            if (sub_data.field_map.contains(sub_key)) {
                                SetMessageValue(sub_msg, sub_data.field_map[sub_key], sub_val, i, key + "#" + sub_key, data.tsv_name);
                            } else {
                                LOG_WARN("[字段错误] 表[" << data.tsv_name << "] 缺少 field[" << sub_key << "]");
                            }
                        }
                    }
                }
            }
        }

        // 1.导出pb二进制文件
        auto outputFilePath = store_path + "/" + data.proto_name + "." + store_suffix;
        LOG_INFO(outputFilePath);
        ofstream outfile;
        outfile.open(outputFilePath, ios::out | ios::app | ios::binary);
        message_array->SerializePartialToOstream(&outfile);
        outfile.close();

        // 2.导出pb txt文件
        {
            auto outputFilePath = store_path + "/" + data.proto_name + ".txt";
            LOG_INFO(outputFilePath);
            ofstream outfile;
            outfile.open(outputFilePath, ios::out | ios::app | ios::binary);
            outfile << message_array->Utf8DebugString();
            outfile.close();
        }

        delete message_array;
    }
    delete factory;
}

//
// 用tsv数据填充
//
void tsv2pb::SetMessageValue(Message* message, const FieldDescriptor* field, const string& value, int row, const string& col, const string& tsv_name) {
    auto reflection = message->GetReflection();
    if (field->is_repeated()) {
        switch (field->cpp_type()) {
            case FieldDescriptor::CppType::CPPTYPE_BOOL:
            {
                if (value.empty()) return;
                reflection->AddBool(message, field, !(ToUpper(value) == "FALSE" or value == "0"));
                break;
            }
            case FieldDescriptor::CppType::CPPTYPE_DOUBLE:
            {
                if (value.empty()) return;
                double ret;
                if (safe_strtod(value, &ret)) {
                    reflection->AddDouble(message, field, ret);
                } else {
                    LOG_ERROR("[字段错误] 表[" << tsv_name << "] 行[" << row + 4 << "] 列[" << col << "]  pb_field_name[" << field->name() << "] tsv_value[" << value << "] must be [" << field->cpp_type_name() << "]");
                    has_error_ = true;
                }
                break;
            }
            case FieldDescriptor::CppType::CPPTYPE_FLOAT:
            {
                if (value.empty()) return;
                float ret;
                if (safe_strtof(value, &ret)) {
                    reflection->AddFloat(message, field, ret);
                } else {
                    LOG_ERROR("[字段错误] 表[" << tsv_name << "] 行[" << row + 4 << "] 列[" << col << "]  pb_field_name[" << field->name() << "] tsv_value[" << value << "] must be [" << field->cpp_type_name() << "]");
                    has_error_ = true;
                }
                break;
            }
            case FieldDescriptor::CppType::CPPTYPE_INT32:
            {
                if (value.empty()) return;
                int32 ret;
                if (safe_strto32(value, &ret)) {
                    reflection->AddInt32(message, field, ret);
                } else {
                    LOG_ERROR("[字段错误] 表[" << tsv_name << "] 行[" << row + 4 << "] 列[" << col << "]  pb_field_name[" << field->name() << "] tsv_value[" << value << "] must be [" << field->cpp_type_name() << "]");
                    has_error_ = true;
                }
                break;
            }
            case FieldDescriptor::CppType::CPPTYPE_INT64:
            {
                if (value.empty()) return;
                int64 ret;
                if (safe_strto64(value, &ret)) {
                    reflection->AddInt64(message, field, ret);
                } else {
                    LOG_ERROR("[字段错误] 表[" << tsv_name << "] 行[" << row + 4 << "] 列[" << col << "]  pb_field_name[" << field->name() << "] tsv_value[" << value << "] must be [" << field->cpp_type_name() << "]");
                    has_error_ = true;
                }
                break;
            }
            case FieldDescriptor::CppType::CPPTYPE_UINT32:
            {
                if (value.empty()) return;
                uint32 ret;
                if (safe_strtou32(value, &ret)) {
                    reflection->AddUInt32(message, field, ret);
                } else {
                    LOG_ERROR("[字段错误] 表[" << tsv_name << "] 行[" << row + 4 << "] 列[" << col << "]  pb_field_name[" << field->name() << "] tsv_value[" << value << "] must be [" << field->cpp_type_name() << "]");
                    has_error_ = true;
                }
                break;
            }
            case FieldDescriptor::CppType::CPPTYPE_UINT64:
            {
                if (value.empty()) return;
                uint64 ret;
                if (safe_strtou64(value, &ret)) {
                    reflection->AddUInt64(message, field, ret);
                } else {
                    LOG_ERROR("[字段错误] 表[" << tsv_name << "] 行[" << row + 4 << "] 列[" << col << "]  pb_field_name[" << field->name() << "] tsv_value[" << value << "] must be [" << field->cpp_type_name() << "]");
                    has_error_ = true;
                }
                break;
            }
            case FieldDescriptor::CppType::CPPTYPE_ENUM:
            {
                if (value.empty()) return;
                auto map = proto_enum_data_[field->enum_type()->full_name()];
                if (map.find(value) != map.end()) {
                    int32 ret = map[value];
                    reflection->AddEnumValue(message, field, ret);
                } else {
                    LOG_ERROR("[字段错误] 表[" << tsv_name << "] 行[" << row + 4 << "] 列[" << col << "]  pb_field_name[" << field->name() << "] tsv_value[" << value << "] not in enum[" << field->enum_type()->full_name() << "]");
                    has_error_ = true;
                }
                break;
            }
            case FieldDescriptor::CppType::CPPTYPE_STRING:
            {
                if (value.empty()) return;
                auto value2 = regex_replace(value, regex("\\n"), "\n");
                reflection->AddString(message, field, value2);
                break;
            }
            case FieldDescriptor::CppType::CPPTYPE_MESSAGE:
                // TODO
                // google::protobuf::Message* submessage = reflection->MutableMessage(message, field);
                // parse_message(value, submessage);
                break;
            default:
                LOG_ERROR("SetMessageValue: " << field->cpp_type());
                has_error_ = true;
                break;
        }
    } else {
        switch (field->cpp_type()) {
            case FieldDescriptor::CppType::CPPTYPE_BOOL:
            {
                if (value.empty()) return;
                reflection->SetBool(message, field, !(ToUpper(value) == "FALSE" or value == "0"));
                break;
            }
            case FieldDescriptor::CppType::CPPTYPE_DOUBLE:
            {
                if (value.empty()) return;
                double ret;
                if (safe_strtod(value, &ret)) {
                    reflection->SetDouble(message, field, ret);
                } else {
                    LOG_ERROR("[字段错误] 表[" << tsv_name << "] 行[" << row + 4 << "] 列[" << col << "]  pb_field_name[" << field->name() << "] tsv_value[" << value << "] must be [" << field->cpp_type_name() << "]");
                    has_error_ = true;
                }
                break;
            }
            case FieldDescriptor::CppType::CPPTYPE_FLOAT:
            {
                if (value.empty()) return;
                float ret;
                if (safe_strtof(value, &ret)) {
                    reflection->SetFloat(message, field, ret);
                } else {
                    LOG_ERROR("[字段错误] 表[" << tsv_name << "] 行[" << row + 4 << "] 列[" << col << "]  pb_field_name[" << field->name() << "] tsv_value[" << value << "] must be [" << field->cpp_type_name() << "]");
                    has_error_ = true;
                }
                break;
            }
            case FieldDescriptor::CppType::CPPTYPE_INT32:
            {
                if (value.empty()) return;
                int32 ret;
                if (safe_strto32(value, &ret)) {
                    reflection->SetInt32(message, field, ret);
                } else {
                    LOG_ERROR("[字段错误] 表[" << tsv_name << "] 行[" << row + 4 << "] 列[" << col << "]  pb_field_name[" << field->name() << "] tsv_value[" << value << "] must be [" << field->cpp_type_name() << "]");
                    has_error_ = true;
                }
                break;
            }
            case FieldDescriptor::CppType::CPPTYPE_INT64:
            {
                if (value.empty()) return;
                int64 ret;
                if (safe_strto64(value, &ret)) {
                    reflection->SetInt64(message, field, ret);
                } else {
                    LOG_ERROR("[字段错误] 表[" << tsv_name << "] 行[" << row + 4 << "] 列[" << col << "]  pb_field_name[" << field->name() << "] tsv_value[" << value << "] must be [" << field->cpp_type_name() << "]");
                    has_error_ = true;
                }
                break;
            }
            case FieldDescriptor::CppType::CPPTYPE_UINT32:
            {
                if (value.empty()) return;
                uint32 ret;
                if (safe_strtou32(value, &ret)) {
                    reflection->SetUInt32(message, field, ret);
                } else {
                    LOG_ERROR("[字段错误] 表[" << tsv_name << "] 行[" << row + 4 << "] 列[" << col << "]  pb_field_name[" << field->name() << "] tsv_value[" << value << "] must be [" << field->cpp_type_name() << "]");
                    has_error_ = true;
                }
                break;
            }
            case FieldDescriptor::CppType::CPPTYPE_UINT64:
            {
                if (value.empty()) return;
                uint64 ret;
                if (safe_strtou64(value, &ret)) {
                    reflection->SetUInt64(message, field, ret);
                } else {
                    LOG_ERROR("[字段错误] 表[" << tsv_name << "] 行[" << row + 4 << "] 列[" << col << "]  pb_field_name[" << field->name() << "] tsv_value[" << value << "] must be [" << field->cpp_type_name() << "]");
                    has_error_ = true;
                }
                break;
            }
            case FieldDescriptor::CppType::CPPTYPE_ENUM:
            {
                if (value.empty()) return;
                auto map = proto_enum_data_[field->enum_type()->full_name()];
                if (map.find(value) != map.end()) {
                    int32 ret = map[value];
                    reflection->SetEnumValue(message, field, ret);
                } else {
                    LOG_ERROR("[字段错误] 表[" << tsv_name << "] 行[" << row + 4 << "] 列[" << col << "]  pb_field_name[" << field->name() << "] tsv_value[" << value << "] not in enum[" << field->enum_type()->full_name() << "]");
                    has_error_ = true;
                }
                break;
            }
            case FieldDescriptor::CppType::CPPTYPE_STRING:
            {
                if (value.empty()) return;
                auto value2 = regex_replace(value, regex("\\\\n"), "\n");
                reflection->SetString(message, field, value2);
                break;
            }
            case FieldDescriptor::CppType::CPPTYPE_MESSAGE:
            {
                google::protobuf::Message* submessage = reflection->MutableMessage(message, field);
                // parse_message(value, submessage);
                break;
            }
            default:
                LOG_ERROR("SetMessageValue: " << field->cpp_type());
                has_error_ = true;
                break;
        }
    }
}

int tsv2pb::Export(const string &proto_path, const string &tsv_path, const string &store_path, const string &store_suffix, xlsxcheck& xlsx_check) {
    has_error_ = false;
    LOG_INFO("ParseProto:" + proto_path);
    ParseProto(proto_path);
    LOG_INFO("ParseProto:" + proto_path + " OK!!!");

    LOG_INFO("PreProcessValue:");
    PreProcessValue(xlsx_check);
    LOG_INFO("PreProcessValue: OK!!!");

    LOG_INFO("ReflectionMessage:");
    ReflectionMessage(tsv_path, store_path, store_suffix, xlsx_check);
    LOG_INFO("ReflectionMessage: OK!!!");

    if (has_error_) return -1;
    return 0;
}