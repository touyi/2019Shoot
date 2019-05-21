namespace FSM.GameMainStateDef
{
    public class ScoreShowState : BaseState
    {
        public void Enter()
        {
            throw new System.NotImplementedException();
        }

        public void Execute()
        {
            throw new System.NotImplementedException();
        }

        public void Exit()
        {
            throw new System.NotImplementedException();
        }

        public void RegistToFsm(StateMachine<GameMainState, GameMainEvent> fsm)
        {
            fsm.In(GameMainState.ScoreShow)
                .On(GameMainEvent.End).GoTo(GameMainState.WaitScan)
                .Attach(this);

        }
    }
}