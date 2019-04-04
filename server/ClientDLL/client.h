#pragma once
#include <iostream>
#include <winsock2.h>
#include <process.h>

#pragma comment(lib, "WS2_32.lib")
using namespace std;

//�궨��
//#define	SERVERIP			"127.0.0.1"		//������IP
//#define	SERVERPORT			6666			//������TCP�˿�
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

    char* serverIp = NULL;
    int serverPort = 0;
public:
    //��������
    BOOL InitClient(const char* ip, int port);              //��ʼ��
    BOOL ConnectServer(void);           //���ӷ�����
    void ExitClient(void);              //�˳�������

    void SendData(int proto, char* content);
    
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








