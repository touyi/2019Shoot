using GamePlay.Actor;
using Wrapper;

namespace GamePlay.Command
{
    public enum CmdType
    {
        InputCmd,
        UIRootCmd,
        AttackCmd,
    }

    public interface IBaseCommand
    {
        CmdType CmdType { get; }
        bool IsUse { get; set; }
    }

    public class InputCmd : Poolable<InputCmd>, IBaseCommand
    {
        public enum ActionType
        {
            Fire,
            StopFire,
        }

        public CmdType CmdType
        {
            get { return CmdType.InputCmd; }
        }

        public bool IsUse { get; set; }
        public ActionType Action_Type;

        protected override void Init()
        {
            IsUse = false;
        }
    }

    public class UICmd : Poolable<UICmd>, IBaseCommand
    {
        public enum UIType
        {
            Root,
        }
        public enum UIState
        {
            Close,
            Open,
        }

        public CmdType CmdType
        {
            get { return CmdType.UIRootCmd; }
        }

        public bool IsUse { get; set; }
        public UIType UiType;
        public UIState UiState;
        public string Info;

        protected override void Init()
        {
            IsUse = false;
            Info = "";
        }
    }
    
    public class AttackCmd : Poolable<AttackCmd>, IBaseCommand
    {
        public CmdType CmdType { get; private set; }
        public bool IsUse { get; set; }
        public IActor SrcActor;
        public IActor DesActor;
        public float Demage;
        protected override void Init()
        {
            base.Init();
            SrcActor = null;
            DesActor = null;
            this.IsUse = false;
            this.CmdType = CmdType.AttackCmd;
        }
    }

    public interface IAcceptCommand
    {
        void AcceptCmd(IBaseCommand cmd);
    }
}