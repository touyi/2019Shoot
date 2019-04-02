using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MITween {

    static Hashtable args = new Hashtable();
    static void Init(float timer,bool islocal, iTween.EaseType easeType)
    {
        // 动画时间
        if(args.ContainsKey("time"))
            args["time"] = timer;
        else
            args.Add("time", timer);
        // 是否局部坐标
        if (args.ContainsKey("islocal"))
            args["islocal"] = islocal;
        else
            args.Add("islocal", islocal);
        // 变换曲线
        if (args.ContainsKey("easetype"))
            args["easetype"] = easeType;
        else
            args.Add("easetype", easeType);
    }
    public static void Tween(GameObject go, Vector3 toscal, float timer, bool islocal = false,iTween.EaseType easeType = iTween.EaseType.linear)
    {
        Init(timer, islocal, easeType);
        if (args.ContainsKey("scale"))
        {
            args["scale"] = toscal;
        }
        else args.Add("scale", toscal);
        iTween.ScaleTo(go, args);
    }
}
