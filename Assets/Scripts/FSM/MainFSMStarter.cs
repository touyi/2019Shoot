namespace FSM
{
    public class MainFSMStarter
    {
        private readonly StateMachine<GameMainState, GameMainEvent> _mainFsm =
            new StateMachine<GameMainState, GameMainEvent>();
        public void Init()
        {
            // 在此处初始化游戏主状态机
            
            
            _mainFsm.Initialize(GameMainState.WaitScan);
            _mainFsm.Start();
        }

        public void Uninit()
        {
            _mainFsm.Stop();
        }
    }
}