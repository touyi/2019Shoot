using FSM.GameMainStateDef;

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
            
            _mainFsm.Initialize(GameMainState.WaitScan);
            _mainFsm.Start();
        }

        public void Uninit()
        {
            _mainFsm.Stop();
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