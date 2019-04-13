#include "DataItem.h"



void * DataItem::GetBuffer()
{
    return buffer;
}

long long DataItem::SetBuffer(void * ptr)
{
    if (ptr != NULL) {
        memcpy(ptr, this->buffer, MAX_NUM_BUF);
        return (long)ptr;
    }
}

DataItem::DataItem()
{
}


DataItem::~DataItem()
{
}
