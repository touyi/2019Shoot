using System.Net.Configuration;
using GamePlay;
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
            GameMain.Instance.CurrentGamePlay.Dispathcer.RegistListener(GameEventDefine.GameBegin, this.OnGameBegin);
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
            GameMain.Instance.CurrentGamePlay.Dispathcer.RemoveListener(GameEventDefine.GameBegin, this.OnGameBegin);
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

        private void OnGameBegin(EventData param)
        {
            this._fsm.Ref.Fire(GameMainEvent.Begin);
//            if (param.type == EProtocol.NetCmd)
//            {
//                CommandList cmdList = param.message as CommandList;
//                for (int i = 0; i < cmdList.commandDatas.Count; i++)
//                {
//                    var cmd = cmdList.commandDatas[i];
//                    switch (cmd.ctype)
//                    {
//                            case CmdType.GameBegin:
//                                
//                                break;
//                    }
//                }
//            }
        }
    }
}