//
// Created by liuyi on 2021/6/23.
//

#ifndef XLS2PB_CPP_TSV2PB_H
#define XLS2PB_CPP_TSV2PB_H

#include <iostream>
#include <vector>
#include <map>
#include <google/protobuf/message.h>
#include <google/protobuf/compiler/importer.h>

using namespace std;
struct ReflectionMessageData;
class xlsxcheck;

class tsv2pb {
public:
    tsv2pb();
    ~tsv2pb();

    int Export(const string& proto_path, const string& tsv_path, const string& store_path, const string& store_suffix, xlsxcheck& xlsx_check);

private:
    void ParseProto(const string& proto_path);
    void ParseMessage(const google::protobuf::Descriptor* descriptor, const string& relative_name);
    void ParseEnum(const google::protobuf::EnumDescriptor* descriptor, const string& relative_name);

    void PreProcessValue(xlsxcheck& xlsx_check);

    void ReflectionMessage(const string &tsv_path, const string &store_path, const string &store_suffix, xlsxcheck& xlsx_check);
    void SetMessageValue(google::protobuf::Message *message, const google::protobuf::FieldDescriptor *field, const string& value, int row, const string& col, const string& tsv_name);

private:
    google::protobuf::compiler::Importer* importer_;
    // proto消息数据
    vector<ReflectionMessageData> proto_message_data_;
    // proto子消息数据
    map<string, ReflectionMessageData> proto_sub_msg_data_;
    // proto enum数据
    map<string, map<string, int>> proto_enum_data_;

    bool has_error_;
};


#endif //XLS2PB_CPP_TSV2PB_H
