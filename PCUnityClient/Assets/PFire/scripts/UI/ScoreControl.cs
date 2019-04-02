using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 * 分数显示控制模块
 */
public class ScoreControl : MonoBehaviour {

    public Text nowScore;
    public Text ScoreBord;
    public GameObject bord;
    private void Awake()
    {
        FGameInfo.Instance.OnInfoChange += UpdateScore;
    }
    void UpdateScore(FGameInfo.InfoChangeType type)
    {
        if(type==FGameInfo.InfoChangeType.Score)
        {
            nowScore.text = "score:" + FGameInfo.Instance.NowScore;
            ScoreBord.text = "你的分数：" + FGameInfo.Instance.NowScore + " 历史最高分：" + FGameInfo.Instance.MaxScore; 
        }
    }
    public void ShowBord()
    {
        MITween.Tween(bord, new Vector3(1, 1, 1), 0.4f,true,iTween.EaseType.easeInOutBack);
    }
    public void HiddenBord()
    {
        MITween.Tween(bord, new Vector3(0, 0, 0), 0.4f, true,iTween.EaseType.easeInBack);
    }
}
