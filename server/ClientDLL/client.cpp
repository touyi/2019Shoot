#include "client.h"
#include "Tools.h"

/**
 *	初始化
 */
BOOL Client::InitClient(const char* ip, int port)
{
    SetServerInfo(ip, port);
	//初始化全局变量
	InitMember();

	//创建SOCKET
	if (!InitSockt())
	{
		return FALSE;
	}

	return TRUE;
}
/**
 * 初始化全局变量
 */
void Client::InitMember(void)
{
	InitializeCriticalSection(&cs);

	sClient = INVALID_SOCKET;	//套接字
    hThreadProcess = NULL;			//接收数据线程句柄
	hThreadSend = NULL;			//发送数据线程句柄
	bConnecting = FALSE;		//为连接状态

    //初始化数据缓冲区
	memset(bufSend, 0, MAX_NUM_BUF);
}

/**
 * 创建非阻塞套接字
 */
BOOL Client::InitSockt(void)
{
	int			reVal;	//返回值
	WSADATA		wsData;	//WSADATA变量
	reVal = WSAStartup(MAKEWORD(2,2),&wsData);//初始化Windows Sockets Dll

	//创建套接字
	sClient = socket(AF_INET, SOCK_STREAM, 0);
	if(INVALID_SOCKET == sClient)
		return FALSE;


	//设置套接字非阻塞模式
	unsigned long ul = 1;
	reVal = ioctlsocket(sClient, FIONBIO, (unsigned long*)&ul);
	if (reVal == SOCKET_ERROR)
		return FALSE;

	return TRUE;
}

/**
 * 连接服务器
 */
BOOL Client::ConnectServer(void)
{
	int reVal;			//返回值
	sockaddr_in serAddr;//服务器地址
	//输入要连接的主机地址
    serAddr.sin_family = AF_INET;
    serAddr.sin_port = htons(this->serverPort);
    serAddr.sin_addr.S_un.S_addr = inet_addr(this->serverIp);

	while(true)
	{
		//连接服务器
		reVal = connect(sClient, (struct sockaddr*)&serAddr, sizeof(serAddr));
		//处理连接错误
		if(SOCKET_ERROR == reVal)
		{
			int nErrCode = WSAGetLastError();
			if( WSAEWOULDBLOCK == nErrCode || WSAEINVAL == nErrCode)    //连接还没有完成
			{
				continue;
			}
			else if (WSAEISCONN == nErrCode)//连接已经完成
			{
				break;
			}
			else//其它原因，连接失败
			{
				return FALSE;
			}
		}
		if ( reVal == 0 )//连接成功
			break;
	}

    if (!this->CreateProcessThread()) {
        this->ExitClient();
        return FALSE;
    }

	bConnecting = TRUE;

    // 发送握手消息
    this->SendData(0, "PCUnity");
	return TRUE;
}
/**
 * 创建发送和接收数据线程
 */
BOOL Client::CreateProcessThread(void)
{
	//创建接收数据的线程
	unsigned long ulThreadId;
	hThreadProcess = CreateThread(NULL, 0, ProcessThread, this, 0, &ulThreadId);
	if (NULL == hThreadProcess)
		return FALSE;

	//创建发送数据的线程
	hThreadSend = CreateThread(NULL, 0, SendDataThread, this, 0, &ulThreadId);
	if (NULL == hThreadSend)
		return FALSE;

	return TRUE;
}
/**
 * 接收数据线程
 */
DWORD __stdcall	Client::ProcessThread(void* pParam)
{
    Client* client = static_cast<Client*>(pParam);
    if (client == NULL) {
        return 0;
    }
	//int		reVal;				    //返回值
	//char    bufRecv[MAX_NUM_DATA];   //接收数据缓冲区

	while(client->bConnecting)			    //连接状态
	{
        Sleep(FRAME_TIME);
        if (!Client::RunRecv(client)) {
            return 0;
        }
        if (!Client::RunSend(client)) {
            return 0;
        }
        // TODO NEXT




  //      memset(bufRecv, 0, MAX_NUM_DATA);
		//reVal = recv(client->sClient, bufRecv, MAX_NUM_DATA, 0);//接收数据
		//if (SOCKET_ERROR == reVal)
		//{
		//	int nErrCode = WSAGetLastError();
		//	if (WSAEWOULDBLOCK == nErrCode)			//接受数据缓冲区不可用
		//	{
		//		continue;							//继续接收数据
		//	}else{
  //              client->bConnecting = FALSE;
		//		return 0;							//线程退出
		//	}
		//}

		//if ( reVal == 0)							//服务器关闭了连接
		//{
  //          client->bConnecting = FALSE;
  //          client->bSend = FALSE;
  //          memset(bufRecv, 0, MAX_NUM_BUF);		//清空接收缓冲区
  //          client->ExitClient();
		//	return 0;								//线程退出
		//}
		//if(reVal > 0)
  //      {
  //          DataBuffer* data = new DataBuffer();
  //          memset(data, 0, sizeof(DataBuffer));
  //          memcpy(data->buffer, bufRecv, MAX_NUM_DATA);
  //          client->m_safeQueue.push(data);
  //      }
	}
	return 0;
}
/**
 * 发送数据线程
 */
DWORD __stdcall	Client::SendDataThread(void* pParam)
{
    Client* client = static_cast<Client*>(pParam);
    if (client == NULL) {
        return 0;
    }
	while(client->bConnecting)						//连接状态
	{
		if (client->bSend)						//发送数据
		{
            EnterCriticalSection(&client->cs);	//进入临界区
			while(TRUE)
            {
                int val = send(client->sClient, client->bufSend, MAX_NUM_BUF,0);

                //处理返回错误
                if (SOCKET_ERROR == val)
                {
                    int nErrCode = WSAGetLastError();
                    if(WSAEWOULDBLOCK == nErrCode)		//发送缓冲区不可用
                    {
                        continue;						//继续循环
                    }else
                    {
                        LeaveCriticalSection(&client->cs);	//离开临界区
                        return 0;
                    }
                }

                client->bSend = FALSE;			//发送状态
                    break;					//跳出for
            }
            LeaveCriticalSection(&client->cs);	//离开临界区
		}
    }
	return 0;
}
bool Client::RunRecv(Client * client)
{
    return false;
}
bool Client::RunSend(Client * client)
{
    return false;
}
/**
 * 输入数据和显示结果
 */
void Client::InputAndOutput(void)
{
    char cInput[MAX_NUM_BUF];	//用户输入缓冲区
    while(bConnecting)			//连接状态
	{
		memset(cInput, 0, MAX_NUM_BUF);
		cin >> cInput;			        //输入表达式
        EnterCriticalSection(&cs);		//进入临界区
        memset(bufSend, 0, sizeof(bufSend));
		memcpy(bufSend, cInput, strlen(cInput));
		LeaveCriticalSection(&cs);		//离开临界区
		bSend = TRUE;
	}
}

Client::~Client()
{
    this->ExitClient();
}

int Client::SetServerInfo(const char * ip, int port)
{
    {
        if (ip == NULL) {

            return -1;
        }
        int len = strlen(ip);
        if (serverIp != NULL) {
            delete serverIp;
            serverIp = NULL;
        }
        serverIp = new char[len + 1]{ 0 };
        strcpy(serverIp, ip);
        serverPort = port ;
        return 1;
    }
}

/**
 * 客户端退出
 */
void Client::ExitClient(void)
{
	DeleteCriticalSection(&cs);
    CloseHandle(hThreadProcess);
	CloseHandle(hThreadSend);
    memset(bufSend, 0, MAX_NUM_BUF);
	closesocket(sClient);
	WSACleanup();
}

void Client::SendData(int proto, const char * content)
{
    
    if (bConnecting)			//连接状态
    {
        char cInput[MAX_NUM_BUF];	//用户输入缓冲区
        Int32ToChar(cInput, proto);
        memcpy(cInput + 4, content, strlen(content));
        EnterCriticalSection(&cs);		//进入临界区
        memset(bufSend, 0, sizeof(bufSend));
        memcpy(bufSend, cInput, strlen(cInput));
        // Temp TODO
        memcpy(bufSend, "PCUnity", strlen("PCUnity")); // Temp
        LeaveCriticalSection(&cs);		//离开临界区
        bSend = TRUE;
    }
}

DataBuffer * Client::PopNextPackageData()
{
    DataBuffer* data = NULL;
    if (!m_safeQueue.try_pop(data)) {
        data = NULL;
    }
    return data;
}

bool Client::IsDataEmpty()
{
    return m_safeQueue.empty();
}

bool Client::IsConnected()
{
    return this->bConnecting;
}


