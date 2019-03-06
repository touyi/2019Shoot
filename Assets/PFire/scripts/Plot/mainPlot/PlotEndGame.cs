using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotEndGame : FPlot
{


    PScreenControl screen; 

    public override void PlotEnd()
    {
    }

    public override void PlotStart()
    {
        FGameObjectBar._instance.PCScreen.GetComponent<PCScreen>().HiddenScreen();

        //screen = FGameObjectBar._instance.InfoScreen.GetComponent<PScreenControl>();
        //screen.LoadText("/text/task/die.txt");
        //screen.Show();
        GameObject.Find("Info").GetComponent<ScoreControl>().ShowBord();
    }

    public override void PlotUpdate()
    {
    }
}
