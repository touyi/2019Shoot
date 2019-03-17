using GamePlay;
using GamePlay.Actor;
using UnityEngine;

namespace FSM.GameMainStateDef
{
    // TODO
    public class InGameState : BaseState
    {
        public void Enter()
        {
            // TODO 初始化玩家
            NormalGamePlay gamePlay = GameMain.Instance.CurrentGamePlay as NormalGamePlay;
            if (gamePlay == null)
            {
                Debug.Log("gameplay is null");
                return;
            }

            ActorManager actorManager = gamePlay.ActorManager;
            // TODO actorManager.CreateActor();
        }

        public void Execute()
        {
        }

        public void Exit()
        {
        }

        public void RegistToFsm(StateMachine<GameMainState, GameMainEvent> fsm)
        {
            fsm.In(GameMainState.InGame)
                .On(GameMainEvent.End).GoTo(GameMainState.WaitScan)
                .Attach(this);
        }
    }
}