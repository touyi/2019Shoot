#include "TypeCastHelper.h"
#include<memory.h>
#include<iostream>
using std::cout;
using std::endl;
union IntUnion
{
    int num;
    char data[4];
};
TypeCastHelper::TypeCastHelper()
{
}
TypeCastHelper::~TypeCastHelper()
{
}
int TypeCastHelper::CastInt(char* buffer)
{
    if (buffer == nullptr) {
        return 0;
    }
    IntUnion data;
    memcpy(data.data, buffer, sizeof(IntUnion));
    return data.num;
}

void TypeCastHelper::Put(char * buffer)
{
    cout << ((int)buffer[0]) << "+" << ((int)buffer[1]) << endl;
}

//char * TypeCastHelper::Get()
//{
//    char* data = new char[2];
//    data[0] = -1;
//    data[1] = 2;
//    return data;
//}
