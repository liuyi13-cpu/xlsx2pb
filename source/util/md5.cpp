#include "md5.h"

MD5::MD5()
{
    Init();
}
MD5::MD5(const std::string& str)
{
    Init();
    Update((unsigned char const*)str.c_str(), str.size());
}

MD5::MD5(const char* str, unsigned int len)
{
    Init();
    Update((unsigned char const*)str, len);
}

void MD5::Init()
{
    buf_[0] = 0x67452301;
    buf_[1] = 0xefcdab89;
    buf_[2] = 0x98badcfe;
    buf_[3] = 0x10325476;

    bits_[0] = 0;
    bits_[1] = 0;

    memset((void*)&in_, 0, sizeof(in_));
}

void MD5::Update(const std::string& str)
{
    Update((unsigned char const*)str.c_str(), str.size());
}

void MD5::Update(unsigned char const* str, unsigned int len)
{
    unsigned int t = bits_[0];
    if ((bits_[0] = t + ((unsigned int)len << 3)) < t) bits_[1]++;
    bits_[1] += len >> 29;
    t = (t >> 3) & 0x3f;
    if (t) {
        unsigned char* p = (unsigned char*)in_ + t;
        t = 64 - t;
        if (len < t) {
            memmove(p, str, len);
            return;
        }
        memmove(p, str, t);
        Reverse(in_, 16);
        Transform(buf_, (unsigned int*)in_);
        str += t;
        len -= t;
    }
    while (len >= 64) {
        memmove(in_, str, 64);
        Reverse(in_, 16);
        Transform(buf_, (unsigned int*)in_);
        str += 64;
        len -= 64;
    }
    memmove(in_, str, len);
}

void MD5::Final()
{
    unsigned int count = (bits_[0] >> 3) & 0x3f;
    unsigned char* p = in_ + count;
    *p++ = 0x80;
    count = 64 - 1 - count;
    if (count < 8) {
        memset(p, 0, count);
        Reverse(in_, 16);
        Transform(buf_, (unsigned int*)in_);
        memset(in_, 0, 56);
    } else
        memset(p, 0, count - 8);
    Reverse(in_, 14);
    ((unsigned int*)in_)[14] = bits_[0];
    ((unsigned int*)in_)[15] = bits_[1];
    Transform(buf_, (unsigned int*)in_);
    Reverse((unsigned char*)buf_, 4);
}

std::string MD5::Hash(const std::string& s_in)
{
    Init();
    Update(s_in);
    return Out();
}

std::string MD5::HashBinary(const std::string& s_in)
{
    Init();
    Update(s_in);

    return OutBinary();
}

#if _WIN32
std::string MD5::HashFile(const wchar_t* filename)
{
    if (filename == nullptr) return std::string();
    std::FILE* file = _wfopen(filename, L"rb");
    if (file == nullptr) return std::string();
    std::string res = HashFile(file);
    std::fclose(file);
    return res;
}
#endif

std::string MD5::HashFile(const char* filename)
{
    if (filename == nullptr) return std::string();
    std::FILE* file = std::fopen(filename, "rb");
    if (file == nullptr) return std::string();
    std::string res = HashFile(file);
    std::fclose(file);
    return res;
}

std::string MD5::HashFile(std::FILE* file)
{
    Init();
    unsigned char buff[BUFSIZ];
    size_t len = 0;
    while ((len = std::fread(buff, sizeof(char), BUFSIZ, file)) > 0) {
        Update(buff, len);
    }
    return Out();
}

std::string MD5::Hash(const char* s_in, unsigned int s_len)
{
    Init();
    Update((unsigned char const*)s_in, s_len);

    return Out();
}

uint64_t MD5::Hash64(const char* str, int len)
{
    uint64_t r = 0;
    std::string h = Hash(str, len);
    for (int i = 8; i < 24; i++) {
        r <<= 4;
        r += DValue(h[i]);
    }
    return r;
}

uint32_t MD5::Hash32(const char* str, int len)
{
    uint32_t r = 0;
    std::string h = Hash(str, len);
    for (int i = 8; i < 16; i++) {
        r <<= 4;
        r += DValue(h[i]);
    }
    return r;
}

int MD5::DValue(char c)
{
    if (c <= '9' && c >= '0')
        return c - '0';
    else if ((c >= 'a') && (c <= 'f'))
        return c - 'a' + 10;
    else if ((c >= 'A') && (c <= 'F'))
        return c - 'A' + 10;
    else
        return -1;
}

std::string MD5::HashBinary(const char* s_in, unsigned int s_len)
{
    Init();
    Update((unsigned char const*)s_in, s_len);

    return OutBinary();
}

std::string MD5::Out()
{
    Final();
    char s[32];
    unsigned char c;
    for (int i = 0; i < 16; ++i) {
        c = ((unsigned char*)buf_)[i];
        s[i * 2] = XChar((c & 0xf0) >> 4);
        s[i * 2 + 1] = XChar(c & 0x0f);
    }
    return std::string(s, 32);
}

std::string MD5::OutBinary()
{
    Final();
    char s[16];
    unsigned char c;
    for (int i = 0; i < 16; ++i) {
        c = ((unsigned char*)buf_)[i];
        s[i] = c;
    }
    return std::string(s, 16);
}

char MD5::XChar(unsigned char c)
{
    return c < 0x0a ? '0' + c : 'a' - 10 + c;
}

void MD5::Echo()
{
    std::cerr << Out() << std::endl;
}

void MD5::Reverse(unsigned char* str, unsigned int longs)
{
    unsigned int t;
    do {
        t = (unsigned int)((unsigned int)str[3] << 8 | str[2]) << 16 |
            ((unsigned int)str[1] << 8 | str[0]);
        *(unsigned int*)str = t;
        str += 4;
    } while (--longs);
}

#define F1(x, y, z) (z ^ (x & (y ^ z)))
#define F2(x, y, z) F1(z, x, y)
#define F3(x, y, z) (x ^ y ^ z)
#define F4(x, y, z) (y ^ (x | ~z))

#define MD5STEP(f, w, x, y, z, d, s) \
    do {                             \
        w += f(x, y, z) + d;         \
        w = w << s | w >> (32 - s);  \
        w += x;                      \
    } while (0)

void MD5::Transform(unsigned int str[4], unsigned int const sin[16])
{
    unsigned int a, b, c, d;

    a = str[0];
    b = str[1];
    c = str[2];
    d = str[3];

    MD5STEP(F1, a, b, c, d, sin[0] + 0xd76aa478, 7);
    MD5STEP(F1, d, a, b, c, sin[1] + 0xe8c7b756, 12);
    MD5STEP(F1, c, d, a, b, sin[2] + 0x242070db, 17);
    MD5STEP(F1, b, c, d, a, sin[3] + 0xc1bdceee, 22);
    MD5STEP(F1, a, b, c, d, sin[4] + 0xf57c0faf, 7);
    MD5STEP(F1, d, a, b, c, sin[5] + 0x4787c62a, 12);
    MD5STEP(F1, c, d, a, b, sin[6] + 0xa8304613, 17);
    MD5STEP(F1, b, c, d, a, sin[7] + 0xfd469501, 22);
    MD5STEP(F1, a, b, c, d, sin[8] + 0x698098d8, 7);
    MD5STEP(F1, d, a, b, c, sin[9] + 0x8b44f7af, 12);
    MD5STEP(F1, c, d, a, b, sin[10] + 0xffff5bb1, 17);
    MD5STEP(F1, b, c, d, a, sin[11] + 0x895cd7be, 22);
    MD5STEP(F1, a, b, c, d, sin[12] + 0x6b901122, 7);
    MD5STEP(F1, d, a, b, c, sin[13] + 0xfd987193, 12);
    MD5STEP(F1, c, d, a, b, sin[14] + 0xa679438e, 17);
    MD5STEP(F1, b, c, d, a, sin[15] + 0x49b40821, 22);

    MD5STEP(F2, a, b, c, d, sin[1] + 0xf61e2562, 5);
    MD5STEP(F2, d, a, b, c, sin[6] + 0xc040b340, 9);
    MD5STEP(F2, c, d, a, b, sin[11] + 0x265e5a51, 14);
    MD5STEP(F2, b, c, d, a, sin[0] + 0xe9b6c7aa, 20);
    MD5STEP(F2, a, b, c, d, sin[5] + 0xd62f105d, 5);
    MD5STEP(F2, d, a, b, c, sin[10] + 0x02441453, 9);
    MD5STEP(F2, c, d, a, b, sin[15] + 0xd8a1e681, 14);
    MD5STEP(F2, b, c, d, a, sin[4] + 0xe7d3fbc8, 20);
    MD5STEP(F2, a, b, c, d, sin[9] + 0x21e1cde6, 5);
    MD5STEP(F2, d, a, b, c, sin[14] + 0xc33707d6, 9);
    MD5STEP(F2, c, d, a, b, sin[3] + 0xf4d50d87, 14);
    MD5STEP(F2, b, c, d, a, sin[8] + 0x455a14ed, 20);
    MD5STEP(F2, a, b, c, d, sin[13] + 0xa9e3e905, 5);
    MD5STEP(F2, d, a, b, c, sin[2] + 0xfcefa3f8, 9);
    MD5STEP(F2, c, d, a, b, sin[7] + 0x676f02d9, 14);
    MD5STEP(F2, b, c, d, a, sin[12] + 0x8d2a4c8a, 20);

    MD5STEP(F3, a, b, c, d, sin[5] + 0xfffa3942, 4);
    MD5STEP(F3, d, a, b, c, sin[8] + 0x8771f681, 11);
    MD5STEP(F3, c, d, a, b, sin[11] + 0x6d9d6122, 16);
    MD5STEP(F3, b, c, d, a, sin[14] + 0xfde5380c, 23);
    MD5STEP(F3, a, b, c, d, sin[1] + 0xa4beea44, 4);
    MD5STEP(F3, d, a, b, c, sin[4] + 0x4bdecfa9, 11);
    MD5STEP(F3, c, d, a, b, sin[7] + 0xf6bb4b60, 16);
    MD5STEP(F3, b, c, d, a, sin[10] + 0xbebfbc70, 23);
    MD5STEP(F3, a, b, c, d, sin[13] + 0x289b7ec6, 4);
    MD5STEP(F3, d, a, b, c, sin[0] + 0xeaa127fa, 11);
    MD5STEP(F3, c, d, a, b, sin[3] + 0xd4ef3085, 16);
    MD5STEP(F3, b, c, d, a, sin[6] + 0x04881d05, 23);
    MD5STEP(F3, a, b, c, d, sin[9] + 0xd9d4d039, 4);
    MD5STEP(F3, d, a, b, c, sin[12] + 0xe6db99e5, 11);
    MD5STEP(F3, c, d, a, b, sin[15] + 0x1fa27cf8, 16);
    MD5STEP(F3, b, c, d, a, sin[2] + 0xc4ac5665, 23);

    MD5STEP(F4, a, b, c, d, sin[0] + 0xf4292244, 6);
    MD5STEP(F4, d, a, b, c, sin[7] + 0x432aff97, 10);
    MD5STEP(F4, c, d, a, b, sin[14] + 0xab9423a7, 15);
    MD5STEP(F4, b, c, d, a, sin[5] + 0xfc93a039, 21);
    MD5STEP(F4, a, b, c, d, sin[12] + 0x655b59c3, 6);
    MD5STEP(F4, d, a, b, c, sin[3] + 0x8f0ccc92, 10);
    MD5STEP(F4, c, d, a, b, sin[10] + 0xffeff47d, 15);
    MD5STEP(F4, b, c, d, a, sin[1] + 0x85845dd1, 21);
    MD5STEP(F4, a, b, c, d, sin[8] + 0x6fa87e4f, 6);
    MD5STEP(F4, d, a, b, c, sin[15] + 0xfe2ce6e0, 10);
    MD5STEP(F4, c, d, a, b, sin[6] + 0xa3014314, 15);
    MD5STEP(F4, b, c, d, a, sin[13] + 0x4e0811a1, 21);
    MD5STEP(F4, a, b, c, d, sin[4] + 0xf7537e82, 6);
    MD5STEP(F4, d, a, b, c, sin[11] + 0xbd3af235, 10);
    MD5STEP(F4, c, d, a, b, sin[2] + 0x2ad7d2bb, 15);
    MD5STEP(F4, b, c, d, a, sin[9] + 0xeb86d391, 21);

    str[0] += a;
    str[1] += b;
    str[2] += c;
    str[3] += d;
}
#undef MD5STEP

#undef F4
#undef F3
#undef F2
#undef F1
