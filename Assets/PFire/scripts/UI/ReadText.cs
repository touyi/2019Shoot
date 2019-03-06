using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;


public class ReadText : MonoBehaviour {

    public Image showImg;
    StreamReader sreader;
    public Sprite[] taskSprite;
    int nowId;
    Text text;
    void Start () {
        //sreader = new StreamReader(Application.streamingAssetsPath + "/text/task01.txt",Encoding.Default);
        text = GetComponent<Text>();
	}
    public bool NextText()
    {
        if (nowId >= taskSprite.Length)
            return false;
        showImg.sprite = taskSprite[nowId];
        nowId++;
        return true;

        //if(sreader.EndOfStream)
        //{
        //    return false;
        //}
        //if(text==null)
        //{
        //    text = GetComponent<Text>();
        //}
        //text.text = sreader.ReadLine();
        //return true;
    }
	public void LoadText(string name)
    {
        nowId = 0;
        NextText();
        // 取消文本读取 改用img静态图片

        //if(sreader!=null)
        //{
        //    sreader.Close();
        //}
        //sreader = new StreamReader(Application.streamingAssetsPath + name, Encoding.Default);
        //NextText();
    }
	
}
