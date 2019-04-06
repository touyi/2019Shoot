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

DataItem ClientWarp::PopNextData()
{
    DataItem data;
    DataBuffer* buffer = this->client.PopNextPackageData();
    if (buffer != NULL) {
        data.protocol = buffer->Package.head.proto;
        memcpy(data.buffer, buffer->Package.datas, sizeof(buffer->Package.datas));
        delete buffer;
    }
    else {
        data.protocol = 0; // 0表示错误 没有接到数据
    }
    return data;
}
