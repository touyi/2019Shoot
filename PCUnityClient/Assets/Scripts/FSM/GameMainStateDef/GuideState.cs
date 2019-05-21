using System.Runtime.CompilerServices;
using GamePlay;
using GamePlay.Actor;
using UnityEngine;
using Wrapper;

namespace FSM.GameMainStateDef
{
    public class GuideState : BaseState
    {
        private long localPlayerGid = 0;
        private WeakRef<StateMachine<GameMainState, GameMainEvent>> _fsm =
            new WeakRef<StateMachine<GameMainState, GameMainEvent>>();
        public void Enter()
        {
            Debug.Log("Enter:" + this.GetType().ToString());
            NormalGamePlay gamePlay = GameMain.Instance.CurrentGamePlay;
            ActorManager actorManager = gamePlay.ActorManager;
            ActorBuildData data = ActorBuildData.Get();
            data.BornWorldPos = gamePlay.LevelManager.GetLocalPlayerPos(0);
            data.type = ActorType.EmptyLocalPlayer;
            this.localPlayerGid = actorManager.CreateActor(data);
            data.Release();
            
            gamePlay.Dispathcer.RegistListener(GameEventDefine.GameBegin, this.OnGameGo);
        }

        public void Execute()
        {
        }

        public void Exit()
        {
            GameMain.Instance.CurrentGamePlay.ActorManager.DestoryActorImmediately(this.localPlayerGid, false);
        }

        public void RegistToFsm(StateMachine<GameMainState, GameMainEvent> fsm)
        {
            fsm.In(GameMainState.Guide)
                .On(GameMainEvent.GO).GoTo(GameMainState.InGame)
                .On(GameMainEvent.End).GoTo(GameMainState.WaitScan)
                .Attach(this);
            this._fsm.Ref = fsm;
        }

        private void OnGameGo(EventData data)
        {
            this._fsm.Ref.Fire(GameMainEvent.GO);
        }
    }
}