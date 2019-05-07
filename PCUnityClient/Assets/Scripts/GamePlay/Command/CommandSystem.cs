using Wrapper;

namespace GamePlay.Command
{
    public enum CmdType
    {
        InputCmd,
    }

    public interface IBaseCommand
    {
        CmdType CmdType { get; set; }
        bool IsUse { get; set; }
    }

    public class InputCmd : Poolable<InputCmd>, IBaseCommand
    {
        public enum ActionType
        {
            Fire,
            StopFire,
        }
        public CmdType CmdType { get; set; }
        public bool IsUse { get; set; }
        public ActionType Action_Type;

        protected override void Init()
        {
            IsUse = false;
        }
    }

    public interface IAcceptCommand
    {
        void AcceptCmd(IBaseCommand cmd);
    }
}