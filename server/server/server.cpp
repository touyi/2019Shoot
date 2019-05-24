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
 * 全局变量
 */
BOOL	bRunning;							//与客户端的连接状态
BOOL    bSend;                              //发送标记位
BOOL    clientConn;                         //连接客户端标记
SOCKET	sServer;							//服务器监听套接字
CRITICAL_SECTION  cs;			            //保护数据的临界区对象
HANDLE	hAcceptThread;						//数据处理线程句柄
HANDLE	hCleanThread;						//数据接收线程
ClIENTVECTOR clientvector;                  //存储子套接字
PartnerProcess OnlyPartner;                  //唯一配对项

/**
 * 初始化
 */
BOOL initSever(void)
{
    //初始化全局变量
	initMember();

	//初始化SOCKET
	if (!initSocket())
		return FALSE;

	return TRUE;
}

/**
 * 初始化全局变量
 */
void	initMember(void)
{
	InitializeCriticalSection(&cs);				            //初始化临界区
	bSend = FALSE;
	clientConn = FALSE;
	bRunning = FALSE;									    //服务器为没有运行状态
	hAcceptThread = NULL;									//设置为NULL
	hCleanThread = NULL;
	sServer = INVALID_SOCKET;								//设置为无效的套接字
	clientvector.clear();									//清空向量
}

/**
 *  初始化SOCKET
 */
bool initSocket(void)
{
	//返回值
	int reVal;

	//初始化Windows Sockets DLL
	WSADATA  wsData;
	reVal = WSAStartup(MAKEWORD(2,2),&wsData);

	//创建套接字
	sServer = socket(AF_INET, SOCK_STREAM, 0);
	if(INVALID_SOCKET== sServer)
		return FALSE;

	//设置套接字非阻塞模式
	//unsigned long ul = 1;
	//reVal = ioctlsocket(sServer, FIONBIO, (unsigned long*)&ul);
	//if (SOCKET_ERROR == reVal)
	//	return FALSE;

	//绑定套接字
	sockaddr_in serAddr;
	serAddr.sin_family = AF_INET;
	serAddr.sin_port = htons(SERVERPORT);
	serAddr.sin_addr.S_un.S_addr = INADDR_ANY;
	reVal = bind(sServer, (struct sockaddr*)&serAddr, sizeof(serAddr));
	if(SOCKET_ERROR == reVal )
		return FALSE;

	//监听
	reVal = listen(sServer, CONN_NUM);
	if(SOCKET_ERROR == reVal)
		return FALSE;

	return TRUE;
}

/**
 *  启动服务
 */
bool startService(void)
{
    BOOL reVal = TRUE;	//返回值
	//char cInput;		//输入字符
	//do
	//{
	//	cin >> cInput;
	//	if ('s' == cInput || 'S' == cInput)
	//	{
	//		if (createCleanAndAcceptThread())	//接受客户端请求的线程
	//		{
	//			showServerStartMsg(TRUE);		//创建线程成功信息
	//		}else{
	//			reVal = FALSE;
	//		}
	//		break;//跳出循环体
	//	}else{
	//		showTipMsg(START_SERVER);
	//	}

	//} while(cInput != 's' && cInput != 'S'); //必须输入's'或者'S'字符

 //   cin.sync();                     //清空输入缓冲区

    if (createCleanAndAcceptThread())	//接受客户端请求的线程
    {
        LogManager::Log("服务器启动成功！！");
    }
    else {
        LogManager::Log("服务器启动失败！！");
        reVal = FALSE;
    }

	return reVal;

}

/**
 * 产生清理资源和接受客户端连接线程
 */
BOOL createCleanAndAcceptThread(void)
{
    bRunning = TRUE;//设置服务器为运行状态

	//创建释放资源线程
	unsigned long ulThreadId;
	//创建接收客户端请求线程
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
	//创建接收数据线程
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
 * 接受客户端连接
 */
DWORD __stdcall acceptThread(void* pParam)
{
    SOCKET  sAccept;							                        //接受客户端连接的套接字
	sockaddr_in addrClient;						                        //客户端SOCKET地址

	while(bRunning)						                                //服务器的状态
	{
		memset(&addrClient, 0, sizeof(sockaddr_in));					//初始化
		int	lenClient = sizeof(sockaddr_in);				        	//地址长度
		sAccept = accept(sServer, (sockaddr*)&addrClient, &lenClient);	//接受客户请求
		if(INVALID_SOCKET == sAccept )
		{
			int nErrCode = WSAGetLastError();
			if(nErrCode == WSAEWOULDBLOCK)	                            //无法立即完成一个非阻挡性套接字操作
			{
				Sleep(TIMEFOR_THREAD_SLEEP);
				continue;                                               //继续等待
			}
			else
            {
				return 0;                                               //线程退出
			}

		}
		else//接受客户端的请求
		{
		    clientConn = TRUE;          //已经连接上客户端
		    CClient *pClient = new CClient(sAccept, addrClient);
		    EnterCriticalSection(&cs);
            //显示客户端的IP和端口
            char *pClientIP = inet_ntoa(addrClient.sin_addr);
            u_short  clientPort = ntohs(addrClient.sin_port);
            std::stringstream ss;
            ss <<"Accept a client IP: "<<pClientIP<<"\tPort: "<< clientPort;
            LogManager::Log(ss.str());
			clientvector.push_back(pClient);            //加入容器
            LeaveCriticalSection(&cs);

            pClient->StartRuning();
		}
	}
	return 0;//线程退出
}

/**
 * 清理资源线程
 */
DWORD __stdcall cleanThread(void* pParam)
 {
    while (bRunning)                  //服务器正在运行
	{
		EnterCriticalSection(&cs);//进入临界区

		//清理已经断开的未配对连接客户端内存空间
		ClIENTVECTOR::iterator iter = clientvector.begin();
		for (iter; iter != clientvector.end();)
		{
			CClient *pClient = (CClient*)*iter;
			if (!pClient->IsConning())			//客户端线程已经退出
			{
				iter = clientvector.erase(iter);	//删除节点
                LogManager::Log("连接断开：" + string(*pClient));
				delete pClient;				//释放内存
				pClient = NULL;
			}else{
				iter++;						//指针下移
			}
		}
        // 清理已经断开的配对连接客户端内存空间<-这个在每次循环（Run()）中判断清理 防止取空
        

		if(clientvector.size() == 0)
        {
            clientConn = FALSE;
        }

		LeaveCriticalSection(&cs);          //离开临界区

		Sleep(TIMEFOR_THREAD_HELP);
	}


	//服务器停止工作
	if (!bRunning)
	{
		//断开每个连接,线程退出
		EnterCriticalSection(&cs);
		ClIENTVECTOR::iterator iter = clientvector.begin();
		for (iter; iter != clientvector.end();)
		{
			CClient *pClient = (CClient*)*iter;
			//如果客户端的连接还存在，则断开连接，线程退出
			if (pClient->IsConning())
			{
				pClient->DisConning();
			}
			++iter;
		}
		//离开临界区
		LeaveCriticalSection(&cs);

		//给连接客户端线程时间，使其自动退出
		Sleep(TIMEFOR_THREAD_HELP);
	}

	clientvector.clear();		//清空链表
	clientConn = FALSE;

	return 0;
 }
/*
* 清理链接断开的
*/
bool PartnerStateRight() {
    // 清理连接断开的
    CClient* mobile = OnlyPartner.GetMobile();
    CClient* screen = OnlyPartner.GetScreen();
    if (mobile != NULL && screen != NULL && mobile->IsConning() && screen->IsConning()) {
        return true;
    }
    SendCMDToClient(OnlyPartner.GetScreen(), Message::CmdType::GameEnd);
    EnterCriticalSection(&cs);//进入临界区
    if (mobile != NULL) {
        OnlyPartner.SetMobilde(NULL);
        if (!mobile->IsConning()) {
            LogManager::Log("连接断开：" + string(*mobile));
            //TODO 网络连接断开 需要处理PC游戏结束
            delete mobile;
        }
        else {
            clientvector.push_back(mobile);
        }
    }
    if (screen != NULL) {
        OnlyPartner.SetScreen(NULL);
        if (!screen->IsConning()) {
            LogManager::Log("连接断开：" + string(*screen));
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
 * 处理数据
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
            //memset(sendBuf, 0, MAX_NUM_BUF);//清空接收缓冲区
            //Message::KeyChange keychange;
            //auto keydata = keychange.add_keydatas();
            //keydata->set_key(0);
            //keydata->set_keystate(1);
            //int size = keychange.ByteSize();
            //keychange.SerializeToArray(sendBuf, size);
            ////发送数据
            //handleData(123, sendBuf, keychange.ByteSize(), 0);
        }
        else {
            // 尝试大小屏幕配对
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
                LogManager::Debug(string("配对成功：") + string(*OnlyPartner.GetScreen()) + string(*OnlyPartner.GetMobile()));
                // 配对成功 发送开始CMD
                SendCMDToClient(OnlyPartner.GetScreen(), Message::CmdType::GameBegin);
            }
        }
        
        Sleep(FRAME_TIME);
    }
 }


/**
 *  选择模式处理数据
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
            sClient = clientvector.at(id);     //发送到指定客户端
            sClient->SetFrameSend(proto, buffer, bufferlen);
        }
        else                                    //不在范围
        {
            LogManager::Error("The client isn't in scope!");
        }
        LeaveCriticalSection(&cs);
    }
    else
    {
        // TODO 全部发送
        EnterCriticalSection(&cs);
        buffer += strlen(WRITE_ALL);
        bSend = TRUE;
        LeaveCriticalSection(&cs);
    }
    //else if('e'==buffer[0] || 'E'== buffer[0])     //判断是否退出
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
 *  释放资源
 */
void  exitServer(void)
{
	closesocket(sServer);					//关闭SOCKET
	WSACleanup();							//卸载Windows Sockets DLL
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
//	if (START_SERVER == input)          //启动服务器
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
 * 显示启动服务器成功与失败消息
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
 * 显示服务器退出消息
 */
//void  showServerExitMsg(void)
//{
//
//	cout << "**********************" << endl;
//	cout << "* Server exit...     *" << endl;
//	cout << "**********************" << endl;
//}

