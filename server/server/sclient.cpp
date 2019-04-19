#include <process.h>
#include <stdio.h>
#include "sclient.h"
#include "server.h"
#include "WebSocket/websocket_handler.h"
#include "WebSocket/sockhead.h"
#include <exception>

//extern BOOL bSend;
//extern char	dataBuf[MAX_NUM_BUF];
/*
 * 构造函数
 */
CClient::CClient(const SOCKET sClient, const sockaddr_in &addrClient)
{
	//初始化变量
	m_hThreadRecv = NULL;
	m_hThreadSend = NULL;
	m_socket = sClient;
	m_addr = addrClient;
	m_bConning = FALSE;
	m_bExit = FALSE;
	m_bSend = FALSE;
    m_webSocketHandler = NULL;
    m_socketType = SocketConnType::Unknow;
	memset(m_data.buf, 0, MAX_NUM_BUF);

	//创建事件
	m_hEvent = CreateEvent(NULL, FALSE, FALSE, NULL);//手动设置信号状态，初始化为无信号状态

	//初始化临界区
	InitializeCriticalSection(&m_cs);
}
/*
 * 析构函数
 */
CClient::~CClient()
{
	closesocket(m_socket);			//关闭套接字
	m_socket = INVALID_SOCKET;		//套接字无效
	DeleteCriticalSection(&m_cs);	//释放临界区对象
	CloseHandle(m_hEvent);			//释放事件对象
    if (m_webSocketHandler != NULL) {
        delete m_webSocketHandler;
    }
}

int CClient::CheckSocketType(CClient* pClient)
{
    char buffer[MAX_NUM_WEB_ALL];
    memset(buffer, 0, MAX_NUM_WEB_ALL);
    int recVal = CClient::RecvDataInner(pClient, buffer);
    if (recVal <= 0) {
        return -1;
    }
    if (strstr(buffer, "GET") || strstr(buffer, "HTTP")) {
        // websocket
        pClient->m_socketType = SocketConnType::Web;
        pClient->m_webSocketHandler = new Websocket_Handler(pClient->Socket());
        memcpy(pClient->m_webSocketHandler->getbuff(), buffer, MAX_NUM_WEB_ALL);
        pClient->m_webSocketHandler->process(buffer);
        LogManager::Debug("Web 端连接");
    }
    else if(strstr(buffer, "PCUnity")){
        pClient->m_socketType = SocketConnType::Normal;
        LogManager::Debug("PC 端连接");
    }
    return 1;
}

DataBuffer * CClient::PopNextData()
{
    DataBuffer* buffer = NULL;
    this->m_RecvBufferQuene.try_pop(buffer);
    return buffer;
}

bool CClient::isEmpty()
{
    return this->m_RecvBufferQuene.empty();
}

/*
 * 创建发送和接收数据线程
 */
BOOL CClient::StartRuning(void)
{
	m_bConning = TRUE;//设置连接状态

	//创建接收数据线程
	unsigned long ulThreadId;
	m_hThreadRecv = CreateThread(NULL, 0, RecvDataThread, this, 0, &ulThreadId);
	if(NULL == m_hThreadRecv)
	{
		return FALSE;
	}else{
		CloseHandle(m_hThreadRecv);
	}

	//创建发送客户端数据的线程
	m_hThreadSend =  CreateThread(NULL, 0, FrameSendDataThread, this, 0, &ulThreadId);
	if(NULL == m_hThreadSend)
	{
		return FALSE;
	}else{
		CloseHandle(m_hThreadSend);
	}

	return TRUE;
}

void CClient::SetFrameSendInner(DataBuffer * buffer)
{
    this->m_sendBufferQuene.push(buffer);
    m_bSend = TRUE;
}

void CClient::SerFrameSend(DataBuffer buffer)
{
    DataBuffer* _buffer = new DataBuffer(buffer);
    this->SetFrameSendInner(_buffer);
}

void CClient::SetFrameSend(UShort proto, const char * buffer, UShort bufferlen)
{
    // TODO 这里会有大量GC
    DataBuffer* datapack = new DataBuffer();
    datapack->Package.head.proto = proto;
    datapack->Package.head.Length = bufferlen + 4; // 加上头部4个字节
    memcpy(datapack->Package.datas, buffer, bufferlen);
    this->SetFrameSendInner(datapack);

    
}

bool CClient::InnerSendData(DataBuffer * buffer)
{
    bool flag = true;
    //进入临界区
    EnterCriticalSection(&this->m_cs);
    //发送数据
    int val = send(this->m_socket, buffer->buffer, buffer->Package.head.Length, 0);
    //处理返回错误
    if (SOCKET_ERROR == val)
    {
        int nErrCode = WSAGetLastError();
        if (nErrCode == WSAEWOULDBLOCK)//发送数据缓冲区不可用
        {
            LogManager::Error("发送数据缓冲区不可用");
        }
        else if (WSAENETDOWN == nErrCode ||
            WSAETIMEDOUT == nErrCode ||
            WSAECONNRESET == nErrCode)//客户端关闭了连接
        {
            this->m_bConning = FALSE;	//连接断开
            this->m_bSend = FALSE;
        }
        else {
            this->m_bConning = FALSE;	//连接断开
            this->m_bSend = FALSE;
        }
        flag = false;
    }
    //离开临界区
    LeaveCriticalSection(&this->m_cs);
    return flag;
}

/*
 * 接收客户端数据
 */
DWORD  CClient::RecvDataThread(void* pParam)
{
	CClient *pClient = (CClient*)pParam;	//客户端对象指针
    if (CClient::CheckSocketType(pClient) > 0) {
        CClient::RecvDataNormal(pClient);
    }
	pClient->m_bConning = FALSE;			//与客户端的连接断开
	return 0;								//线程退出
}

/*
 * @des: 向客户端发送数据
 */
DWORD CClient::FrameSendDataThread(void* pParam)
{
	CClient *pClient = (CClient*)pParam;//转换数据类型为CClient指针
	while(pClient->m_bConning)//连接状态
	{
        Sleep(FRAME_TIME);
        if(pClient->m_bSend || bSend || !pClient->m_sendBufferQuene.empty())
        {
            if (!pClient->InnerSendData(pClient->m_sendBufferQuene.wait_and_pop())) {
                break;
            }
			//设置事件为无信号状态
			pClient->m_bSend = FALSE;
			bSend = FALSE;
		}

	}

	return 0;
}

void CClient::RecvDataNormal(CClient * pClient)
{
    int		reVal;							//返回值
    char	tempBuffer[MAX_NUM_WEB_ALL];				//临时变量

    while (pClient->m_bConning)				//连接状态
    {
        memset(tempBuffer, 0, MAX_NUM_WEB_ALL);
        reVal = CClient::RecvDataInner(pClient, tempBuffer);
        if (reVal <= 0) {
            break;
        }
        //收到数据
        else if (reVal > 0)
        {
            EnterCriticalSection(&pClient->m_cs);
            // TODO GC问题
            DataBuffer* databuffer = new DataBuffer(tempBuffer);
            pClient->m_RecvBufferQuene.push(databuffer);
            LeaveCriticalSection(&pClient->m_cs);

            memset(tempBuffer, 0, MAX_NUM_WEB_ALL);	//清空临时变量
        }
    }
}

int CClient::RecvDataInner(CClient * pClient, char* buffer)
{
    if (pClient == NULL || buffer == NULL) {
        LogManager::Error("pClient is null or buffer is null");
        return -2;
    }
    int	reVal;							//返回值

    if (pClient->m_bConning)				//连接状态
    {
        memset(buffer, 0, MAX_NUM_WEB_ALL);
        reVal = recv(pClient->m_socket, buffer, MAX_NUM_WEB_ALL, 0);	//接收数据
        
        if (reVal <= 0) {
            return -1;
        }
        //收到数据
        else if (reVal > 0)
        {
            if (pClient->WebSocketType() == SocketConnType::Web && pClient->m_webSocketHandler != NULL) {
                // 解析web数据
                memcpy(pClient->m_webSocketHandler->getbuff(), buffer, MAX_NUM_WEB_ALL);
                pClient->m_webSocketHandler->process(buffer);
                //pClient->m_webSocketHandler->GetParseData(buffer);
            }
            return 1;
        }
    }
    return 1;
}

