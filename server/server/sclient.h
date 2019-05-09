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
    BOOL	 StartRuning(void);					//�������ͺͽ��������߳�
    //void	 HandleData(const char* pExpr);		//������ʽ
    BOOL IsConning(void) {					//�Ƿ����Ӵ���
        return m_bConning;
    }

    BOOL IsCanClean() {
        // NEXT TODO
    }
    void DisConning(void) {					//�Ͽ���ͻ��˵�����
        m_bConning = FALSE;
    }
    BOOL IsExit(void) {						//���պͷ����߳��Ƿ��Ѿ��˳�
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
    static DWORD __stdcall	 RecvDataThread(void* pParam);		//���տͻ�������
    static DWORD __stdcall	 FrameSendDataThread(void* pParam);		//��ͻ��˷�������
    static void	RecvDataNormal(CClient* pClient);		//���տͻ�������
    static int	RecvDataInner(CClient* pClient, char* buffer);		//��ͻ��˷�������
    static int CheckSocketType(CClient* pClient);

    DataBuffer* PopNextData();
    bool isEmpty();

private:
    CClient();
private:
    ThreadSafe_Queue<DataBuffer*> m_sendBufferQuene; // ���ݷ��Ͷ���
    ThreadSafe_Queue<DataBuffer*> m_RecvBufferQuene; // ���ݽ��ն���

    SOCKET		m_socket;			//�׽���
    sockaddr_in	m_addr;				//��ַ
    DATABUF		m_data;				//����
    HANDLE		m_hEvent;			//�¼�����
    HANDLE		m_hThreadSend;		//���������߳̾��
    HANDLE		m_hThreadRecv;		//���������߳̾��
    CRITICAL_SECTION m_cs;			//�ٽ�������
    BOOL		m_bConning;			//�ͻ�������״̬
    BOOL        m_bSend;            //���ݷ���״̬
    BOOL		m_bExit;			//�߳��˳�
    Websocket_Handler* m_webSocketHandler;
    SocketConnType m_socketType;
};


#endif // SCLIENT_H_INCLUDED
