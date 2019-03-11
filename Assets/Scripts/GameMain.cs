using FSM;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    private float lastFrameTime = 0f;
    private MainFSMStarter _mainFsmStarter = new MainFSMStarter();
    private void Awake()
    {
    }

    private void Start()
    {
        _mainFsmStarter.Init();
    }

    private void FixedUpdate()
    {
        float deltaTime = Time.time - lastFrameTime;
        lastFrameTime = Time.time;
        _mainFsmStarter.Update(lastFrameTime);
    }
}
