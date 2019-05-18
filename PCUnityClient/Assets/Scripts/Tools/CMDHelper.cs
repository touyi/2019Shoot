using GamePlay.Command;

namespace Tools
{
    public static class CMDHelper
    {
        public static void AcceptUICmdToActorManager(UICmd.UIType uiType, UICmd.UIState uiState, string info = null)
        {
            UICmd cmd = UICmd.Get();
            cmd.UiState = uiState;
            cmd.UiType = uiType;
            cmd.Info = info;
            GameMain.Instance.CurrentGamePlay.ActorManager.AcceptCmd(cmd);
            cmd.Release();
        }
    }
}