using FSM;
using GamePlay;
using UnityEditor;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    private float lastFrameTime = 0f;
    // private MainFSMStarter _mainFsmStarter = new MainFSMStarter();
    public static GameMain Instance = null;

    private IGamePlay _currentGamePlay = null;

    public IGamePlay CurrentGamePlay
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
        this._currentGamePlay.Start();
    }

    private void FixedUpdate()
    {
        float deltaTime = Time.time - lastFrameTime;
        lastFrameTime = Time.time;
        // _mainFsmStarter.Update(lastFrameTime);
        if (CurrentGamePlay != null)
        {
            CurrentGamePlay.Update(deltaTime);
        }

        
    }
}
