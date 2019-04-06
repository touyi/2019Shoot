#pragma once
#include"client.h"
class DataItem
{
public:
    int protocol;
    char buffer[MAX_NUM_BUF];
public:
    DataItem();
    ~DataItem();
};

