namespace FSM.GameMainStateDef
{
    public class WaitScanState : BaseState
    {
        public void Enter()
        {
            // TODO
        }

        public void Execute()
        {
            // TODO
        }

        public void Exit()
        {
            // TODO
        }

        public void RegistToFsm(StateMachine<GameMainState, GameMainEvent> fsm)
        {
            fsm.In(GameMainState.WaitScan)
                .On(GameMainEvent.Begin).GoTo(GameMainState.InGame)
                .Attach(this);
        }
    }
}