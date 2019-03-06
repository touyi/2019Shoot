using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class ClientKeyInfo : MonoBehaviour {
    // 使用object对bool类型装箱 方便原子操作
    private static object change = false;
    private static object cChange = false;
    private static object sure = false;
    private static object sSure = false;

    private static Vector3 rota;
    public static bool isConnect = false;

    public static bool fire = false;
    private static float firecd = 0f;
    static bool isChangeRota = false;
    public static bool CChange
    {
        get
        {
            return (bool)change;
        }

        set
        {
            Interlocked.Exchange(ref cChange, value);
        }
    }
    public static bool SSure
    {
        get
        {
            return (bool)sure;
        }

        set
        {
            Interlocked.Exchange(ref sSure, value);
        }
    }
    // firecd 加锁
    static readonly object obj = new object();
    public static float Firecd
    {
        get
        {
            return firecd;
        }

        set
        {
            lock (obj)
            {
                firecd = value;
            }
        }
    }

    public static Vector3 Rota
    {
        get
        {
            return rota;
        }

        set
        {
            rota = value;
            isChangeRota = true;
           // FGameObjectBar._instance.gunBase.GetComponent<GunRotationControl>().SetScreenPos(rota);
        }
    }



    // 单次按键响应函数
    static void OnceKey(ref object judgeKey,ref object useKey)
    {
        if((bool)judgeKey==true && (bool)useKey==true)
        {
            Interlocked.Exchange(ref judgeKey, false);
            Interlocked.Exchange(ref useKey, false);
        }
        else if((bool)judgeKey==true && (bool)useKey==false)
        {
            Interlocked.Exchange(ref useKey, true);
        }
    }

    private void LateUpdate()
    {
        OnceKey(ref cChange, ref change);
        OnceKey(ref sSure, ref sure);
        if(Firecd>0)
        {
            fire = true;
            Firecd -= Time.deltaTime;
        }
        else
        {
            fire = false;
        }
        if(isChangeRota)
        {
            isChangeRota = false;
            FGameObjectBar._instance.gunBase.GetComponent<GunRotationControl>().SetScreenPos(rota);
        }
    }
}
