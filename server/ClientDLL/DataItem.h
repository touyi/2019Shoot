#pragma once
#include"client.h"
class DataItem
{
public:
    int protocol;
    int bufferLength;
private:
    char buffer[MAX_NUM_BUF];
public:
    void* GetBuffer();
    long long SetBuffer(void* ptr);
    int ByteRead(void* ptr) {
        return (int)((char*)ptr)[0];
    }
    DataItem();
    ~DataItem();
};

