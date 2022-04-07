#pragma once

#include <stdint.h>

#include <cstring>
#include <iostream>
#include <string>

#include "singleton.h"

class MD5 : public Singleton<MD5>
{
public:
    MD5();
    explicit MD5(const std::string&);
    MD5(const char*, unsigned int);

public:
    void Init();
    void Update(const std::string&);
    void Update(unsigned char const*, unsigned int);
    void Final();
    std::string Hash(const std::string&);
    std::string HashBinary(const std::string&);
    std::string Hash(const char*, unsigned int);
    std::string HashBinary(const char*, unsigned int);
    std::string HashFile(const char* filename);
#if _WIN32
    std::string HashFile(const wchar_t* filename);
#endif
    std::string HashFile(std::FILE* file);
    std::string Out();
    std::string OutBinary();
    char XChar(unsigned char);
    void Echo();

    uint64_t Hash64(const char* str, int len);
    uint32_t Hash32(const char* str, int len);
    int DValue(char c);

private:
    void Reverse(unsigned char*, unsigned int);
    void Transform(unsigned int[4], unsigned int const[16]);

private:
    unsigned int buf_[4] = {};
    unsigned int bits_[2] = {};
    unsigned char in_[64] = {};
};
