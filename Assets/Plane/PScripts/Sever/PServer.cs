using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System;
using System.IO;
using System.Runtime.InteropServices;

public class PServer {

    
    // 服务器状态探针
    public delegate void OnListener(object inf = null);
    public static event OnListener OnServerStart;
    public static event OnListener OnServerEnd;
    public static event OnListener OnUserBegin;
    public static event OnListener OnUserEnd;
    #region 单例
    static PServer _instances = null;

    public static PServer _Instances
    {
        get
        {
            if(_instances==null)
            {
                _instances = new PServer();
            }
            return _instances;
        }
        
    }
    #endregion

    public void SeverStart(object inf = null)
    {
        Debug.Log(inf);
        if(OnServerStart!=null)
            OnServerStart(inf);
    }
    public void ServerEnd(object inf = null)
    {
        if(OnServerEnd!=null)
            OnServerEnd(inf);
    }
    public void UserBegin(object inf = null)
    {
        if(OnUserBegin!=null)
            OnUserBegin(inf);
    }
    public void UserEnd(object inf = null)
    {
        if (OnUserEnd != null)
            OnUserEnd(inf);
    }
}
