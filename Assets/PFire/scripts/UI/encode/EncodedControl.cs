using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 * 二维码页面控制 
 * 游戏开始展示二维码
 * 用户连接进入隐藏二维码
 * 用户退出显示二维码
 */
public class EncodedControl : MonoBehaviour {
    RawImage rawImage;
    bool isHidden = false;
    bool isShow = false;
    // Use this for initialization
    private void Awake()
    {
        // 绑定事件
        PServer.OnServerStart += OnServerBegin;
        PServer.OnUserBegin += HiddenEncode;
        PServer.OnUserEnd += ShowEncode;
    }
    void OnServerBegin(object inf)
    {
        SetShowImage(BarcodeCam.GetEncodeByString(inf as string)); // 获取二维并且展示二维码
    }
    
    

    private void Update()
    {
        if(isShow)
        {
            Hashtable args = new Hashtable();
            args.Add("from", 0.0f);
            args.Add("to", 1.0f);
            args.Add("time", 0.5);
            args.Add("easeType", iTween.EaseType.easeInOutExpo);
            args.Add("delay", 4f);

            args.Add("oncomplete", "AnimationEnd");
            args.Add("oncompletetarget", gameObject);
            args.Add("oncompleteparams", false);

            args.Add("onupdate", "AnimationUpdata");
            args.Add("onupdatetarget", gameObject);

            iTween.ValueTo(gameObject, args);
            isShow = false;
        }
        else if(isHidden)
        {
            Hashtable args = new Hashtable();
            args.Add("from", 1.0f);
            args.Add("to", 0.0f);
            args.Add("time", 0.5);
            args.Add("easeType", iTween.EaseType.easeInOutExpo);

            args.Add("onstart", "SetActive");
            args.Add("onstartparams", true);
            args.Add("onstarttarget", gameObject);

            args.Add("onupdate", "AnimationUpdata");
            args.Add("onupdatetarget", gameObject);

            iTween.ValueTo(gameObject, args);
            isHidden = false;
        }
    }
    // 延迟3秒显示二维码
    void ShowEncode(object inf)
    {
        Debug.Log("show");
        isShow = true;
    }
    // 立即过渡隐藏二维码
    void HiddenEncode(object inf)
    {
        Debug.Log("hidden");
        isHidden = true;
    }
    void AnimationUpdata(object par)
    {
        float alph = (float)par;
        GetComponent<CanvasGroup>().alpha = alph;
    }
    void SetActive(bool isactive)
    {
        this.gameObject.SetActive(isactive);
    }

    // 设置二维码展示图片
    public void SetShowImage(Texture2D text)
    {
        if(rawImage==null)
            rawImage = transform.Find("encodeimg").GetComponent<RawImage>();
        rawImage.texture = text;
    }
}
