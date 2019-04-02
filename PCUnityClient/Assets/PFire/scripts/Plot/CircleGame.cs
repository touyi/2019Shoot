using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleGame : MonoBehaviour {

    bool isNewGame = false;
    private void Awake()
    {
        PServer.OnUserBegin += OnNewGame;
        PServer.OnUserEnd += InitScence;
    }
    void OnNewGame(object inf)
    {
        isNewGame = true;
    }
    private void Update()
    {
        if(isNewGame)
        {
            FGameManage.Instance.LoadPlotByName("First");
            isNewGame = false;
        }
    }
    public void InitScence(object inf)
    {
        
        FGameObjectBar._instance.enemySpawn.clearPlane();
    }
}
