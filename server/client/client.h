#pragma once
#include <iostream>
#include <winsock2.h>
#include <process.h>

#pragma comment(lib, "WS2_32.lib")
using namespace std;

//宏定义
#define	SERVERIP			"127.0.0.1"		//服务器IP
#define	SERVERPORT			6666			//服务器TCP端口
#define	MAX_NUM_BUF			60				//缓冲区的最大长度

class Client {
private:
    //变量
    SOCKET	sClient;							//套接字
    HANDLE	hThreadSend;						//发送数据线程
    HANDLE	hThreadRecv;						//接收数据线程
    char    bufSend[MAX_NUM_BUF];				//发送数据缓冲区
    BOOL    bSend = FALSE;                      //发送标记位
    BOOL	bConnecting;						//与服务器的连接状态
    HANDLE	arrThread[2];						//子线程数组
    CRITICAL_SECTION cs;					//临界区对象，锁定bufSend
public:
    //函数声明
    BOOL InitClient(void);              //初始化
    void InitMember(void);              //初始化全局变量
    BOOL InitSockt(void);               //非阻塞套接字
    BOOL ConnectServer(void);           //连接服务器
    //bool RecvLine(SOCKET s, char* buf); //读取一行数据
    //bool sendData(SOCKET s, char* str); //发送数据
    //void recvAndSend(void);             //数据处理函数
    //bool recvData(SOCKET s, char* buf);
    void ShowConnectMsg(BOOL bSuc);     //连接打印函数
    void ExitClient(void);              //退出服务器
    BOOL CreateSendAndRecvThread(void);
    void InputAndOutput(void);

    friend DWORD __stdcall RecvDataThread(void* pParam);
    friend DWORD __stdcall SendDataThread(void* pParam);

};

DWORD __stdcall RecvDataThread(void* pParam);
DWORD __stdcall SendDataThread(void* pParam);






