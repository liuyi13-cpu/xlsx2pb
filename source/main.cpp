#include <iostream>
#include "util/logger.h"
#include "util/CommonUtil.h"
#include "xlsx2tsv.h"
#include "tsv2pb.h"
#include "xlsxcheck.h"

using namespace std;

const string VER = "0.0.1";

std::map<string, string> arguments_list_;

int ParseArguments(int argc, char* argv[]) {
    for (int i = 1; i < argc; ++i) {
        auto list = CommonUtil::stringSplit(argv[i], "=");
        auto key = list[0].substr(list[0].find("--") + 2);
        if (list.size() == 2) {
            arguments_list_[key] = list[1];
        } else {
            arguments_list_[key] = key;
        }
    }
    return 0;
}

int main(int argc, char* argv[]) {
#ifdef DEBUG
#ifdef UNIX
    string xlsx_input = "/Users/liuyi/xlsx2pb/source/res/excel";
    string tsv_path = "/Users/liuyi/xlsx2pb/source/res/tsv";
    string proto_path = "/Users/liuyi/xlsx2pb/source/res/proto";
    string store_path = "/Users/liuyi/xlsx2pb/source/res/store";
    string store_suffix = "bytes";
#else
    string xlsx_input = "E:/project/b3/GameClientTools/xlsx2pb/source/res/excel";
    string tsv_path = "E:/project/b3/GameClientTools/xlsx2pb/source/res/tsv";
    string proto_path = "E:/project/b3/GameClientTools/xlsx2pb/source/res/proto";
    string store_path = "E:/project/b3/GameClientTools/xlsx2pb/source/res/store";
    string store_suffix = "bytes";
#endif

#else
    if (ParseArguments(argc, argv) < 0) {
        return -1;
    }
    auto xlsx_input = arguments_list_["xlsx_input"];
    auto tsv_path = arguments_list_["tsv_path"];
    auto proto_path = arguments_list_["proto_path"];
    auto store_path = arguments_list_["store_path"];
    auto store_suffix = arguments_list_["store_suffix"];

    if (arguments_list_.find("version") != arguments_list_.end()) {
        auto version = arguments_list_["version"];
        LOG_INFO("version: " + VER);
        return 0;
    }
#endif
    Log::Init("xls2pb_cpp.log", true);
    LOG_INFO("[xlsx_input] " + xlsx_input);
    LOG_INFO("[tsv_path] " + tsv_path);
    LOG_INFO("[proto_path] " + proto_path);
    LOG_INFO("[store_path] " + store_path);
    LOG_INFO("[store_suffix] " + store_suffix);

    try {
        int error;

        // 1 xlsx2tsv
        LOG_INFO("starting convert xlsx to tsv......");
        xlsx2tsv _xls2tsv;
        error = _xls2tsv.Export(xlsx_input, tsv_path);;
        if (error < 0) {
            return error;
        }

        // 2 check lable
        LOG_INFO("starting check lable......");
        xlsxcheck _xlsxcheck;
        error = _xlsxcheck.Export(proto_path, tsv_path);
        if (error < 0) {
            return error;
        }

        // 3 tsv2pb
        LOG_INFO("starting convert tsv to pb......");
        tsv2pb _tsv2pb;
        error = _tsv2pb.Export(proto_path, tsv_path, store_path, store_suffix, _xlsxcheck);
        if (error < 0) {
            return error;
        }
    } catch (exception const& e) {
        LOG_ERROR(e.what());
        return -1;
    } catch (...) {
        LOG_ERROR("Exception occurred");
        return -1;
    }
    return 0;
}
