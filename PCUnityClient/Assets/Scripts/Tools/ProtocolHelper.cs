
using NetInput;
using Protocol;

namespace Tools
{
    public static class ProtocolHelper
    {
        public static SInput.KeyState TransitionKeyState(Message.KeyState sState)
        {
            switch (sState)
            {
                case Message.KeyState.Click:
                    return SInput.KeyState.Down;
                case Message.KeyState.Down:
                    return SInput.KeyState.KeepDown;
                case Message.KeyState.Up:
                    return SInput.KeyState.Up;
                default:
                    return SInput.KeyState.Up;
            }
        }
        
        public static Message.KeyState TransitionKeyStateU(SInput.KeyState uState)
        {
            switch (uState)
            {
                case SInput.KeyState.Down:
                    return Message.KeyState.Click;
                case SInput.KeyState.KeepDown:
                    return Message.KeyState.Down;
                case SInput.KeyState.Up:
                    return Message.KeyState.Up;
                default:
                    return Message.KeyState.Up;
            }
        }

        public static Message.KeyType TransitionKeyType(InputKeyType uType)
        {
            switch (uType)
            {
                    case InputKeyType.Change:
                        return Message.KeyType.Change;
                    case InputKeyType.Fire:
                        return Message.KeyType.Fire;
                    case InputKeyType.DirVector:
                        
                    default:
                        return Message.KeyType.Fire;
                       
            }
        }
        
        public static InputKeyType TransitionKeyTypeU(Message.KeyType sType)
        {
            switch (sType)
            {
                case Message.KeyType.Change:
                    return InputKeyType.Change;
                case Message.KeyType.Fire:
                    return InputKeyType.Fire;        
                default:
                    return InputKeyType.DirVector;
                       
            }
        }

//        public static SInput.KeyState GetKeyState(InputKeyType itype)
//        {
//            Message.KeyType mtype = TransitionKeyType(itype);
//            Message.KeyState mstate = ClientSocket.Instance.GetKeyState(mtype);
//            return TransitionKeyState(mstate);
//        }
    }
}