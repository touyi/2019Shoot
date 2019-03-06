using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 控制台显示器的四个角的合拢于分开
 */
public class PCBar : MonoBehaviour {

    public Transform[] barTrans;
    Vector3[] bar;      // 记录初始的位置
    Hashtable args = new Hashtable();
    public float timer = 0.3f;
    void Awake()
    {
        bar = new Vector3[barTrans.Length];
        for(int i = 0;i<barTrans.Length;i++)
        {
            bar[i] = barTrans[i].localPosition;
        }
        args.Add("easetype", iTween.EaseType.linear);
        args.Add("time", timer);
        args.Add("islocal", true);
    }
    public void Show()
    {
        for (int i = 0; i < barTrans.Length; i++)
        {
            if (args.ContainsKey("position"))
            {
                args["position"] = bar[i];
            }
            else args.Add("position", bar[i]);
            
            iTween.MoveTo(barTrans[i].gameObject, args);
        }
    }
    public void Hidden()
    {
        for (int i = 0; i < barTrans.Length; i++)
        {
            if (args.ContainsKey("position"))
            {
                args["position"] = new Vector3(0,0,0);
            }
            else args.Add("position", new Vector3(0,0,0));
            iTween.MoveTo(barTrans[i].gameObject, args);
        }
    }

}
