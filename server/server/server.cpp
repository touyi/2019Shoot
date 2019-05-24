#include "PartnerProcess.h"
#include "sclient.h"
#include "Log.h"
#include "protocol/Protocol.pb.h"

//struct ScreenPartner {
//public:
//    void SetScreen(CClient* client) {
//        std::lock_guard<std::mutex>lk(mut);
//        Screen = client;
//    }
//    
//    void SetMobilde(CClient* client) {
//        std::lock_guard<std::mutex>lk(mut);
//        Mobile = client;
//    }
//    CClient* GetScreen() {
//        return Screen;
//    }
//    CClient* GetMobile() {
//        return Mobile;
//    }
//
//    bool IsPair() {
//        return Mobile != NULL && Screen != NULL;
//    }
//private:
//    mutable std::mutex mut;
//    CClient* Screen = NULL;
//    CClient* Mobile = NULL;
//};

/**
 * ȫ�ֱ���
 */
BOOL	bRunning;							//��ͻ��˵�����״̬
BOOL    bSend;                              //���ͱ��λ
BOOL    clientConn;                         //���ӿͻ��˱��
SOCKET	sServer;							//�����������׽���
CRITICAL_SECTION  cs;			            //�������ݵ��ٽ�������
HANDLE	hAcceptThread;						//���ݴ����߳̾��
HANDLE	hCleanThread;						//���ݽ����߳�
ClIENTVECTOR clientvector;                  //�洢���׽���
PartnerProcess OnlyPartner;                  //Ψһ�����

/**
 * ��ʼ��
 */
BOOL initSever(void)
{
    //��ʼ��ȫ�ֱ���
	initMember();

	//��ʼ��SOCKET
	if (!initSocket())
		return FALSE;

	return TRUE;
}

/**
 * ��ʼ��ȫ�ֱ���
 */
void	initMember(void)
{
	InitializeCriticalSection(&cs);				            //��ʼ���ٽ���
	bSend = FALSE;
	clientConn = FALSE;
	bRunning = FALSE;									    //������Ϊû������״̬
	hAcceptThread = NULL;									//����ΪNULL
	hCleanThread = NULL;
	sServer = INVALID_SOCKET;								//����Ϊ��Ч���׽���
	clientvector.clear();									//�������
}

/**
 *  ��ʼ��SOCKET
 */
bool initSocket(void)
{
	//����ֵ
	int reVal;

	//��ʼ��Windows Sockets DLL
	WSADATA  wsData;
	reVal = WSAStartup(MAKEWORD(2,2),&wsData);

	//�����׽���
	sServer = socket(AF_INET, SOCK_STREAM, 0);
	if(INVALID_SOCKET== sServer)
		return FALSE;

	//�����׽��ַ�����ģʽ
	//unsigned long ul = 1;
	//reVal = ioctlsocket(sServer, FIONBIO, (unsigned long*)&ul);
	//if (SOCKET_ERROR == reVal)
	//	return FALSE;

	//���׽���
	sockaddr_in serAddr;
	serAddr.sin_family = AF_INET;
	serAddr.sin_port = htons(SERVERPORT);
	serAddr.sin_addr.S_un.S_addr = INADDR_ANY;
	reVal = bind(sServer, (struct sockaddr*)&serAddr, sizeof(serAddr));
	if(SOCKET_ERROR == reVal )
		return FALSE;

	//����
	reVal = listen(sServer, CONN_NUM);
	if(SOCKET_ERROR == reVal)
		return FALSE;

	return TRUE;
}

/**
 *  ��������
 */
bool startService(void)
{
    BOOL reVal = TRUE;	//����ֵ
	//char cInput;		//�����ַ�
	//do
	//{
	//	cin >> cInput;
	//	if ('s' == cInput || 'S' == cInput)
	//	{
	//		if (createCleanAndAcceptThread())	//���ܿͻ���������߳�
	//		{
	//			showServerStartMsg(TRUE);		//�����̳߳ɹ���Ϣ
	//		}else{
	//			reVal = FALSE;
	//		}
	//		break;//����ѭ����
	//	}else{
	//		showTipMsg(START_SERVER);
	//	}

	//} while(cInput != 's' && cInput != 'S'); //��������'s'����'S'�ַ�

 //   cin.sync();                     //������뻺����

    if (createCleanAndAcceptThread())	//���ܿͻ���������߳�
    {
        LogManager::Log("�����������ɹ�����");
    }
    else {
        LogManager::Log("����������ʧ�ܣ���");
        reVal = FALSE;
    }

	return reVal;

}

/**
 * ����������Դ�ͽ��ܿͻ��������߳�
 */
BOOL createCleanAndAcceptThread(void)
{
    bRunning = TRUE;//���÷�����Ϊ����״̬

	//�����ͷ���Դ�߳�
	unsigned long ulThreadId;
	//�������տͻ��������߳�
	hAcceptThread = CreateThread(NULL, 0, acceptThread, NULL, 0, &ulThreadId);
	if( NULL == hAcceptThread)
	{
		bRunning = FALSE;
		return FALSE;
	}
	else
    {
		CloseHandle(hAcceptThread);
	}
	//�������������߳�
	hCleanThread = CreateThread(NULL, 0, cleanThread, NULL, 0, &ulThreadId);
	if( NULL == hCleanThread)
	{
		return FALSE;
	}
	else
    {
		CloseHandle(hCleanThread);
	}
	return TRUE;
}

/**
 * ���ܿͻ�������
 */
DWORD __stdcall acceptThread(void* pParam)
{
    SOCKET  sAccept;							                        //���ܿͻ������ӵ��׽���
	sockaddr_in addrClient;						                        //�ͻ���SOCKET��ַ

	while(bRunning)						                                //��������״̬
	{
		memset(&addrClient, 0, sizeof(sockaddr_in));					//��ʼ��
		int	lenClient = sizeof(sockaddr_in);				        	//��ַ����
		sAccept = accept(sServer, (sockaddr*)&addrClient, &lenClient);	//���ܿͻ�����
		if(INVALID_SOCKET == sAccept )
		{
			int nErrCode = WSAGetLastError();
			if(nErrCode == WSAEWOULDBLOCK)	                            //�޷��������һ�����赲���׽��ֲ���
			{
				Sleep(TIMEFOR_THREAD_SLEEP);
				continue;                                               //�����ȴ�
			}
			else
            {
				return 0;                                               //�߳��˳�
			}

		}
		else//���ܿͻ��˵�����
		{
		    clientConn = TRUE;          //�Ѿ������Ͽͻ���
		    CClient *pClient = new CClient(sAccept, addrClient);
		    EnterCriticalSection(&cs);
            //��ʾ�ͻ��˵�IP�Ͷ˿�
            char *pClientIP = inet_ntoa(addrClient.sin_addr);
            u_short  clientPort = ntohs(addrClient.sin_port);
            std::stringstream ss;
            ss <<"Accept a client IP: "<<pClientIP<<"\tPort: "<< clientPort;
            LogManager::Log(ss.str());
			clientvector.push_back(pClient);            //��������
            LeaveCriticalSection(&cs);

            pClient->StartRuning();
		}
	}
	return 0;//�߳��˳�
}

/**
 * ������Դ�߳�
 */
DWORD __stdcall cleanThread(void* pParam)
 {
    while (bRunning)                  //��������������
	{
		EnterCriticalSection(&cs);//�����ٽ���

		//�����Ѿ��Ͽ���δ������ӿͻ����ڴ�ռ�
		ClIENTVECTOR::iterator iter = clientvector.begin();
		for (iter; iter != clientvector.end();)
		{
			CClient *pClient = (CClient*)*iter;
			if (!pClient->IsConning())			//�ͻ����߳��Ѿ��˳�
			{
				iter = clientvector.erase(iter);	//ɾ���ڵ�
                LogManager::Log("���ӶϿ���" + string(*pClient));
				delete pClient;				//�ͷ��ڴ�
				pClient = NULL;
			}else{
				iter++;						//ָ������
			}
		}
        // �����Ѿ��Ͽ���������ӿͻ����ڴ�ռ�<-�����ÿ��ѭ����Run()�����ж����� ��ֹȡ��
        

		if(clientvector.size() == 0)
        {
            clientConn = FALSE;
        }

		LeaveCriticalSection(&cs);          //�뿪�ٽ���

		Sleep(TIMEFOR_THREAD_HELP);
	}


	//������ֹͣ����
	if (!bRunning)
	{
		//�Ͽ�ÿ������,�߳��˳�
		EnterCriticalSection(&cs);
		ClIENTVECTOR::iterator iter = clientvector.begin();
		for (iter; iter != clientvector.end();)
		{
			CClient *pClient = (CClient*)*iter;
			//����ͻ��˵����ӻ����ڣ���Ͽ����ӣ��߳��˳�
			if (pClient->IsConning())
			{
				pClient->DisConning();
			}
			++iter;
		}
		//�뿪�ٽ���
		LeaveCriticalSection(&cs);

		//�����ӿͻ����߳�ʱ�䣬ʹ���Զ��˳�
		Sleep(TIMEFOR_THREAD_HELP);
	}

	clientvector.clear();		//�������
	clientConn = FALSE;

	return 0;
 }
/*
* �������ӶϿ���
*/
bool PartnerStateRight() {
    // �������ӶϿ���
    CClient* mobile = OnlyPartner.GetMobile();
    CClient* screen = OnlyPartner.GetScreen();
    if (mobile != NULL && screen != NULL && mobile->IsConning() && screen->IsConning()) {
        return true;
    }
    SendCMDToClient(OnlyPartner.GetScreen(), Message::CmdType::GameEnd);
    EnterCriticalSection(&cs);//�����ٽ���
    if (mobile != NULL) {
        OnlyPartner.SetMobilde(NULL);
        if (!mobile->IsConning()) {
            LogManager::Log("���ӶϿ���" + string(*mobile));
            //TODO �������ӶϿ� ��Ҫ����PC��Ϸ����
            delete mobile;
        }
        else {
            clientvector.push_back(mobile);
        }
    }
    if (screen != NULL) {
        OnlyPartner.SetScreen(NULL);
        if (!screen->IsConning()) {
            LogManager::Log("���ӶϿ���" + string(*screen));
            delete screen;
        }
        else {
            clientvector.push_back(screen);
        }
    }
    LeaveCriticalSection(&cs);
    
    return false;
}
/**
 * ��������
 */
 void Run(void)
 {

    while(bRunning)
    {
        
        if (OnlyPartner.IsPair()) {
            if (!PartnerStateRight()) {
                continue;
            }
            OnlyPartner.ExchangeData();
            //memset(sendBuf, 0, MAX_NUM_BUF);//��ս��ջ�����
            //Message::KeyChange keychange;
            //auto keydata = keychange.add_keydatas();
            //keydata->set_key(0);
            //keydata->set_keystate(1);
            //int size = keychange.ByteSize();
            //keychange.SerializeToArray(sendBuf, size);
            ////��������
            //handleData(123, sendBuf, keychange.ByteSize(), 0);
        }
        else {
            // ���Դ�С��Ļ���
            for (ClIENTVECTOR::iterator iter = clientvector.begin(); iter != clientvector.end();) {
                if (OnlyPartner.GetMobile() == NULL && (*iter)->WebSocketType() == SocketConnType::Web) {
                    OnlyPartner.SetMobilde(*iter);
                    iter = clientvector.erase(iter);
                }
                else if (OnlyPartner.GetScreen() == NULL && (*iter)->WebSocketType() == SocketConnType::Normal) {
                    OnlyPartner.SetScreen(*iter);
                    iter = clientvector.erase(iter);
                }
                else {
                    iter++;
                }
            }
            if (OnlyPartner.IsPair()) {
                LogManager::Debug(string("��Գɹ���") + string(*OnlyPartner.GetScreen()) + string(*OnlyPartner.GetMobile()));
                // ��Գɹ� ���Ϳ�ʼCMD
                SendCMDToClient(OnlyPartner.GetScreen(), Message::CmdType::GameBegin);
            }
        }
        
        Sleep(FRAME_TIME);
    }
 }


/**
 *  ѡ��ģʽ��������
 */
 void handleData(UShort proto, const char* buffer, UShort bufferlen, ClientID id)
 {
    if(buffer == NULL)
    {
        return;
    }
    if(id >= 0)
    {
        CClient *sClient;
        EnterCriticalSection(&cs);
        if(id < clientvector.size())
        {
            sClient = clientvector.at(id);     //���͵�ָ���ͻ���
            sClient->SetFrameSend(proto, buffer, bufferlen);
        }
        else                                    //���ڷ�Χ
        {
            LogManager::Error("The client isn't in scope!");
        }
        LeaveCriticalSection(&cs);
    }
    else
    {
        // TODO ȫ������
        EnterCriticalSection(&cs);
        buffer += strlen(WRITE_ALL);
        bSend = TRUE;
        LeaveCriticalSection(&cs);
    }
    //else if('e'==buffer[0] || 'E'== buffer[0])     //�ж��Ƿ��˳�
    //{
    //    bConning = FALSE;
    //    showServerExitMsg();
    //    Sleep(TIMEFOR_THREAD_EXIT);
    //    exitServer();
    //}
    //else
    //{
    //    cout <<"Input error!!"<<endl;
    //}

    
 }

/**
 *  �ͷ���Դ
 */
void  exitServer(void)
{
	closesocket(sServer);					//�ر�SOCKET
	WSACleanup();							//ж��Windows Sockets DLL
}

void SendCMDToClient(CClient * client, Message::CmdType type)
{
    if (client == NULL)return;
    if(type == Message::CmdType::GameEnd)
        LogManager::Debug("Send");
    DataBuffer buffer;
    Message::CommandList cmdList;
    auto cmd = cmdList.add_commanddatas();
    cmd->set_ctype(type);
    int size = cmdList.ByteSize();
    buffer.Package.head.proto = 2;
    buffer.Package.head.Length = size + HEAD_SIZE;
    cmdList.SerializeToArray(buffer.Package.datas, size);
    client->SetFrameSend(buffer);
}

//void showTipMsg(int input)
//{
//    EnterCriticalSection(&cs);
//	if (START_SERVER == input)          //����������
//	{
//		cout << "**********************" << endl;
//		cout << "* s(S): Start server *" << endl;
//		cout << "* e(E): Exit  server *" << endl;
//		cout << "**********************" << endl;
//		cout << "Please input:" ;
//
//	}
//	else if(INPUT_DATA == input)
//    {
//        cout << "*******************************************" << endl;
//        cout << "* please connect clients,then send data   *" << endl;
//		cout << "* write+num+data:Send data to client-num  *" << endl;
//		cout << "*   all+data:Send data to all clients     *" << endl;
//		cout << "*          e(E): Exit  server             *" << endl;
//		cout << "*******************************************" << endl;
//		cout << "Please input:" <<endl;
//    }
//	 LeaveCriticalSection(&cs);
//}

/**
 * ��ʾ�����������ɹ���ʧ����Ϣ
 */
//void  showServerStartMsg(BOOL bSuc)
//{
//	if (bSuc)
//	{
//		cout << "**********************" << endl;
//		cout << "* Server succeeded!  *" << endl;
//		cout << "**********************" << endl;
//	}else{
//		cout << "**********************" << endl;
//		cout << "* Server failed   !  *" << endl;
//		cout << "**********************" << endl;
//	}
//
//}

/**
 * ��ʾ�������˳���Ϣ
 */
//void  showServerExitMsg(void)
//{
//
//	cout << "**********************" << endl;
//	cout << "* Server exit...     *" << endl;
//	cout << "**********************" << endl;
//}

