#pragma once
#include <iostream>
#include <winsock2.h>
#include <process.h>
#include "ThreadSafeQuene.h"
#pragma comment(lib, "WS2_32.lib")
using namespace std;
using namespace ThreadSafe;

//宏定义
#define DATA_HEAD_NUM       (sizeof(ProtoHead))
#define	MAX_NUM_BUF			60				//缓冲区的最大长度
#define	MAX_NUM_DATA		(60 + DATA_HEAD_NUM)	//数据包的最大长度
#define FRAME_TIME           67

typedef unsigned short UShort;

//数据包头结构，该结构在win32下为4byte
struct ProtoHead
{
    UShort	proto;	//类型
    UShort	Length;	//数据包的长度(包括头的长度)
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


class Client {
private:
    //变量
    SOCKET	sClient;							//套接字
//    HANDLE	hThreadSend;						//发送数据线程
    HANDLE	hThreadProcess;						//接收数据线程
 //   char    bufSend[MAX_NUM_BUF];				//发送数据缓冲区
 //   BOOL    bSend = FALSE;                      //发送标记位
    BOOL	bConnecting;						//与服务器的连接状态
    CRITICAL_SECTION cs;					//临界区对象，锁定bufSend

    char* serverIp = NULL;
    int serverPort = 0;
    ThreadSafe_Queue<DataBuffer*> m_safeRecvQueue;
    ThreadSafe_Queue<DataBuffer*> m_safeSendQueue;
public:
    //函数声明
    BOOL InitClient(const char* ip, int port);              //初始化
    BOOL ConnectServer(void);           //连接服务器
    void ExitClient(void);              //退出服务器

    void SendData(int proto, const char* content, int contentLength);

    DataBuffer* PopNextPackageData();
    bool IsDataEmpty();
    bool IsConnected();
    /*
    void InputAndOutput(void);*/

    ~Client();
    

    

private:
    int SetServerInfo(const char* ip, int port);
    void InitMember(void);              //初始化全局变量
    BOOL InitSockt(void);               //非阻塞套接字
    BOOL CreateProcessThread(void);
    static DWORD __stdcall ProcessThread(void* pParam);
    //static DWORD __stdcall SendDataThread(void* pParam);
    static bool RunRecv(Client* client);
    static bool RunSend(Client* client);

};








