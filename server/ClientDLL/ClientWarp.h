#pragma once
#include "client.h"
#include "DataItem.h"
class ClientWarp
{
private:
    Client client;
public:
    ClientWarp();
    ~ClientWarp();
    int InitClient(const char* ip, int port);  //初始化
    int ConnectServer(void);           //连接服务器
    void ExitClient(void);              //退出服务器
    void SendData(int proto, void* content, int contentLength); // 发送数据
    bool IsDataEmpty();
    bool IsConnected();
    DataItem PopNextData();
};

