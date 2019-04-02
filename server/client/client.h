#pragma once
#include <iostream>
#include <winsock2.h>
#include <process.h>

#pragma comment(lib, "WS2_32.lib")
using namespace std;

//�궨��
#define	SERVERIP			"127.0.0.1"		//������IP
#define	SERVERPORT			6666			//������TCP�˿�
#define	MAX_NUM_BUF			60				//����������󳤶�

class Client {
private:
    //����
    SOCKET	sClient;							//�׽���
    HANDLE	hThreadSend;						//���������߳�
    HANDLE	hThreadRecv;						//���������߳�
    char    bufSend[MAX_NUM_BUF];				//�������ݻ�����
    BOOL    bSend = FALSE;                      //���ͱ��λ
    BOOL	bConnecting;						//�������������״̬
    HANDLE	arrThread[2];						//���߳�����
    CRITICAL_SECTION cs;					//�ٽ�����������bufSend
public:
    //��������
    BOOL InitClient(void);              //��ʼ��
    void InitMember(void);              //��ʼ��ȫ�ֱ���
    BOOL InitSockt(void);               //�������׽���
    BOOL ConnectServer(void);           //���ӷ�����
    //bool RecvLine(SOCKET s, char* buf); //��ȡһ������
    //bool sendData(SOCKET s, char* str); //��������
    //void recvAndSend(void);             //���ݴ�����
    //bool recvData(SOCKET s, char* buf);
    void ShowConnectMsg(BOOL bSuc);     //���Ӵ�ӡ����
    void ExitClient(void);              //�˳�������
    BOOL CreateSendAndRecvThread(void);
    void InputAndOutput(void);

    friend DWORD __stdcall RecvDataThread(void* pParam);
    friend DWORD __stdcall SendDataThread(void* pParam);

};

DWORD __stdcall RecvDataThread(void* pParam);
DWORD __stdcall SendDataThread(void* pParam);






