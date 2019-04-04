﻿
using GamePlay;
using NetInput;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    private float lastFrameTime = 0f;
    // private MainFSMStarter _mainFsmStarter = new MainFSMStarter();
    public static GameMain Instance = null;

    private NormalGamePlay _currentGamePlay = null;

    public NormalGamePlay CurrentGamePlay
    {
        get { return this._currentGamePlay; }
    }
    

    private void Awake()
    {
        GameMain.Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        //_mainFsmStarter.Init();
        // TODO 异步加载
        this._currentGamePlay = GamePlayBuilder.BuildNormalGamePlay();
        this._currentGamePlay.Init();
        Tools.Timer.Instance.DelayCall(0.03f, this.DelayStart, null);
    }

    private void DelayStart(object param)
    {
        if (this._currentGamePlay != null)
        {
            this._currentGamePlay.Start();
        }
    }
    
    

    private void FixedUpdate()
    {
        float deltaTime = Time.time - lastFrameTime;
        lastFrameTime = Time.time;
        
        CurrentInput.CurInput.Update(deltaTime);
        Tools.Timer.Instance.Update(deltaTime);
        if (CurrentGamePlay != null && CurrentGamePlay.IsRunning)
        {
            CurrentGamePlay.Update(deltaTime);
        }
    }

    private void Update()
    {
    }
}