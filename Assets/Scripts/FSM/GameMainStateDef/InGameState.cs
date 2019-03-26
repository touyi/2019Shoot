using System.Collections;
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
            ActorBuildData data = ActorBuildData.Get();
            data.BornWorldPos = gamePlay.LevelManager.GetLocalPlayerPos(0);
            data.type = ActorType.LocalPlayer;
            Actor actor = actorManager.CreateActor(data);
            data.Release();
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