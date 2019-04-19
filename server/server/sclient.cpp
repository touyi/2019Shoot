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
 * ���캯��
 */
CClient::CClient(const SOCKET sClient, const sockaddr_in &addrClient)
{
	//��ʼ������
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

	//�����¼�
	m_hEvent = CreateEvent(NULL, FALSE, FALSE, NULL);//�ֶ������ź�״̬����ʼ��Ϊ���ź�״̬

	//��ʼ���ٽ���
	InitializeCriticalSection(&m_cs);
}
/*
 * ��������
 */
CClient::~CClient()
{
	closesocket(m_socket);			//�ر��׽���
	m_socket = INVALID_SOCKET;		//�׽�����Ч
	DeleteCriticalSection(&m_cs);	//�ͷ��ٽ�������
	CloseHandle(m_hEvent);			//�ͷ��¼�����
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
        LogManager::Debug("Web ������");
    }
    else if(strstr(buffer, "PCUnity")){
        pClient->m_socketType = SocketConnType::Normal;
        LogManager::Debug("PC ������");
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
 * �������ͺͽ��������߳�
 */
BOOL CClient::StartRuning(void)
{
	m_bConning = TRUE;//��������״̬

	//�������������߳�
	unsigned long ulThreadId;
	m_hThreadRecv = CreateThread(NULL, 0, RecvDataThread, this, 0, &ulThreadId);
	if(NULL == m_hThreadRecv)
	{
		return FALSE;
	}else{
		CloseHandle(m_hThreadRecv);
	}

	//�������Ϳͻ������ݵ��߳�
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
    // TODO ������д���GC
    DataBuffer* datapack = new DataBuffer();
    datapack->Package.head.proto = proto;
    datapack->Package.head.Length = bufferlen + 4; // ����ͷ��4���ֽ�
    memcpy(datapack->Package.datas, buffer, bufferlen);
    this->SetFrameSendInner(datapack);

    
}

bool CClient::InnerSendData(DataBuffer * buffer)
{
    bool flag = true;
    //�����ٽ���
    EnterCriticalSection(&this->m_cs);
    //��������
    int val = send(this->m_socket, buffer->buffer, buffer->Package.head.Length, 0);
    //�����ش���
    if (SOCKET_ERROR == val)
    {
        int nErrCode = WSAGetLastError();
        if (nErrCode == WSAEWOULDBLOCK)//�������ݻ�����������
        {
            LogManager::Error("�������ݻ�����������");
        }
        else if (WSAENETDOWN == nErrCode ||
            WSAETIMEDOUT == nErrCode ||
            WSAECONNRESET == nErrCode)//�ͻ��˹ر�������
        {
            this->m_bConning = FALSE;	//���ӶϿ�
            this->m_bSend = FALSE;
        }
        else {
            this->m_bConning = FALSE;	//���ӶϿ�
            this->m_bSend = FALSE;
        }
        flag = false;
    }
    //�뿪�ٽ���
    LeaveCriticalSection(&this->m_cs);
    return flag;
}

/*
 * ���տͻ�������
 */
DWORD  CClient::RecvDataThread(void* pParam)
{
	CClient *pClient = (CClient*)pParam;	//�ͻ��˶���ָ��
    if (CClient::CheckSocketType(pClient) > 0) {
        CClient::RecvDataNormal(pClient);
    }
	pClient->m_bConning = FALSE;			//��ͻ��˵����ӶϿ�
	return 0;								//�߳��˳�
}

/*
 * @des: ��ͻ��˷�������
 */
DWORD CClient::FrameSendDataThread(void* pParam)
{
	CClient *pClient = (CClient*)pParam;//ת����������ΪCClientָ��
	while(pClient->m_bConning)//����״̬
	{
        Sleep(FRAME_TIME);
        if(pClient->m_bSend || bSend || !pClient->m_sendBufferQuene.empty())
        {
            if (!pClient->InnerSendData(pClient->m_sendBufferQuene.wait_and_pop())) {
                break;
            }
			//�����¼�Ϊ���ź�״̬
			pClient->m_bSend = FALSE;
			bSend = FALSE;
		}

	}

	return 0;
}

void CClient::RecvDataNormal(CClient * pClient)
{
    int		reVal;							//����ֵ
    char	tempBuffer[MAX_NUM_WEB_ALL];				//��ʱ����

    while (pClient->m_bConning)				//����״̬
    {
        memset(tempBuffer, 0, MAX_NUM_WEB_ALL);
        reVal = CClient::RecvDataInner(pClient, tempBuffer);
        if (reVal <= 0) {
            break;
        }
        //�յ�����
        else if (reVal > 0)
        {
            EnterCriticalSection(&pClient->m_cs);
            // TODO GC����
            DataBuffer* databuffer = new DataBuffer(tempBuffer);
            pClient->m_RecvBufferQuene.push(databuffer);
            LeaveCriticalSection(&pClient->m_cs);

            memset(tempBuffer, 0, MAX_NUM_WEB_ALL);	//�����ʱ����
        }
    }
}

int CClient::RecvDataInner(CClient * pClient, char* buffer)
{
    if (pClient == NULL || buffer == NULL) {
        LogManager::Error("pClient is null or buffer is null");
        return -2;
    }
    int	reVal;							//����ֵ

    if (pClient->m_bConning)				//����״̬
    {
        memset(buffer, 0, MAX_NUM_WEB_ALL);
        reVal = recv(pClient->m_socket, buffer, MAX_NUM_WEB_ALL, 0);	//��������
        
        if (reVal <= 0) {
            return -1;
        }
        //�յ�����
        else if (reVal > 0)
        {
            if (pClient->WebSocketType() == SocketConnType::Web && pClient->m_webSocketHandler != NULL) {
                // ����web����
                memcpy(pClient->m_webSocketHandler->getbuff(), buffer, MAX_NUM_WEB_ALL);
                pClient->m_webSocketHandler->process(buffer);
                //pClient->m_webSocketHandler->GetParseData(buffer);
            }
            return 1;
        }
    }
    return 1;
}

