using System.Net.Configuration;
using GamePlay.Command;
using Message;
using MessageSystem;
using Protocol;
using Wrapper;
using CmdType = Message.CmdType;

namespace FSM.GameMainStateDef
{
    public class WaitScanState : BaseState
    {
        private WeakRef<StateMachine<GameMainState, GameMainEvent>> _fsm =
            new WeakRef<StateMachine<GameMainState, GameMainEvent>>();
        public void Enter()
        {
            NetMessage.Instance.RegistNetListener(EProtocol.NetCmd, GameMessage);
            UICmd cmd = UICmd.Get();
            cmd.UiState = UICmd.UIState.Open;
            cmd.UiType = UICmd.UIType.Root;
            // TODO 服务器下发
            cmd.Info = "http://192.168.31.183:8080";
            GameMain.Instance.CurrentGamePlay.ActorManager.AcceptCmd(cmd);
            cmd.Release();
            
        }

        public void Execute()
        {
            // TODO
        }

        public void Exit()
        {
            NetMessage.Instance.RemoveNetListener(EProtocol.NetCmd, GameMessage);
            UICmd cmd = UICmd.Get();
            cmd.UiState = UICmd.UIState.Close;
            cmd.UiType = UICmd.UIType.Root;
            GameMain.Instance.CurrentGamePlay.ActorManager.AcceptCmd(cmd);
            cmd.Release();
            
        }

        public void RegistToFsm(StateMachine<GameMainState, GameMainEvent> fsm)
        {
            fsm.In(GameMainState.WaitScan)
                .On(GameMainEvent.Begin).GoTo(GameMainState.InGame)
                .Attach(this);
            this._fsm.Ref = fsm;
        }

        private void GameMessage(EventParam param)
        {
            if (param.type == EProtocol.NetCmd)
            {
                CommandList cmdList = param.message as CommandList;
                for (int i = 0; i < cmdList.commandDatas.Count; i++)
                {
                    var cmd = cmdList.commandDatas[i];
                    switch (cmd.ctype)
                    {
                            case CmdType.GameBegin:
                                this._fsm.Ref.Fire(GameMainEvent.Begin);
                                break;
                    }
                }
            }
        }
    }
}