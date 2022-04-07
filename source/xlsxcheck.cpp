//
// Created by liuyi on 2021/8/1.
//
#include <fstream>
#include <filesystem>
#include <iostream>
#include <google/protobuf/text_format.h>
#include <google/protobuf/map.h>
#include <regex>
#include "xlsxcheck.h"
#include "util/CommonUtil.h"
#include "util/logger.h"

using namespace google::protobuf;
using namespace google::protobuf::util;
using namespace TOOL;

const int ROW_KEY = 1;
const int ROW_COMMENT = 2;
const int ROW_CHECK = 3;
const int ROW_VALUE = 4;

// 多表联合检查字段唯一性数据库
struct AllUniqueData {
    string tabel_name;
    int row;
    int col;
};

xlsxcheck::xlsxcheck() {
}

xlsxcheck::~xlsxcheck(){
    for (const auto& [k, v] : xlsx_data_map_) {
        delete v;
    }
    for (const auto& [k, v] : check_lable_map_) {
        delete v;
    }
}

int xlsxcheck::Export(const string& proto_path, const string& tsv_path) {
    has_error_ = false;
    LoadCheckLabel(proto_path);
    for (const auto& p: filesystem::recursive_directory_iterator(tsv_path)) {
        auto filePath = p.path();
        if (filePath.extension() == ".tsv") {
            LOG_INFO("load tsv:" << filePath.filename());
            LoadTsv(filePath);
        }
    }

    CheckLabel();

    if (has_error_) return -1;
    LOG_INFO("[xlsxcheck]: SUCCESS");
    return 0;
}

void xlsxcheck::LoadTsv(const std::filesystem::path& filePath) {
    XlsxData* xlsxData = new XlsxData();
    xlsxData->set_xlsxname(filePath.stem().string());
    auto checklabel_map = xlsxData->mutable_checklabel_map();
    xlsx_data_map_[filePath.stem().string()] = xlsxData;

    ifstream infile;
    infile.open(filePath, ios::in);

    std::map<int, string> key_map;
    string line;
    int row = 0;

    while (getline(infile, line))
    {
        row++;
        line = CommonUtil::ClearHeadTail(line, "\r"); // 去掉行尾的换行符
        auto value_list = CommonUtil::stringSplit(line, "\t", false);
        if (row == ROW_KEY) {
            for (int i = 0; i < value_list.size(); ++i) {
                key_map[i] = value_list[i];
                xlsxData->add_field_list(value_list[i]);
            }
        } else if (row == ROW_COMMENT) {
            // 注释
        } else if (row == ROW_CHECK) {
            // 检表语句
            for (int i = 0; i < value_list.size(); ++i) {
                if (value_list[i].empty()) continue;
                CheckLabelCommand* msg_command = new CheckLabelCommand();
                if (check_lable_map_.find(value_list[i]) != check_lable_map_.end()) {
                    msg_command->CopyFrom(*check_lable_map_[value_list[i]]);
                } else {
                    auto str = regex_replace(value_list[i], regex(R"(\\n)"), " ");
                    if (!google::protobuf::TextFormat::ParseFromString(str, msg_command)) {
                        LOG_ERROR("[检表语句错误] 表[" << xlsxData->xlsxname() << "] 语句[" << str << "]");
                        has_error_ = true;
                    }
                }
                (*checklabel_map)[i] = *msg_command;
            }
        } else if (row >= ROW_VALUE) {
            // 跳过头为空的行
            if (value_list[0].empty()) continue;

            auto original_value_array = xlsxData->add_original_value_list();
            auto processed_value_map = xlsxData->add_processed_value_list();
            auto value_map = processed_value_map->mutable_value_map();
            for (int i = 0; i < value_list.size(); ++i) {
                original_value_array->add_values(value_list[i]);
                auto key = key_map[i];
                auto value = value_list[i];

                string outKey;
                if (CommonUtil::isStruct(key, outKey)) {
                    // 结构字段
                    ParseStructValue(outKey, value, value_map);
                } else {
                    // 普通字段
                    ParseValue(outKey, value, value_map);
                }
            }
        }
    }

    LOG_INFO(xlsxData->Utf8DebugString());

    infile.close();
}

// 普通字段解析
void xlsxcheck::ParseValue(string& key,  const string& value, Map<string, XlsxValue>* processed_value_map) {
    if (!processed_value_map->contains(key)) {
        XlsxValue* xlsxValue = new XlsxValue();
        xlsxValue->set_field(key);
        (*processed_value_map)[key] = *xlsxValue;
    }
    XlsxValue* xlsxValue = &processed_value_map->at(key);
    if (!CommonUtil::stringContain(value, "&")) {
        xlsxValue->add_value(value);
    } else {
        // &分离数组支持
        auto list = CommonUtil::stringSplit(value, "&");
        for (auto& sub : list) {
            xlsxValue->add_value(sub);
        }
    }
}

// 结构字段解析
void xlsxcheck::ParseStructValue(string& key1,  const string& value, Map<string, XlsxValue>* processed_value_map) {
    auto list = CommonUtil::stringSplit(key1, "#");
    auto key = list[0];
    string sub_key = list[1];
    int sub_index = 0;
    if (list.size() > 2) {
        safe_strto32(list[2], &sub_index);
    }

    if (!processed_value_map->contains(key)) {
        XlsxValue* xlsxValue = new XlsxValue();
        xlsxValue->set_field(key);
        (*processed_value_map)[key] = *xlsxValue;
    }
    XlsxValue* xlsxValue = &processed_value_map->at(key);

    if (!xlsxValue->struct_value().contains(sub_key)) {
        XlsxValue* structValue = new XlsxValue();
        structValue->set_field(sub_key);
        (*xlsxValue->mutable_struct_value())[sub_key] = *structValue;
    }

    XlsxValue* structValue = (XlsxValue*) &xlsxValue->struct_value().at(sub_key);
    structValue->add_value(value);
}

#pragma region 检表
// 检表配置
void xlsxcheck::LoadCheckLabel(const string& proto_path) {
    auto file = proto_path + "/check_label.txt";
    ifstream infile;
    infile.open(file, ios::in);
    stringstream buffer;
    buffer << infile.rdbuf();
    infile.close();

    TOOL::CheckLabelArray msg;
    if (!google::protobuf::TextFormat::ParseFromString(buffer.str(), &msg)) {
        LOG_ERROR("[检表语句错误] check_label[" << buffer.str() << "]");
        has_error_ = true;
    }
    LOG_INFO(msg.Utf8DebugString());

    for (const auto& value : msg.check_label_array()) {
        TOOL::CheckLabelCommand* msg_command = new TOOL::CheckLabelCommand();
        if (!google::protobuf::TextFormat::ParseFromString(value.check_string(), msg_command)) {
            LOG_ERROR("[检表语句错误] check_label[" << value.check_string() << "]");
            has_error_ = true;
        }
        LOG_INFO(msg_command->Utf8DebugString());
        check_lable_map_[value.check_label()] = msg_command;
    }
}

// 检表配置
void xlsxcheck::CheckLabel() {
    map<string, map<string, AllUniqueData>> cache_unique;

    for (const auto& [k, v] : xlsx_data_map_) {
        LOG_INFO("[CHECK LABEL] " << k);
        vector<int> key_list;
        for (const auto& [k1, v1] : v->checklabel_map()) {
            auto key = k1;
            auto command = v1;

            if (command.key()) {
                key_list.push_back(key);
            }
            if (command.data_not_empty()) {
                CLC_data_not_empty(key, v);
            }
            if (command.data_unique()) {
                CLC_data_unique(key, v, command);
            }
            if (!command.unique().empty()) {
                CLC_unique(k,cache_unique, key, v, command);
            }
            if (command.le_right()) {
                CLC_right(key, v, command, true);
            }
            if (command.lt_right()) {
                CLC_right(key, v, command, false);
            }
            if (command.check_from_right()) {
                CLC_check_from_right(key, v, command);
            }
            if (!command.unique_field().empty()) {
                CLC_unique_field(key, v, command);
            }
            if (command.range_size() > 0) {
                CLC_range(key, v, command);
            }
            if (command.num_size() > 0) {
                CLC_num(key, v, command);
            }
            if (command.ref_size() > 0) {
                CLC_ref(key, v, command);
            }
        }
        CLC_key(v, key_list);

        if (!has_error_)
            LOG_INFO("[CHECK LABEL] " << k << " SUCCESS");
    }
}

// 主键检测
void xlsxcheck::CLC_key(const XlsxData* xlsx_data, vector<int>& key_list) {
    if (key_list.size() == 0) return;

    map<string, int> cache;
    for (int i = 0; i < xlsx_data->original_value_list().size(); ++i) {
        string union_key;
        for (int index = 0; index < key_list.size(); ++index) {
            if (index == 0) {
                union_key = xlsx_data->original_value_list(i).values(index);
            } else {
                union_key = union_key + "|" + xlsx_data->original_value_list(i).values(index);
            }
        }

        int line = i + 4;
        if (cache.find(union_key) != cache.end()) {
            LOG_ERROR("[检表语句错误] 表[" << xlsx_data->xlsxname() << "] 行[" << cache[union_key] << "#" << line << "] 主键重复[" << union_key << "]");
            has_error_ = true;
        } else {
            cache[union_key] = line;
        }
    }
}

// 检查字段是否不为空
void xlsxcheck::CLC_data_not_empty(int index, const XlsxData* xlsx_data) {
    for (int i = 0; i < xlsx_data->original_value_list().size(); ++i) {
        int line = i + 4;
        if (xlsx_data->original_value_list(i).values(index).empty()) {
            LOG_ERROR("[检表语句错误] 表[" << xlsx_data->xlsxname() << "] 行[" << line << "] 列[" << CommonUtil::FormatCol(index) << "] 字段不能为空");
            has_error_ = true;
        }
    }
}

// 检查字段内容唯一性
void xlsxcheck::CLC_data_unique(int index, const XlsxData* xlsx_data, const CheckLabelCommand& command) {
    map<string, int> cache;

    for (int i = 0; i < xlsx_data->original_value_list().size(); ++i) {
        int line = i + 4;
        auto value = xlsx_data->original_value_list(i).values(index);
        if (value.empty() && command.empty_skip()) continue; // 字段为空可跳过检查

        // &分离数组支持
        auto list_ = CommonUtil::stringSplit(value, "&");
        for (const auto &item : list_) {
            if (cache.find(item) != cache.end()) {
                LOG_ERROR("[检表语句错误] 表[" << xlsx_data->xlsxname() << "] 行[" << cache[item] << "#" << line << "] 列[" << CommonUtil::FormatCol(index) << "] 字段内容不唯一[" << item << "]");
                has_error_ = true;
            } else {
                cache[item] = line;
            }
        }
    }
}

// 多表联合检查字段唯一性
void xlsxcheck::CLC_unique(const string& table, map<string, map<string, AllUniqueData>>& cache_unique, int index, const XlsxData* xlsx_data, const CheckLabelCommand& command) {
    if (cache_unique.find(command.unique()) == cache_unique.end()) {
        map<string, AllUniqueData> map;
        cache_unique[command.unique()] = map;
    }
    map<string, AllUniqueData> cache = cache_unique[command.unique()];
    for (int i = 0; i < xlsx_data->original_value_list().size(); ++i) {
        int line = i + 4;
        auto value = xlsx_data->original_value_list(i).values(index);
        if (value.empty() && command.empty_skip()) continue; // 字段为空可跳过检查

        // &分离数组支持
        auto list_ = CommonUtil::stringSplit(value, "&");
        for (const auto &item : list_) {
            if (cache.find(item) != cache.end()) {
                LOG_ERROR("[检表语句错误] 表[" << xlsx_data->xlsxname() << "] table_name[" << cache[item].tabel_name << "#" << table << "行[" << cache[item].row << "#" << line << "] 列[" << CommonUtil::FormatCol(cache[item].col) << "#" << CommonUtil::FormatCol(index) << "] 多表联合检查字段唯一性[" << item << "]");
                has_error_ = true;
            } else {
                AllUniqueData data;
                data.tabel_name = table;
                data.row = line;
                data.col = index + 1;
                cache[item] = data;
            }
        }
    }
}

// 小于等于右边字段 | 小于右边字段
void xlsxcheck::CLC_right(int index, const XlsxData* xlsx_data, const CheckLabelCommand& command, bool equal) {
    for (int i = 0; i < xlsx_data->original_value_list().size(); ++i) {
        int line = i + 4;

        auto value = xlsx_data->original_value_list(i).values(index);
        if (value.empty() && command.empty_skip()) continue; // 字段为空可跳过检查

        uint64 ret_next;
        auto value_next = xlsx_data->original_value_list(i).values(index + 1);
        safe_strtou64(value_next, &ret_next);

        // &分离数组支持
        auto list_ = CommonUtil::stringSplit(value, "&");
        for (const auto &item : list_) {
            // 转换成uint64对比
            uint64 ret;
            safe_strtou64(item, &ret);

            if (equal) {
                if (!(ret <= ret_next)) {
                    LOG_ERROR("[检表语句错误] 表[" << xlsx_data->xlsxname() << "] 行[" << line << "] 列[" << CommonUtil::FormatCol(index) << "] 小于等于右边字段[" << item << "] <= [" << value_next << "]");
                    has_error_ = true;
                }
            } else {
                if (!(ret < ret_next)) {
                    LOG_ERROR("[检表语句错误] 表[" << xlsx_data->xlsxname() << "] 行[" << line << "] 列[" << CommonUtil::FormatCol(index) << "] 小于右边字段[" << item << "] < [" << value_next << "]");
                    has_error_ = true;
                }
            }
        }
    }
}

// 按值进行检查
void xlsxcheck::CLC_check_from_right(int index, const XlsxData* xlsx_data, const CheckLabelCommand& command) {
    for (int i = 0; i < xlsx_data->original_value_list().size(); ++i) {
        int line = i + 4;
        auto value = xlsx_data->original_value_list(i).values(index);
        auto value_next = xlsx_data->original_value_list(i).values(index + 1);

        if (value.empty() || value_next.empty()) continue;

        TOOL::CheckLabelCommand msg_command;
        if (!google::protobuf::TextFormat::ParseFromString(value_next, &msg_command)) {
            LOG_ERROR("[检表语句错误] 表[" << xlsx_data->xlsxname() << "] 行[" << line << "] 列[" << CommonUtil::FormatCol(index) << "] 语句格式[" << value_next << "]");
            has_error_ = true;
        }

        if (command.range_size() > 0) {
            CLC_range_one(i, index, xlsx_data, command);
        }
        if (msg_command.ref_size() > 0) {
            CLC_ref_one(i, index, xlsx_data, msg_command);
        }
    }
}

// 检查本列字段是否只含有唯一的字段
void xlsxcheck::CLC_unique_field(int index, const XlsxData* xlsx_data, const CheckLabelCommand& command) {
    map<string, int> cache;
    for (int i = 0; i < xlsx_data->original_value_list().size(); ++i) {
        int line = i + 4;
        auto value = xlsx_data->original_value_list(i).values(index);
        if (value.empty()) continue;

        // &分离数组支持
        auto list_ = CommonUtil::stringSplit(value, "&");
        for (const auto &item : list_) {
            if (item == command.unique_field()) {
                if (cache.find(item) != cache.end()) {
                    LOG_ERROR("[检表语句错误] 表[" << xlsx_data->xlsxname() << "] 行[" << cache[item] << "#" << line << "] 列[" << CommonUtil::FormatCol(index) << "] 检查本列字段是否只含有唯一的字段[" << item << "]");
                    has_error_ = true;
                } else {
                    cache[item] = line;
                }
            }
        }
    }
}

// 数值范围检查(简单)
void xlsxcheck::CLC_range_one(int i, int index, const XlsxData* xlsx_data, const CheckLabelCommand& command) {
    int line = i + 4;
    auto value = xlsx_data->original_value_list(i).values(index);
    if (value.empty() && command.empty_skip()) return; // 字段为空可跳过检查

    // &分离数组支持
    auto list_ = CommonUtil::stringSplit(value, "&");
    for (const auto &item : list_) {
        auto exist = false;
        for (const auto& range : command.range()) {
            uint64 ret;
            safe_strtou64(item, &ret);

            auto list = CommonUtil::stringSplit(range, "->");
            if (list.size() != 2) {
                break;
            } else {
                if (list[0] != "#") {
                    uint64 min;
                    safe_strtou64(list[0], &min);
                    if (!(min <= ret)) {
                        continue;
                    }
                }
                if (list[1] != "#") {
                    uint64 max;
                    safe_strtou64(list[1], &max);
                    if (!(ret < max)) {
                        continue;
                    }
                }
                exist = true;
                break;
            }
        }
        if (!exist) {
            LOG_ERROR("[检表语句错误] 表[" << xlsx_data->xlsxname() << "] 行[" << line << "] 列[" << CommonUtil::FormatCol(index) << "] 检查语句[" << command.Utf8DebugString() << "] 数值范围检查[" << item << "]");
            has_error_ = true;
        }
    }
}

// 数值范围检查(简单)
void xlsxcheck::CLC_range(int index, const XlsxData* xlsx_data, const CheckLabelCommand& command) {
    for (int i = 0; i < xlsx_data->original_value_list().size(); ++i) {
        CLC_range_one(i, index, xlsx_data, command);
    }
}

// 数值范围检查(带适用字段)
void xlsxcheck::CLC_num(int index, const XlsxData* xlsx_data, const CheckLabelCommand& command) {
    for (int i = 0; i < xlsx_data->original_value_list().size(); ++i) {
        int line = i + 4;
        auto value = xlsx_data->original_value_list(i).values(index);
        if (value.empty() && command.empty_skip()) continue; // 字段为空可跳过检查

        // &分离数组支持
        auto list_ = CommonUtil::stringSplit(value, "&");
        for (const auto &item : list_) {
            for (const auto& num : command.num()) {
                if (!num.apply().empty()) {
                    auto last_value = xlsx_data->original_value_list(i).values(index - 1);
                    if (num.apply() != last_value) continue;
                }

                auto exist = true;
                uint64 ret;
                safe_strtou64(item, &ret);

                auto list = CommonUtil::stringSplit(num.range(), "->");
                if (list.size() == 2) {
                    if (list[0] != "#") {
                        uint64 min;
                        safe_strtou64(list[0], &min);
                        if (!(min <= ret)) {
                            exist = false;
                        }
                    }
                    if (list[1] != "#") {
                        uint64 max;
                        safe_strtou64(list[1], &max);
                        if (!(ret < max)) {
                            exist = false;
                        }
                    }
                }

                if (!exist) {
                    LOG_ERROR("[检表语句错误] 表[" << xlsx_data->xlsxname() << "] 行[" << line << "] 列[" << CommonUtil::FormatCol(index) << "] 检查语句[" << command.Utf8DebugString() << "] 数值范围检查[" << item << "]");
                    has_error_ = true;
                }
            }
        }
    }
}

// 表字段引用约束
void xlsxcheck::CLC_ref_one(int i, int index, const XlsxData* xlsx_data, const CheckLabelCommand& command) {
    int line = i + 4;
    auto value = xlsx_data->original_value_list(i).values(index);
    if (value.empty() && command.empty_skip()) return; // 字段为空可跳过检查

    // &分离数组支持
    auto list_ = CommonUtil::stringSplit(value, "&");
    for (const auto &item: list_) {
        for (const auto &ref: command.ref()) {
            if (!ref.apply().empty()) {
                auto last_value = xlsx_data->original_value_list(i).values(index - 1);
                if (ref.apply() != last_value) continue;
            }

            bool exist = false;
            auto *target_xlsx_data = xlsx_data_map_[ref.table()];
            if (!target_xlsx_data) {
                LOG_ERROR("[检表语句错误] 表[" << xlsx_data->xlsxname() << "] 行[" << line << "] 列["
                                        << CommonUtil::FormatCol(index) << "] 表字段引用约束表名字错误[" << ref.table() << "]");
                has_error_ = true;
                continue;
            }
            auto &value_map = target_xlsx_data->processed_value_list(0).value_map();
            if (!value_map.contains(ref.field())) {
                LOG_ERROR("[检表语句错误] 表[" << xlsx_data->xlsxname() << "] 行[" << line << "] 列["
                                        << CommonUtil::FormatCol(index) << "] 表字段引用约束表字段错误[" << ref.field() << "]");
                has_error_ = true;
                continue;
            }

            for (const auto &processed_value: target_xlsx_data->processed_value_list()) {
                if (!processed_value.value_map().contains(ref.field())) {
                    LOG_ERROR(ref.field());
                    continue;
                }
                auto &xlsx_value = processed_value.value_map().at(ref.field());
                // todo 可以用map来优化遍历速度
                for (const auto &target_value: xlsx_value.value()) {
                    if (target_value == item) {
                        exist = true;
                        break;
                    }
                }
                if (exist) break;
            }

            if (!exist) {
                LOG_ERROR("[检表语句错误] 表[" << xlsx_data->xlsxname() << "] 行[" << line << "] 列["
                                        << CommonUtil::FormatCol(index) << "] 表字段引用约束[" << item << "] 检表语句["
                                        << command.Utf8DebugString() << "]");
                has_error_ = true;
            }
        }
    }
}

// 表字段引用约束
void xlsxcheck::CLC_ref(int index, const XlsxData* xlsx_data, const CheckLabelCommand& command) {
    for (int i = 0; i < xlsx_data->original_value_list().size(); ++i) {
        CLC_ref_one(i, index, xlsx_data, command);
    }
}
#pragma endregion 检表