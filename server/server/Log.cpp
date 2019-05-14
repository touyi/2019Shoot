#include "Log.h"
#include<iostream>
#include<sstream>
using namespace std;

#include <stdio.h>
#include <time.h>
#include <string.h>
#include <stdarg.h>

void LogManager::Error(string msg)
{
    cout << "[Error]:" << msg << endl;
}

void LogManager::Debug(string msg)
{
    cout << "[Debug]:" << msg << endl;
}


void LogManager::Log(string msg)
{
    cout << "[Log]:" << msg << endl;
}

void DEBUG_LOG(const char *msg, ...) {
    char message[256] = { 0 };
    va_list args;
    va_start(args, msg);
    vsprintf(message, msg, args);
    va_end(args);
    LogManager::Debug(message);
}
