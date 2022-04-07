//
// Created by liuyi on 2021/6/21.
//

#ifndef XLS2PB_CPP_XLSX2TSV_H
#define XLS2PB_CPP_XLSX2TSV_H

#include <filesystem>

using namespace std;

class xlsx2tsv {
public:
    xlsx2tsv();
    ~xlsx2tsv();

    int Export(const string& xlsx_input, const string& tsv_output);

private:
    void ExportOne(const string& tsv_output, const filesystem::path& filePath);
    bool check_md5_same(const string& tsv_output, const filesystem::path& filePath);
    void save_md5(const string &tsv_output, const filesystem::path &filePath);

private:
    bool has_error_;
};


#endif //XLS2PB_CPP_XLSX2TSV_H
