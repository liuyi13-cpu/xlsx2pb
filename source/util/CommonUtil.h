//
// Created by liuyi on 2021/6/24.
//

#ifndef XLS2PB_CPP_COMMONUTIL_H
#define XLS2PB_CPP_COMMONUTIL_H


#include <string>
using namespace std;

class CommonUtil {
public:
    static bool stringStartsWith(const string& s, const string& sub);
    static bool stringEndsWith(const string& s, const string& sub);
    static bool stringContain(const string& s, const string& sub);
    static vector<string> stringSplit(const string& s, const char* delim, bool skip_empty = true);
    static string& CleanXLSXString(string &str, bool reduceEmpty);
    static string& ClearHeadTail(string &str, const char *s);
#if _WIN32
    static wstring string2wstring(const string &s);
#endif

    static string FormatCol(int row);
    static bool isStruct(const string& s, string& out);
};


#endif //XLS2PB_CPP_COMMONUTIL_H
