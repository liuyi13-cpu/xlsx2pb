//
// Created by liuyi on 2021/6/24.
//

#include <google/protobuf/stubs/strutil.h>
#include "CommonUtil.h"
#include <regex>
#include <sstream>

#if _WIN32
#include <locale>
#include <codecvt>
#endif

bool CommonUtil::stringStartsWith(const string& s, const string& sub) {
    return s.find(sub) == 0;
}

bool CommonUtil::stringEndsWith(const string& s, const string& sub) {
    return s.rfind(sub) == s.length() - sub.length();
}

bool CommonUtil::stringContain(const string& s, const string& sub) {
    return s.find(sub) != string::npos;
}

std::vector<string> CommonUtil::stringSplit(const string& s, const char* delim, bool skip_empty) {
    return google::protobuf::Split(s, delim, skip_empty);
}

string& CommonUtil::CleanXLSXString(string& str, bool reduceEmpty)
{
    if (reduceEmpty) {
        str = ClearHeadTail(str, " ");
        str = ClearHeadTail(str, "\r");
        str = ClearHeadTail(str, "\r\n");
        str = ClearHeadTail(str, "\n");
    }
    str = regex_replace(str, regex("\\\r\\\n"), "\\n");
    str = regex_replace(str, regex("\\\r"), "\\n");
    str = regex_replace(str, regex("\\\n"), "\\n");
    return str;
}

// 去掉收尾特定字符串
string& CommonUtil::ClearHeadTail(string& str, const char* s)
{
    if (str.empty())
    {
        return str;
    }

    str.erase(0,str.find_first_not_of(s));
    str.erase(str.find_last_not_of(s) + 1);
    return str;
}

#if _WIN32
std::wstring CommonUtil::string2wstring(const string& s) {
    std::wstring_convert<std::codecvt_utf8<wchar_t>> converter;
    return converter.from_bytes(s);
}
#endif

string CommonUtil::FormatCol(int col) {
    char b = 'A' + col;
    stringstream ss;
    ss << b;
    return ss.str();
}

bool CommonUtil::isStruct(const string& s, string& out) {
    auto index = s.find("#");
    if (index == string::npos){
        out = s;
        return false;
    }

    auto array = stringSplit(s, "#");
    if (array.size() > 2) {
        out = s;
        return true;
    }

    google::protobuf::int32 ret;
    if (google::protobuf::safe_strto32(array[1], &ret)) {
        out = s.substr(0, index);
        return false;
    }

    out = s;
    return true;
}



