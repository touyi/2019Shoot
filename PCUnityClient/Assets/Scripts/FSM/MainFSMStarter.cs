﻿using FSM.GameMainStateDef;

namespace FSM
{
    public class MainFSMStarter
    {
        private readonly StateMachine<GameMainState, GameMainEvent> _mainFsm =
            new StateMachine<GameMainState, GameMainEvent>();
        public void Init()
        {
            // 在此处初始化游戏主状态机
            this.RegisterGlobalState(new WaitScanState());
            this.RegisterGlobalState(new InGameState());
            // TODO 测试用 初始化为 InGameState
            _mainFsm.Initialize(GameMainState.InGame);
            
        }

        public void Uninit()
        {
            _mainFsm.Stop();
        }

        public void Start()
        {
            _mainFsm.Start();
        }

        public void Update(float deltaTime)
        {
            _mainFsm.Execute();
        }

        private void RegisterGlobalState(BaseState state)
        {
            state.RegistToFsm(this._mainFsm);
        }
    }
}