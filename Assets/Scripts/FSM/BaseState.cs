namespace FSM
{
    public interface BaseState : IState
    {
        void RegistToFsm(StateMachine<GameMainState, GameMainEvent> fsm);
    }

    
}