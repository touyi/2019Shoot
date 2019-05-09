#ifndef SERVER_H_INCLUDED
#define SERVER_H_INCLUDED
#include <iostream>
#include <winsock2.h>
#include <winbase.h>
#include <vector>
#include <map>
#include <string>
#include <process.h>
#include "sclient.h"
#include "protocol/Protocol.pb.h"

#pragma comment(lib, "ws2_32.lib")			//动态库函数

/**
 * 宏定义
 */
#define SERVER_SETUP_FAIL			1		//启动客户端失败
#define START_SERVER                1       //显示开始输入提示
#define INPUT_DATA                  2       //提示输入什么数据

#define SERVERPORT			7000			//服务器TCP端口
#define CONN_NUM            10              //连接客户端数量
#define TIMEFOR_THREAD_SLEEP		500		//等待客户端请求线程睡眠时间
#define TIMEFOR_THREAD_HELP			1500	//清理资源线程退出时间
#define TIMEFOR_THREAD_EXIT			5000	//主线程睡眠时间
#define WRITE_ALL   "all"                   //向所有数据发送数据
#define WRITE       "write"                 //发送标志
#define READ        "read"                  //接收显示标志
#define READ_ALL    "read all"              //接收所有客户端数据
// TODO 这个vector不是线程安全的 配对删除调用可能会有问题
typedef std::vector<CClient*> ClIENTVECTOR;	//向量容器
typedef std::vector<string> SVECTOR;        //内容字符





/**
 * 全局变量
 */
extern BOOL	bRunning;							//与客户端的连接状态
extern BOOL    bSend;                              //发送标记位
extern BOOL    clientConn;                         //连接客户端标记
extern SOCKET	sServer;							//服务器监听套接字
extern CRITICAL_SECTION  cs;			            //保护数据的临界区对象
extern HANDLE	hAcceptThread;						//数据处理线程句柄
extern HANDLE	hCleanThread;						//数据接收线程

/**
 *函数申明
 */
BOOL initSever(void);                       //初始化
void initMember(void);
bool initSocket(void);						//初始化非阻塞套接字
void exitServer(void);						//释放资源
bool startService(void);					//启动服务器
void Run(void);                  //处理数据
bool PartnerStateRight();
void SendCMDToClient(CClient* client, Message::CmdType type);
//void showServerStartMsg(BOOL bSuc);         //显示错误信息
//void showServerExitMsg(void);               //显示退出消息
void handleData(UShort proto, const char* buffer, UShort bufferlen, ClientID id);                 //数据处理
//void showTipMsg(BOOL bFirstInput);          //显示输入提示信息
BOOL createCleanAndAcceptThread(void);      //开启监控函数
DWORD __stdcall acceptThread(void* pParam); //开启客户端请求线程
DWORD __stdcall cleanThread(void* pParam);


#endif // SERVER_H_INCLUDED
