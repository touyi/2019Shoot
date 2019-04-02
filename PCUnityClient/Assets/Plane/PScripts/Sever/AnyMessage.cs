using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AnyMessage : MonoBehaviour
{
    
    Client clientMove;
    Client clientButton;

    int serverPort1;
    int serverPort2;
    public string serverIp;

    void Start()
    {
#if true // 本地调试为false 联网调试为 true

        //获取服务器IP和本机ip
        StreamReader sr = new StreamReader(Application.streamingAssetsPath + "/text/ip/MoveSocketip.txt", System.Text.Encoding.Default);
        serverIp = sr.ReadLine();
        serverPort1 = int.Parse(sr.ReadLine());
        serverPort2 = int.Parse(sr.ReadLine());
        int serverPort3 = int.Parse(sr.ReadLine());

        sr.Close();
        clientMove =new Client(serverIp, serverPort1);
        clientButton = new Client(serverIp, serverPort2);
        clientMove.Start();
        clientButton.Start();
        Debug.Log("两个端口都对接成功");
        if(serverPort3==0)
            PServer._Instances.SeverStart("http://"+serverIp + "/bigsmallthree/index.jsp");
        else PServer._Instances.SeverStart("http://" + serverIp+":" + serverPort3 + "/bigsmallthree/index.jsp");
#endif
    }
    void Update()
    {
        if (clientButton!=null && clientMove!=null && clientMove.isConnect()&& clientMove.isConnect())
        {
            string messageMove = clientMove.GetMessage();
            if(messageMove!=null && messageMove!="")
                AnalysisString(messageMove);
            string messageButton = clientButton.GetMessage();
            if (messageButton != null && messageButton!="")
                AnalysisString(messageButton);
        }
    }
    bool AnalysisString(string msg)
    {
        string[] coms = msg.Split('#');

        for (int i = 0; i < coms.Length; i++)
        {
            if (coms[i].Length > 0)
            {
                switch (coms[i][0])
                {
                    case 'O': // 位移指令
                        coms[i] = coms[i].Substring(1);
                        string[] nums = coms[i].Split(' ');
                        try
                        {
                            if (nums.Length == 3)
                            {
                                ClientKeyInfo.Rota = new Vector3(float.Parse(nums[0]), -float.Parse(nums[1]), float.Parse(nums[2]));
                            }
                        }
                        catch (Exception ex)
                        {
                            break;
                        }
                        break;
                    case 'F': // fire指令
                        ClientKeyInfo.Firecd = 0.2f;
                        ClientKeyInfo.SSure = true;
                        break;
                    case 'C': // 切换指令
                        ClientKeyInfo.CChange = true;
                        break;
                    case 'S': // 确定或者开始命令
                        ClientKeyInfo.SSure = true;
                        break;
                    case 'R': // 重新开始命令
                        break;
                    case 'E': // 用户断开
                        Debug.Log("end");
                        PServer._Instances.UserEnd();
                        FGameInfo.Instance.IsUserConnect = false;
                        break;
                    case 'I': // 用户接入
                        PServer._Instances.UserBegin();
                        FGameInfo.Instance.IsUserConnect = true;
                        Debug.Log("user in");
                        break;
                    case 'A': // 服务器退出
                        
                        return false;
                    default:
                        break;

                }
            }
        }
        return true;
    }
    private void OnApplicationQuit()
    {
        
        PServer._Instances.ServerEnd();
    }
}
