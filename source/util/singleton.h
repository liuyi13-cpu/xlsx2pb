#pragma once

template <typename T>
class Singleton
{
public:
    Singleton(){};
    static T& Instance()
    {
        static T instance_;
        return instance_;
    }

private:
    Singleton(const Singleton&);
    Singleton& operator=(const Singleton&);
};
