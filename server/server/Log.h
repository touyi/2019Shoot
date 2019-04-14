#pragma once
#include<string>
using std::string;
class LogManager
{
private:
    LogManager() = default;
    ~LogManager() = default;
public:
    static void Error(string msg);
    static void Debug(string msg);
    static void Log(string msg);
};

class IClassInfo {
public:
    virtual ~IClassInfo() {}
    virtual operator string() = 0;
};

void DEBUG_LOG(const char *msg, ...);


