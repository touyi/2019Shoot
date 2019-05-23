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
            this.RegisterGlobalState(new GuideState());
            this.RegisterGlobalState(new ScoreShowState());
            // TODO 测试用 初始化为 InGameState
            _mainFsm.Initialize(GameMainState.WaitScan);
            
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
            _mainFsm.Execute(deltaTime);
        }

        private void RegisterGlobalState(BaseState state)
        {
            state.RegistToFsm(this._mainFsm);
        }
    }
}