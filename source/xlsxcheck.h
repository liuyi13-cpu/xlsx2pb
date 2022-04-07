//
// Created by liuyi on 2021/8/1.
//

#ifndef XLS2PB_CPP_XLSXCHECK_H
#define XLS2PB_CPP_XLSXCHECK_H

#include <string>
#include <map>
#include "proto/tools.pb.h"

namespace TOOL {
    class CheckLabelCommand;
    class XlsxData;
    class XlsxValue;
}
namespace google {
    namespace protobuf {
        template<typename Key, typename T>
        class Map;
    }
}
struct AllUniqueData;

using namespace std;
using namespace TOOL;

class xlsxcheck {
public:
    xlsxcheck();
    ~xlsxcheck();

    int Export(const string& proto_path, const string& tsv_path);
    map<string, XlsxData*> xlsx_data_map() const { return xlsx_data_map_; };

private:
    void LoadTsv(const std::filesystem::path& filePath);
    void ParseValue(string& key, const string& value, google::protobuf::Map<string, XlsxValue>* processed_value_map);
    void ParseStructValue(string& key, const string& value, google::protobuf::Map<string, XlsxValue>* processed_value_map);

    void CheckLabel();
    void LoadCheckLabel(const string& proto_path);

    // CLC = CheckLabelCommand
    void CLC_key(const XlsxData* xlsx_data, vector<int>& key_list);
    void CLC_data_not_empty(int index, const XlsxData* xlsx_data);
    void CLC_data_unique(int index, const XlsxData* xlsx_data, const CheckLabelCommand& command);
    void CLC_unique(const string& table, map<string, map<string, AllUniqueData>>& cache_unique, int index, const XlsxData* xlsx_data, const CheckLabelCommand& command);
    void CLC_right(int index, const XlsxData *xlsx_data, const CheckLabelCommand& command, bool equal);
    void CLC_check_from_right(int index, const XlsxData* xlsx_data, const CheckLabelCommand& command);
    void CLC_unique_field(int index, const XlsxData* xlsx_data, const CheckLabelCommand& command);

    void CLC_range_one(int i, int index, const XlsxData* xlsx_data, const CheckLabelCommand& command);
    void CLC_range(int index, const XlsxData *xlsx_data, const CheckLabelCommand& command);

    void CLC_num(int index, const XlsxData* xlsx_data, const CheckLabelCommand& command);

    void CLC_ref_one(int i, int index, const XlsxData* xlsx_data, const CheckLabelCommand& command);
    void CLC_ref(int index, const XlsxData* xlsx_data, const CheckLabelCommand& command);

private:
    map<string, TOOL::CheckLabelCommand*> check_lable_map_;
    map<string, XlsxData*> xlsx_data_map_;
    bool has_error_;
};

#endif //XLS2PB_CPP_XLSXCHECK_H
