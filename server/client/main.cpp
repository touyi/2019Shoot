#include <iostream>
#include "client.h"

using namespace std;

int main()
{
    Client client;
    	//初始化
	if (!client.InitClient())
	{
        client.ExitClient();
	}

	//连接服务器
	if(client.ConnectServer())
	{
        client.ShowConnectMsg(TRUE);
	}
	else
    {
        client.ShowConnectMsg(FALSE);
        client.ExitClient();
	}

	//创建发送和接收数据线程
	if (!client.CreateSendAndRecvThread())
	{
        client.ExitClient();

	}

	//用户输入数据和显示结果
    client.InputAndOutput();
//
//	//退出
//	ExitClient();
//
	return 0;
}
