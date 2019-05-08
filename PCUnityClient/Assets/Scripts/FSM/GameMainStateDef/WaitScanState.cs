using GamePlay.Command;

namespace FSM.GameMainStateDef
{
    public class WaitScanState : BaseState
    {
        public void Enter()
        {
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
            // TODO
        }

        public void RegistToFsm(StateMachine<GameMainState, GameMainEvent> fsm)
        {
            fsm.In(GameMainState.WaitScan)
                .On(GameMainEvent.Begin).GoTo(GameMainState.InGame)
                .Attach(this);
        }
    }
}