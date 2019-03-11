namespace FSM
{
    public interface BaseState : IState
    {
        /// <summary>
        /// 将状态注册到FSM当中去
        /// </summary>
        /// <param name="fsm"></param>
        void RegistToFsm(StateMachine<GameMainState, GameMainEvent> fsm);
    }

    
}