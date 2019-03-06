using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System;
using System.Threading;

using UnityEngine;
using System.Text;

public class Client{


    private  Socket m_serverSocket;
    private Socket  m_clientSocket;
    private byte[] m_result = new byte[1024];

    private int m_serverPort;
    private bool m_isConnect;
    private string m_serverIp;
    private string m_nowMessage;
    //public static bool st_isEnd = false;
    static readonly object obj = new object();
    private Thread m_clientThread;
    public Client(string ip,int port)
    {
        FGameInfo.Instance.OnPlayerDead += SendDeadToServer;
        m_isConnect = false;
        m_serverIp = ip;
        m_serverPort = port;
        PServer.OnServerEnd += End;
    }
    public void End(object a)
    {
        if (m_clientThread != null)
        {
            m_clientThread.Abort();
        }
        if (m_clientSocket != null && m_clientSocket.Connected)
        {
            m_clientSocket.Close();
        }
    }
    public bool isConnect()
    {
        return m_isConnect;
    }
    public string GetMessage()
    {
        string message = m_nowMessage;
        lock (obj)
        {
            m_nowMessage = "";
        }
        return message;
    }
    public void Start()
    {
        
        m_clientThread = new Thread(StartConnect);
        m_clientThread.Start();
    }
    void SendDeadToServer()
    {
        
        try
        {
            m_clientSocket.Send(Encoding.UTF8.GetBytes("#Dead\n"));
            PServer._Instances.UserEnd();
            FGameInfo.Instance.IsUserConnect = false;
            Debug.Log("死亡命令发送");
        }
        catch(Exception e)
        {
            m_clientSocket.Shutdown(SocketShutdown.Both);
        }
    }
    private void StartConnect()
    {

        Debug.Log(m_serverIp);
        while (true)
        {
            IPAddress ip = IPAddress.Parse(m_serverIp);
            m_clientSocket = new Socket(AddressFamily.InterNetwork
                , SocketType.Stream
                , ProtocolType.Tcp);
            try
            {
                Debug.Log(m_serverPort);
                m_clientSocket.Connect(new IPEndPoint(ip, m_serverPort));
                m_clientSocket.Send(Encoding.UTF8.GetBytes("#hello\n"));
                Debug.Log("发送成功");
                StartRecieve();
            }
            catch
            {
                Debug.Log("连接失败，尝试重连");
            }
        }  
    }
    private void StartRecieve()
    {
        while (true)
        {
            try
            {
                m_isConnect = true;
                int receiveNumber = m_clientSocket.Receive(m_result);
                string message = System.Text.Encoding.UTF8.GetString(m_result
                    , 0
                    , receiveNumber);
                lock (obj)
                {
                    m_nowMessage += message;
                }
            }
            catch
            {
                BreakConnect();
                Debug.Log("接收时中断，尝试重连");
                m_clientThread = new Thread(StartConnect);
                m_clientThread.Start();
                break;
            }
        }
    }
    private void BreakConnect()
    {
        m_isConnect = false;
        m_clientSocket.Shutdown(SocketShutdown.Both);
        IPAddress ip = IPAddress.Parse(m_serverIp);      
    }
    
}
