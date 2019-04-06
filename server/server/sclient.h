#ifndef SCLIENT_H_INCLUDED
#define SCLIENT_H_INCLUDED
#include <winsock2.h>
#include "Log.h"
#include "ThreadSafeQuene.h"
#include <iostream>
#include<sstream>
using namespace ThreadSafe;


#define TIMEFOR_THREAD_CLIENT		500		//线程睡眠时间

#define	MAX_NUM_CLIENT		10				//接受的客户端连接最多数量
#define	MAX_NUM_BUF			60				//缓冲区的最大长度
#define	MAX_NUM_DATA		(60 + sizeof(ProtoHead))	//数据包的最大长度
#define INVALID_OPERATOR	1				//无效的操作符
#define INVALID_NUM			2				//分母为零
#define ZERO				0				//零
#define FRAME_TIME           67

typedef int ClientID;
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
};

//数据包中的数据结构
typedef struct _data
{
    char	buf[MAX_NUM_BUF];//数据
}DATABUF, *pDataBuf;


class CClient : public IClassInfo
{
public:
    CClient(const SOCKET sClient, const sockaddr_in &addrClient);
    virtual ~CClient();

    operator string() override {
        std::stringstream ss;
        ss << "IP:"
            << string(inet_ntoa(this->m_addr.sin_addr))
            << " Port:"
            << this->m_addr.sin_port;
        return ss.str();
    }
public:
    BOOL	 StartRuning(void);					//创建发送和接收数据线程
    //void	 HandleData(const char* pExpr);		//计算表达式
    BOOL IsConning(void) {					//是否连接存在
        return m_bConning;
    }
    void DisConning(void) {					//断开与客户端的连接
        m_bConning = FALSE;
    }
    BOOL IsExit(void) {						//接收和发送线程是否已经退出
        return m_bExit;
    }
    void SetFrameSend(UShort proto, char* buffer, UShort bufferlen);

private:
    bool InnerSendData(DataBuffer* buffer);
public:
    static DWORD __stdcall	 RecvDataThread(void* pParam);		//接收客户端数据
    static DWORD __stdcall	 FrameSendDataThread(void* pParam);		//向客户端发送数据

private:
    CClient();
private:
    /*struct DataPack {
    public:
        INT32 proto;
        char* buffer;
        int bufferLen;

        DataPack(const DataPack& src) {
            *this = src;
        }
        DataPack& operator=(const DataPack& rhs) {
            this->proto = rhs.proto;
            this->buffer = new char[rhs.bufferLen];
            memcpy(this->buffer, rhs.buffer, rhs.bufferLen);
            this->bufferLen = rhs.bufferLen;
        }

        DataPack(): proto(0),  buffer(NULL), bufferLen(0) {}
        ~DataPack() {
            if (buffer != NULL) {
                delete[] buffer;
            }
        }
    };*/
    ThreadSafe_Queue<DataBuffer*> m_sendBufferQuene; // 数据发送队列

    SOCKET		m_socket;			//套接字
    sockaddr_in	m_addr;				//地址
    DATABUF		m_data;				//数据
    HANDLE		m_hEvent;			//事件对象
    HANDLE		m_hThreadSend;		//发送数据线程句柄
    HANDLE		m_hThreadRecv;		//接收数据线程句柄
    CRITICAL_SECTION m_cs;			//临界区对象
    BOOL		m_bConning;			//客户端连接状态
    BOOL        m_bSend;            //数据发送状态
    BOOL		m_bExit;			//线程退出
    // char m_buffer[MAX_NUM_BUF];
};


#endif // SCLIENT_H_INCLUDED
