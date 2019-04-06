#ifndef SCLIENT_H_INCLUDED
#define SCLIENT_H_INCLUDED
#include <winsock2.h>
#include "Log.h"
#include "ThreadSafeQuene.h"
#include <iostream>
#include<sstream>
using namespace ThreadSafe;


#define TIMEFOR_THREAD_CLIENT		500		//�߳�˯��ʱ��

#define	MAX_NUM_CLIENT		10				//���ܵĿͻ��������������
#define	MAX_NUM_BUF			60				//����������󳤶�
#define	MAX_NUM_DATA		(60 + sizeof(ProtoHead))	//���ݰ�����󳤶�
#define INVALID_OPERATOR	1				//��Ч�Ĳ�����
#define INVALID_NUM			2				//��ĸΪ��
#define ZERO				0				//��
#define FRAME_TIME           67

typedef int ClientID;
typedef unsigned short UShort;

//���ݰ�ͷ�ṹ���ýṹ��win32��Ϊ4byte
struct ProtoHead
{
    UShort	proto;	//����
    UShort	Length;	//���ݰ��ĳ���(����ͷ�ĳ���)
};

union DataBuffer {
    struct DataPackage {
        ProtoHead head;
        char datas[MAX_NUM_BUF];
    } Package;
    char buffer[MAX_NUM_DATA];
};

//���ݰ��е����ݽṹ
typedef struct _data
{
    char	buf[MAX_NUM_BUF];//����
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
    BOOL	 StartRuning(void);					//�������ͺͽ��������߳�
    //void	 HandleData(const char* pExpr);		//������ʽ
    BOOL IsConning(void) {					//�Ƿ����Ӵ���
        return m_bConning;
    }
    void DisConning(void) {					//�Ͽ���ͻ��˵�����
        m_bConning = FALSE;
    }
    BOOL IsExit(void) {						//���պͷ����߳��Ƿ��Ѿ��˳�
        return m_bExit;
    }
    void SetFrameSend(UShort proto, char* buffer, UShort bufferlen);

private:
    bool InnerSendData(DataBuffer* buffer);
public:
    static DWORD __stdcall	 RecvDataThread(void* pParam);		//���տͻ�������
    static DWORD __stdcall	 FrameSendDataThread(void* pParam);		//��ͻ��˷�������

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
    ThreadSafe_Queue<DataBuffer*> m_sendBufferQuene; // ���ݷ��Ͷ���

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
    // char m_buffer[MAX_NUM_BUF];
};


#endif // SCLIENT_H_INCLUDED
