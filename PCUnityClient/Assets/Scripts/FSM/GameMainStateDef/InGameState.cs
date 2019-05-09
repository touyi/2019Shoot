using System.Collections;
using GamePlay;
using GamePlay.Actor;
using GamePlay.Command;
using Message;
using MessageSystem;
using NetInput;
using Protocol;
using UnityEngine;
using Wrapper;
using CmdType = Message.CmdType;

namespace FSM.GameMainStateDef
{
    // TODO
    public class InGameState : BaseState
    {
        private bool lastFireState = false;

        private WeakRef<StateMachine<GameMainState, GameMainEvent>> _fsm =
            new WeakRef<StateMachine<GameMainState, GameMainEvent>>();
        public void Enter()
        {
            NetMessage.Instance.RegistNetListener(EProtocol.NetCmd, this.GameNetMessage);
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
            bool isFire = CurrentInput.CurInput.GetKey(InputKeyType.Fire);
            if (isFire && (lastFireState != isFire))
            {
                InputCmd cmd = InputCmd.Get();
                cmd.Action_Type = InputCmd.ActionType.Fire;
                GameMain.Instance.CurrentGamePlay.ActorManager.AcceptCmd(cmd);
                cmd.Release();
            }
            else if (!isFire && lastFireState != isFire)
            {
                InputCmd cmd = InputCmd.Get();
                cmd.Action_Type = InputCmd.ActionType.StopFire;
                GameMain.Instance.CurrentGamePlay.ActorManager.AcceptCmd(cmd);
            }
            lastFireState = isFire;
        }

        public void Exit()
        {
            NetMessage.Instance.RemoveNetListener(EProtocol.NetCmd, this.GameNetMessage);
        }

        public void RegistToFsm(StateMachine<GameMainState, GameMainEvent> fsm)
        {
            fsm.In(GameMainState.InGame)
                .On(GameMainEvent.End).GoTo(GameMainState.WaitScan)
                .Attach(this);

            this._fsm.Ref = fsm;
        }

        private void GameNetMessage(EventParam param)
        {
            if (param.type == EProtocol.NetCmd)
            {
                CommandList cmdList = param.message as CommandList;
                for (int i = 0; i < cmdList.commandDatas.Count; i++)
                {
                    var cmd = cmdList.commandDatas[i];
                    switch (cmd.ctype)
                    {
                        case CmdType.GameEnd:
                            this._fsm.Ref.Fire(GameMainEvent.End);
                            break;
                    }
                }
            }
        }
    }
}