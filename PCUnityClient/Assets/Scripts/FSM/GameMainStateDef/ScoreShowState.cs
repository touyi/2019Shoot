namespace FSM.GameMainStateDef
{
    public class ScoreShowState : BaseState
    {
        public void Enter()
        {
        }

        public void Execute()
        {
        }

        public void Exit()
        {
        }

        public void RegistToFsm(StateMachine<GameMainState, GameMainEvent> fsm)
        {
            fsm.In(GameMainState.ScoreShow)
                .On(GameMainEvent.End).GoTo(GameMainState.WaitScan)
                .Attach(this);

        }
    }
}