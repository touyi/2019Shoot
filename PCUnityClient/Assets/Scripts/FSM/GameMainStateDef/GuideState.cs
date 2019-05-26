using System.Runtime.CompilerServices;
using GamePlay;
using GamePlay.Actor;
using GamePlay.Command;
using NetInput;
using Tools;
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
            
            gamePlay.Dispathcer.RegistListener(GameEventDefine.GameEnd, this.OnGameEnd);
            CMDHelper.AcceptUICmdToActorManager(UICmd.UIType.TaskUI, UICmd.UIState.Open);
        }

        public void Execute(float deltaTime)
        {
            if (CurrentInput.CurInput.GetKey(InputKeyType.Fire))
            {
                this._fsm.Ref.Fire(GameMainEvent.Go);
            }
        }

        public void Exit()
        {
            CMDHelper.AcceptUICmdToActorManager(UICmd.UIType.TaskUI, UICmd.UIState.Close);
            GameMain.Instance.CurrentGamePlay.ActorManager.DestoryActorByGid(this.localPlayerGid);
        }

        public void RegistToFsm(StateMachine<GameMainState, GameMainEvent> fsm)
        {
            fsm.In(GameMainState.Guide)
                .On(GameMainEvent.Go).GoTo(GameMainState.InGame)
                .On(GameMainEvent.End).GoTo(GameMainState.WaitScan)
                .Attach(this);
            this._fsm.Ref = fsm;
        }

        private void OnGameEnd(EventData data)
        {
            this._fsm.Ref.Fire(GameMainEvent.End);
        }
        
        
    }
}