#include "ClientWarp.h"



ClientWarp::ClientWarp()
{
}


ClientWarp::~ClientWarp()
{
}

int ClientWarp::InitClient(const char * ip, int port)
{
    return this->client.InitClient(ip, port);
}

int ClientWarp::ConnectServer(void)
{
    return this->client.ConnectServer();
}

void ClientWarp::ExitClient(void)
{
    this->client.ExitClient();
}

void ClientWarp::SendData(int proto, char * content)
{
    this->client.SendData(proto, content);
}

bool ClientWarp::IsDataEmpty()
{
    return this->client.IsDataEmpty();
}

bool ClientWarp::IsConnected()
{
    return this->client.IsConnected();
}

DataItem ClientWarp::PopNextData()
{
    // TODO ������Ż�
    DataItem data;
    DataBuffer* buffer = this->client.PopNextPackageData();
    if (buffer != NULL) {
        data.protocol = buffer->Package.head.proto;
        data.bufferLength = buffer->Package.head.Length - DATA_HEAD_NUM;
        memcpy(data.GetBuffer(), buffer->Package.datas, sizeof(buffer->Package.datas));
        delete buffer;
    }
    else {
        data.protocol = 0; // 0��ʾ���� û�нӵ�����
    }
    return data;
}
