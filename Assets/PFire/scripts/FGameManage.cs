using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 战斗场景游戏主逻辑
 * 剧情控制加载
 */
public class FGameManage : MonoBehaviour {

    static FGameManage _instance = null;
    public delegate void OnAppQuit();

    public static FGameManage Instance
    {
        get
        {
            return _instance;
        }
    }
    FPlot m_nowPlot = null;
    Dictionary<string, FPlot> plotdic = new Dictionary<string, FPlot>();

    void Awake()
    {
        _instance = this;
        if(Encryption.IsRIghtDevices()==false)
        {
            Debug.Log("noright");
            Application.Quit();
        }
    }
    void Start()
    {
        plotdic.Add("First", new PlotFirstWar());
        plotdic.Add("End", new PlotEndGame());
        
        FGameInfo.Instance.OnPlayerDead += PlayerDead;
        LoadPlotByName("First");

    }
    void Update()
    {
        if(m_nowPlot!=null)
        {
            m_nowPlot.PlotUpdate();
        }
    }
    public void LoadPlotByName(string name)
    {
        if(m_nowPlot!=null)
        {
            m_nowPlot.PlotEnd();
        }
        if(plotdic.TryGetValue(name, out m_nowPlot))
        {
            m_nowPlot.PlotStart();
        }
    }
    void PlayerDead()
    {
        LoadPlotByName("End");
    }
    private void OnApplicationQuit()
    {
        
    }
}
