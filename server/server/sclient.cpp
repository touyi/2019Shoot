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
}

void CClient::CheckSocketType(CClient* pClient)
{
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


void CClient::SetFrameSend(UShort proto, const char * buffer, UShort bufferlen)
{
    // TODO 这里会有大量GC
    DataBuffer* datapack = new DataBuffer();
    datapack->Package.head.proto = proto;
    datapack->Package.head.Length = bufferlen + 4; // 加上头部4个字节
    memcpy(datapack->Package.datas, buffer, bufferlen);
    this->m_sendBufferQuene.push(datapack);

    m_bSend = TRUE;
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
    // Next TODO 检查socket连接属性 websocket normal？
    // TODO
    /*if (pClient->IsWebSocket()) {
        CClient::RecvDataWeb(pClient);
    }
    else {
        CClient::RecvDataNormal(pClient);
    }*/
	
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
    char	tempBuffer[MAX_NUM_DATA];				//临时变量

    while (pClient->m_bConning)				//连接状态
    {
        memset(tempBuffer, 0, MAX_NUM_DATA);
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

            memset(tempBuffer, 0, MAX_NUM_BUF);	//清空临时变量
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
        memset(buffer, 0, MAX_NUM_BUF);
        reVal = recv(pClient->m_socket, buffer, MAX_NUM_BUF, 0);	//接收数据
        if (reVal <= 0) {
            return -1;
        }
        //收到数据
        else if (reVal > 0)
        {
            return 1;
        }
    }
    return 1;
    //Websocket_Handler handler(pClient->Socket());
    //while (pClient->m_bConning) {
    //    // NEXT TODO
    //    int bufflen = 0;
    //    if ((bufflen = recv(pClient->m_socket, handler.getbuff(), /*TODO Temp value*/2048, 0)) <= 0) {
    //        // 连接断开
    //        break;
    //    }
    //    handler.process();
    //}
}

