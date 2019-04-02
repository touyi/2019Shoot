using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 战斗场景游戏信息
 */
public class FGameInfo {
    public enum InfoChangeType
    {
        PlayerHealthy,Score
    }

    public delegate void OnListener(InfoChangeType type);
    public delegate void OnListener2();
    public event OnListener OnInfoChange;
    public event OnListener2 OnPlayerDead;
    static FGameInfo _instance = null;
    private float playerHealthy;
    private bool isUserConnect;
    public float maxPlayerHealthy;
    int nowScore = 0;

    public static FGameInfo Instance
    {
        get
        {
            if (_instance == null)
                _instance = new FGameInfo();
            return _instance;
        }
    }

    public float PlayerHealthy
    {
        get
        {
            return playerHealthy;
        }

        set
        {
            playerHealthy = Mathf.Min(value, maxPlayerHealthy);
            if(OnInfoChange!=null)
                OnInfoChange(InfoChangeType.PlayerHealthy);
            if(OnPlayerDead!=null && playerHealthy <= 0)
            {
                OnPlayerDead();
            }
        }
    }

    public int NowScore
    {
        get
        {
            return nowScore;
        }

        set
        {
            nowScore = value;
            PlayerPrefs.SetInt("maxScore", Mathf.Max(nowScore, MaxScore));
            if (OnInfoChange != null)
                OnInfoChange(InfoChangeType.Score);
        }
    }

    public int MaxScore
    {
        get
        {
            if(PlayerPrefs.HasKey("maxScore"))
                return PlayerPrefs.GetInt("maxScore");
            else
            {
                PlayerPrefs.SetInt("maxScore", nowScore);
                return nowScore;
            }
        }
    }

    public bool IsUserConnect
    {
        get
        {
            return isUserConnect;
        }

        set
        {
            isUserConnect = value;
        }
    }
}
