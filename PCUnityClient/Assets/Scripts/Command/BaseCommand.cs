namespace Command
{
    public enum CommandType
    {
        KeyDown,
        KeyUp,
    }
    public abstract class BaseCommand
    {
        public readonly CommandType CmdType;

        protected BaseCommand(CommandType type)
        {
            CmdType = type;
        }
    }
}