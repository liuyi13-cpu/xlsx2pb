//
// Created by liuyi on 2021/6/21.
//

#include <iostream>
#include <OpenXLSX.hpp>
#include <regex>
#include "xlsx2tsv.h"
#include "util/md5.h"
#include "util/logger.h"
#include "util/CommonUtil.h"

using namespace OpenXLSX;

xlsx2tsv::xlsx2tsv() {
}

xlsx2tsv::~xlsx2tsv() {
}

int xlsx2tsv::Export(const string& xlsx_input, const string& tsv_output) {
    has_error_ = false;
    unordered_set<string> sameNameCheck;
    for (const auto& p: filesystem::recursive_directory_iterator(xlsx_input)) {
        if (p.is_directory()) continue;

        auto filePath = p.path();
        auto fileName = filePath.filename();
        if (CommonUtil::stringStartsWith(fileName.string(), "~$")) continue; // 打开的文档
        if (sameNameCheck.contains(fileName.string())) {
            LOG_ERROR("[表名字重复]" << fileName);
            has_error_ = true;
            break;
        }
        sameNameCheck.insert(fileName.string());
        if (filePath.extension() == ".xlsx") {
            LOG_INFO("convert xlsx:"  << fileName);
            if (check_md5_same(tsv_output, filePath)) {
                LOG_INFO("convert xlsx:" << fileName << " MD5 is same");
            } else {
                ExportOneXlsx(tsv_output, filePath);
                if (!has_error_){
                    save_md5(tsv_output, filePath);
                }
                LOG_INFO("convert xlsx:" << fileName << " OK!!!");
            }
        } else if (filePath.extension() == ".csv") {
            LOG_INFO("convert csv:"  << fileName);
            if (check_md5_same(tsv_output, filePath)) {
                LOG_INFO("convert csv:" << fileName << " MD5 is same");
            } else {
                ExportOneCsv(tsv_output, filePath);
                if (!has_error_){
                    save_md5(tsv_output, filePath);
                }
                LOG_INFO("convert csv:" << fileName << " OK!!!");
            }
        } else {
            LOG_ERROR("不支持的文件格式" << filePath);
        }
    }

    if (has_error_) return -1;
    return 0;
}

void xlsx2tsv::ExportOneXlsx(const string& tsv_output, const filesystem::path& filePath) {
    XLDocument doc;
    doc.open(filePath.string());
    auto wks = doc.workbook().sheet(1).get<XLWorksheet>();
    auto rowCount =  wks.rowCount();
    auto columnCount = wks.columnCount();
    LOG_INFO("Rows:" << rowCount << " Columns:" << columnCount);

    ostringstream oss;
    for (int i = 1; i <= rowCount; ++i) {
        for (int j = 1; j <= columnCount; ++j) {
            auto cell = wks.cell(i, j);
            switch (cell.value().type()) {
                case XLValueType::Float:
                    oss << cell.value().get<double>();
                    break;
                case XLValueType::Integer:
                    oss << cell.value().get<int64_t>();
                    break;
                case XLValueType::Boolean:
                    oss << cell.value().get<bool>();
                    break;
                case XLValueType::String:
                {
                    auto string1 = cell.value().get<string>();
                    string1 = CommonUtil::CleanXLSXString(string1, i == 1);
                    oss << string1;
                    break;
                }
                default:
                    break;
            }
            if (j != columnCount) {
                oss << "\t";
            }
        }
        oss << "\n";
    }

#if _WIN32
    auto fileName = filePath.stem().wstring();
    auto outputFilePath = CommonUtil::string2wstring(tsv_output) + L"/" + fileName + L".tsv";
#else
    auto fileName = filePath.stem().string();
    auto outputFilePath = tsv_output + "/" + fileName + ".tsv";
#endif
    ofstream outfile;
    outfile.open(outputFilePath.c_str(), ios::out | ios::trunc);
    outfile << oss.str();
    outfile.close();
}

void xlsx2tsv::ExportOneCsv(const string& tsv_output, const filesystem::path& filePath) {
    ifstream infile;
    infile.open(filePath.c_str(), ios::in);
    if (!infile.good()) return;

#if _WIN32
    auto fileName = filePath.stem().wstring();
    auto outputFilePath = CommonUtil::string2wstring(tsv_output) + L"/" + fileName + L".tsv";
#else
    auto fileName = filePath.stem().string();
    auto outputFilePath = tsv_output + "/" + fileName + ".tsv";
#endif
    ofstream outfile;
    outfile.open(outputFilePath.c_str(), ios::out | ios::trunc);

    string line;
    auto is_first = true;
    while(getline(infile, line)) {
        if (is_first) {
            line = regex_replace(line, regex("\uFEFF"), ""); // 去UTF-8的BOM
            is_first = false;
        }
        line = regex_replace(line, regex(","), "\t");
        outfile << line << "\n";
    }
    infile.close();
    outfile.close();
}

bool xlsx2tsv::check_md5_same(const string& tsv_output, const filesystem::path& filePath) {
    auto is_same = false;
#if DEBUG
    return is_same;
#endif

    MD5 md5;
#if _WIN32
    auto md5_str = md5.HashFile(filePath.wstring().c_str());
    auto fileName = filePath.stem().wstring();
    auto outputFilePath = CommonUtil::string2wstring(tsv_output) + L"/" + fileName + L".md5";
#else
    auto md5_str = md5.HashFile(filePath.string().c_str());
    auto fileName = filePath.stem().string();
    auto outputFilePath = tsv_output + "/" + fileName + ".md5";
#endif

    if (!md5_str.empty()) {
        ifstream infile;
        infile.open(outputFilePath.c_str(), ios::in);
        if (infile.good()) {
            string line;
            getline(infile, line);
            infile.close();
            is_same = line == md5_str;
        }
    }
    return is_same;
}

void xlsx2tsv::save_md5(const string& tsv_output, const filesystem::path& filePath) {
    MD5 md5;
#if _WIN32
    auto md5_str = md5.HashFile(filePath.wstring().c_str());
    auto fileName = filePath.stem().wstring();
    auto outputFilePath = CommonUtil::string2wstring(tsv_output) + L"/" + fileName + L".md5";
#else
    auto md5_str = md5.HashFile(filePath.string().c_str());
    auto fileName = filePath.stem().string();
    auto outputFilePath = tsv_output + "/" + fileName + ".md5";
#endif

    ofstream outfile;
    outfile.open(outputFilePath.c_str(), ios::out | ios::trunc);
    outfile << md5_str;
    outfile.close();
}
