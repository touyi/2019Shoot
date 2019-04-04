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
