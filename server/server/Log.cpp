#include "Log.h"
#include<iostream>
#include<sstream>
using namespace std;



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
