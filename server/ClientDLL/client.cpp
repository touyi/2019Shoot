#include "client.h"
#include "Tools.h"

/**
 *	��ʼ��
 */
BOOL Client::InitClient(const char* ip, int port)
{
    SetServerInfo(ip, port);
	//��ʼ��ȫ�ֱ���
	InitMember();

	//����SOCKET
	if (!InitSockt())
	{
		return FALSE;
	}

	return TRUE;
}
/**
 * ��ʼ��ȫ�ֱ���
 */
void Client::InitMember(void)
{
	InitializeCriticalSection(&cs);

	sClient = INVALID_SOCKET;	//�׽���
    hThreadRecv = NULL;			//���������߳̾��
	hThreadSend = NULL;			//���������߳̾��
	bConnecting = FALSE;		//Ϊ����״̬

    //��ʼ�����ݻ�����
	memset(bufSend, 0, MAX_NUM_BUF);
}

/**
 * �����������׽���
 */
BOOL Client::InitSockt(void)
{
	int			reVal;	//����ֵ
	WSADATA		wsData;	//WSADATA����
	reVal = WSAStartup(MAKEWORD(2,2),&wsData);//��ʼ��Windows Sockets Dll

	//�����׽���
	sClient = socket(AF_INET, SOCK_STREAM, 0);
	if(INVALID_SOCKET == sClient)
		return FALSE;


	//�����׽��ַ�����ģʽ
	unsigned long ul = 1;
	reVal = ioctlsocket(sClient, FIONBIO, (unsigned long*)&ul);
	if (reVal == SOCKET_ERROR)
		return FALSE;

	return TRUE;
}

/**
 * ���ӷ�����
 */
BOOL Client::ConnectServer(void)
{
	int reVal;			//����ֵ
	sockaddr_in serAddr;//��������ַ
	//����Ҫ���ӵ�������ַ
    serAddr.sin_family = AF_INET;
    serAddr.sin_port = htons(this->serverPort);
    serAddr.sin_addr.S_un.S_addr = inet_addr(this->serverIp);

	while(true)
	{
		//���ӷ�����
		reVal = connect(sClient, (struct sockaddr*)&serAddr, sizeof(serAddr));
		//�������Ӵ���
		if(SOCKET_ERROR == reVal)
		{
			int nErrCode = WSAGetLastError();
			if( WSAEWOULDBLOCK == nErrCode || WSAEINVAL == nErrCode)    //���ӻ�û�����
			{
				continue;
			}
			else if (WSAEISCONN == nErrCode)//�����Ѿ����
			{
				break;
			}
			else//����ԭ������ʧ��
			{
				return FALSE;
			}
		}
		if ( reVal == 0 )//���ӳɹ�
			break;
	}

    if (!this->CreateSendAndRecvThread()) {
        this->ExitClient();
        return FALSE;
    }

	bConnecting = TRUE;

	return TRUE;
}
/**
 * �������ͺͽ��������߳�
 */
BOOL Client::CreateSendAndRecvThread(void)
{
	//�����������ݵ��߳�
	unsigned long ulThreadId;
	hThreadRecv = CreateThread(NULL, 0, RecvDataThread, this, 0, &ulThreadId);
	if (NULL == hThreadRecv)
		return FALSE;

	//�����������ݵ��߳�
	hThreadSend = CreateThread(NULL, 0, SendDataThread, this, 0, &ulThreadId);
	if (NULL == hThreadSend)
		return FALSE;

	return TRUE;
}
/**
 * ���������߳�
 */
DWORD __stdcall	Client::RecvDataThread(void* pParam)
{
    Client* client = static_cast<Client*>(pParam);
    if (client == NULL) {
        return 0;
    }
	int		reVal;				    //����ֵ
	char    bufRecv[MAX_NUM_DATA];   //�������ݻ�����

	while(client->bConnecting)			    //����״̬
	{
        Sleep(FRAME_TIME);
        memset(bufRecv, 0, MAX_NUM_DATA);
		reVal = recv(client->sClient, bufRecv, MAX_NUM_DATA, 0);//��������
		if (SOCKET_ERROR == reVal)
		{
			int nErrCode = WSAGetLastError();
			if (WSAEWOULDBLOCK == nErrCode)			//�������ݻ�����������
			{
				continue;							//������������
			}else{
                client->bConnecting = FALSE;
				return 0;							//�߳��˳�
			}
		}

		if ( reVal == 0)							//�������ر�������
		{
            client->bConnecting = FALSE;
            client->bSend = FALSE;
            memset(bufRecv, 0, MAX_NUM_BUF);		//��ս��ջ�����
            client->ExitClient();
			return 0;								//�߳��˳�
		}
		if(reVal > 0)
        {

            //if(('E'==bufRecv[0] || 'e'==bufRecv[0]))     //�ж��Ƿ��˳�
            //{
            //    client->bConnecting = FALSE;
            //    client->bSend = FALSE;
            //    memset(bufRecv, 0, MAX_NUM_BUF);		//��ս��ջ�����
            //    client->ExitClient();
            //}
            //��ʾ����
            DataBuffer* data = new DataBuffer();
            memset(data, 0, sizeof(DataBuffer));
            memcpy(data->buffer, bufRecv, MAX_NUM_DATA);
            client->m_safeQueue.push(data);
        }
	}
	return 0;
}
/**
 * ���������߳�
 */
DWORD __stdcall	Client::SendDataThread(void* pParam)
{
    Client* client = static_cast<Client*>(pParam);
    if (client == NULL) {
        return 0;
    }
	while(client->bConnecting)						//����״̬
	{
		if (client->bSend)						//��������
		{
            EnterCriticalSection(&client->cs);	//�����ٽ���
			while(TRUE)
            {
                int val = send(client->sClient, client->bufSend, MAX_NUM_BUF,0);

                //�����ش���
                if (SOCKET_ERROR == val)
                {
                    int nErrCode = WSAGetLastError();
                    if(WSAEWOULDBLOCK == nErrCode)		//���ͻ�����������
                    {
                        continue;						//����ѭ��
                    }else
                    {
                        LeaveCriticalSection(&client->cs);	//�뿪�ٽ���
                        return 0;
                    }
                }

                client->bSend = FALSE;			//����״̬
                    break;					//����for
            }
            LeaveCriticalSection(&client->cs);	//�뿪�ٽ���
		}
    }
	return 0;
}
/**
 * �������ݺ���ʾ���
 */
void Client::InputAndOutput(void)
{
    char cInput[MAX_NUM_BUF];	//�û����뻺����
    while(bConnecting)			//����״̬
	{
		memset(cInput, 0, MAX_NUM_BUF);
		cin >> cInput;			        //������ʽ
        EnterCriticalSection(&cs);		//�����ٽ���
        memset(bufSend, 0, sizeof(bufSend));
		memcpy(bufSend, cInput, strlen(cInput));
		LeaveCriticalSection(&cs);		//�뿪�ٽ���
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
 * �ͻ����˳�
 */
void Client::ExitClient(void)
{
	DeleteCriticalSection(&cs);
    CloseHandle(hThreadRecv);
	CloseHandle(hThreadSend);
    memset(bufSend, 0, MAX_NUM_BUF);
	closesocket(sClient);
	WSACleanup();
}

void Client::SendData(int proto, char * content)
{
    
    if (bConnecting)			//����״̬
    {
        char cInput[MAX_NUM_BUF];	//�û����뻺����
        Int32ToChar(cInput, proto);
        memcpy(cInput + 4, content, strlen(content));
        EnterCriticalSection(&cs);		//�����ٽ���
        memset(bufSend, 0, sizeof(bufSend));
        memcpy(bufSend, cInput, strlen(cInput));
        LeaveCriticalSection(&cs);		//�뿪�ٽ���
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


