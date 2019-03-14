using FSM;
using GamePlay;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    private float lastFrameTime = 0f;
    // private MainFSMStarter _mainFsmStarter = new MainFSMStarter();
    public static GameMain Instance = null;

    private NormalGamePlay _gamePlay = null;

    public IGamePlay CurrentGamePlay
    {
        get { return this._gamePlay; }
    }

    private void Awake()
    {
        GameMain.Instance = this;
    }

    private void Start()
    {
        //_mainFsmStarter.Init();
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
