#pragma once
#include <iostream>
#include <winsock2.h>
#include <process.h>
#include "ThreadSafeQuene.h"
#pragma comment(lib, "WS2_32.lib")
using namespace std;
using namespace ThreadSafe;

//�궨��
#define	MAX_NUM_BUF			60				//����������󳤶�
#define	MAX_NUM_DATA		(60 + sizeof(ProtoHead))	//���ݰ�����󳤶�
#define FRAME_TIME           67
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


class Client {
private:
    //����
    SOCKET	sClient;							//�׽���
    HANDLE	hThreadSend;						//���������߳�
    HANDLE	hThreadRecv;						//���������߳�
    char    bufSend[MAX_NUM_BUF];				//�������ݻ�����
    BOOL    bSend = FALSE;                      //���ͱ��λ
    BOOL	bConnecting;						//�������������״̬
    CRITICAL_SECTION cs;					//�ٽ�����������bufSend

    char* serverIp = NULL;
    int serverPort = 0;
    ThreadSafe_Queue<DataBuffer*> m_safeQueue;
public:
    //��������
    BOOL InitClient(const char* ip, int port);              //��ʼ��
    BOOL ConnectServer(void);           //���ӷ�����
    void ExitClient(void);              //�˳�������

    void SendData(int proto, char* content);

    DataBuffer* PopNextPackageData();
    bool IsDataEmpty();
    
    void InputAndOutput(void);

    ~Client();
    

    

private:
    int SetServerInfo(const char* ip, int port);
    void InitMember(void);              //��ʼ��ȫ�ֱ���
    BOOL InitSockt(void);               //�������׽���
    BOOL CreateSendAndRecvThread(void);
    static DWORD __stdcall RecvDataThread(void* pParam);
    static DWORD __stdcall SendDataThread(void* pParam);

};








