#include <iostream>
#include "client.h"

using namespace std;

int main()
{
    Client client;
    	//��ʼ��
	if (!client.InitClient())
	{
        client.ExitClient();
	}

	//���ӷ�����
	if(client.ConnectServer())
	{
        client.ShowConnectMsg(TRUE);
	}
	else
    {
        client.ShowConnectMsg(FALSE);
        client.ExitClient();
	}

	//�������ͺͽ��������߳�
	if (!client.CreateSendAndRecvThread())
	{
        client.ExitClient();

	}

	//�û��������ݺ���ʾ���
    client.InputAndOutput();
//
//	//�˳�
//	ExitClient();
//
	return 0;
}
