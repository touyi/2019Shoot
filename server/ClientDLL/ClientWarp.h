#pragma once
#include "client.h"
class ClientWarp
{
private:
    Client client;
public:
    ClientWarp();
    ~ClientWarp();
    int InitClient(const char* ip, int port);  //��ʼ��
    int ConnectServer(void);           //���ӷ�����
    void ExitClient(void);              //�˳�������
    void SendData(int proto, char* content); // ��������
};
