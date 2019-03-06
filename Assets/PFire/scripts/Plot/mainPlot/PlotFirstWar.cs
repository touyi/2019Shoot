using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
/*
 * 剧情一
 */
public class PlotFirstWar : FPlot {

    PScreenControl screen;
    PCScreen pcScreen;
    EnemySpawn eSpawn;
    float deathSpeed = 1.0f;
    
    enum FirstState
    {
        Talk,Fire,End
    };
    FirstState nowState = FirstState.Talk;
    public override void PlotEnd()
    {
        eSpawn.clearPlane();
    }

    public override void PlotStart()
    {
        #region 赋值
        
        pcScreen = FGameObjectBar._instance.PCScreen.GetComponent<PCScreen>();
        screen = FGameObjectBar._instance.InfoScreen.GetComponent<PScreenControl>();
        eSpawn = FGameObjectBar._instance.enemySpawn;
        #endregion

        #region 初始化剧情
        GameObject.Find("Info").GetComponent<ScoreControl>().HiddenBord();
        FGameInfo.Instance.NowScore = 0;
        nowState = FirstState.Talk;
        screen.LoadText("/text/task/task01.txt");
        pcScreen.HiddenScreen();
        screen.Show();
        FGameInfo.Instance.maxPlayerHealthy = 100;
        FGameInfo.Instance.PlayerHealthy = 100;
        //FGameObjectBar._instance.fireGun.GetComponent<FireBullet>().MaxBulletCount = 60;
        FGameObjectBar._instance.lineGun.GetComponent<bulelineControl>().MaxEnergy = 100f;
        isEnd = false;
        #endregion
        Timer._instance.Unregist("readscreen");
        Timer._instance.regist("readscreen", 3);
    }

    public override void PlotUpdate()
    {
        switch(nowState)
        {
            case FirstState.Talk:
                ScreenTalk();
                break;
            case FirstState.Fire:
                Defend();
                break;
            case FirstState.End:
                End();
                break;
        }
    }
    
    void End()
    {
        FGameManage.Instance.LoadPlotByName("End");
        /*if (PInput.Sure())
        {
            if(!screen.NextText())
            {
                // 结束信息展示完毕
                // TODO
            }
        }*/
    }
    void Defend()
    {
        if(eSpawn.isAllWaveEnd())
        {
            // 本关已经战斗部分结束
            // 显示战斗胜利信息
            nowState = FirstState.End;
            screen.Show();
            screen.LoadText("/text/task/end1.txt");
            pcScreen.HiddenScreen();
            return;
        }
        FGameInfo.Instance.PlayerHealthy -= Time.deltaTime * deathSpeed;
    }
    void ScreenTalk()
    {
        if(PInput.Sure() || (Timer._instance.regist("readscreen",3) && FGameInfo.Instance.IsUserConnect))
        {
            if(!screen.NextText())
            {
                nowState = FirstState.Fire;
                screen.Hidden();
                pcScreen.ShowScreen();
                eSpawn.SetNumIncrease(2);
            }
        }
    }
    
}
