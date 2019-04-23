using System;
using Message;
using NetInput;
using Protocol;
using UnityEngine;

namespace Tools
{
    public static class ProtocolHelper
    {
        public static SInput.KeyState TransitionKeyState(Message.KeyState state)
        {
            switch (state)
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

        public static Message.KeyType TransitionKeyType(InputKeyType type)
        {
            switch (type)
            {
                    case InputKeyType.Change:
                        return KeyType.Change;
                    case InputKeyType.Fire:
                        return KeyType.Fire;
                    case InputKeyType.DirVector:
                        
                    default:
                        return KeyType.Fire;
                       
            }
        }

        public static SInput.KeyState GetKeyState(InputKeyType itype)
        {
            Message.KeyType mtype = TransitionKeyType(itype);
            Message.KeyState mstate = ClientSocket.Instance.GetKeyState(mtype);
            return TransitionKeyState(mstate);
        }
    }
}