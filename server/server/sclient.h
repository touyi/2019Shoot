#ifndef SCLIENT_H_INCLUDED
#define SCLIENT_H_INCLUDED
#include <winsock2.h>
#include "Log.h"
#include "ThreadSafeQuene.h"
#include <iostream>
#include<sstream>
#include "WebSocket/websocket_handler.h"
using namespace ThreadSafe;

#include "define.h"



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
            << this->m_addr.sin_port
            << " SocketType:"
            << this->CheckSocketType;
        return ss.str();
    }
    
public:
    BOOL	 StartRuning(void);					//创建发送和接收数据线程
    //void	 HandleData(const char* pExpr);		//计算表达式
    BOOL IsConning(void) {					//是否连接存在
        return m_bConning;
    }

    BOOL IsCanClean() {
        // NEXT TODO
    }
    void DisConning(void) {					//断开与客户端的连接
        m_bConning = FALSE;
    }
    BOOL IsExit(void) {						//接收和发送线程是否已经退出
        return m_bExit;
    }
    SOCKET Socket() {
        return m_socket;
    }
    SocketConnType WebSocketType() {
        return m_socketType;
    }
    void SetFrameSend(UShort proto, const char* buffer, UShort bufferlen);
    void SetFrameSend(const DataBuffer& buffer);

private:
    bool InnerSendData(DataBuffer* buffer);
    void SetFrameSendInner(DataBuffer* buffer);
    
public:
    static DWORD __stdcall	 RecvDataThread(void* pParam);		//接收客户端数据
    static DWORD __stdcall	 FrameSendDataThread(void* pParam);		//向客户端发送数据
    static void	RecvDataNormal(CClient* pClient);		//接收客户端数据
    static int	RecvDataInner(CClient* pClient, char* buffer);		//向客户端发送数据
    static int CheckSocketType(CClient* pClient);

    DataBuffer* PopNextData();
    bool isEmpty();

private:
    CClient();
private:
    ThreadSafe_Queue<DataBuffer*> m_sendBufferQuene; // 数据发送队列
    ThreadSafe_Queue<DataBuffer*> m_RecvBufferQuene; // 数据接收队列

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
    Websocket_Handler* m_webSocketHandler;
    SocketConnType m_socketType;
};


#endif // SCLIENT_H_INCLUDED
