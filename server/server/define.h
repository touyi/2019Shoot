#pragma once
#define TIMEFOR_THREAD_CLIENT		500		//线程睡眠时间

#define	MAX_NUM_CLIENT		10				//接受的客户端连接最多数量
#define	MAX_NUM_BUF			60				//缓冲区的最大长度
#define HEAD_SIZE           sizeof(ProtoHead)
#define	MAX_NUM_DATA		(MAX_NUM_BUF + HEAD_SIZE)	//数据包的最大长度
#define MAX_NUM_WEB_HEAD    1024
#define MAX_NUM_WEB_ALL    (MAX_NUM_WEB_HEAD + MAX_NUM_DATA)
#define INVALID_OPERATOR	1				//无效的操作符
#define INVALID_NUM			2				//分母为零
#define ZERO				0				//零
#define FRAME_TIME           67
const int MobileCMDProtocol = 100;

typedef int ClientID;
typedef unsigned short UShort;

#include<memory.h>

//数据包头结构，该结构在win32下为4byte
struct ProtoHead
{
    UShort	proto;	//类型
    UShort	Length;	//数据包的长度(包括头的长度)
};

enum SocketConnType {
    Unknow,
    Normal,
    Web,
};

union DataBuffer {
    struct DataPackage {
        ProtoHead head;
        char datas[MAX_NUM_BUF];
    } Package;
    char buffer[MAX_NUM_DATA];
public:
    DataBuffer() = default;
    DataBuffer(char* _buffer) {
        memcpy(this->buffer, _buffer, MAX_NUM_DATA);
    }
    DataBuffer(const DataBuffer& buffer) {
        (*this) = buffer;
    }
    DataBuffer(const DataBuffer*&buffer) {
        (*this) = *buffer;
    }
    const DataBuffer& operator=(const DataBuffer& _buffer) {
        memcpy(this->buffer, _buffer.buffer, MAX_NUM_DATA);
        return (*this);
    }
};

//数据包中的数据结构
typedef struct _data
{
    char	buf[MAX_NUM_BUF];//数据
}DATABUF, *pDataBuf;