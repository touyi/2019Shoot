using System.Collections.Generic;
using Wrapper;
using Protocol;
namespace MessageSystem
{
    public class EventParam : Poolable<EventParam>
    {
        public EProtocol type;
        public object message;

        protected override void Uninit()
        {
            message = null;
        }

        protected override void Init()
        {
            message = null;
        }
    }
    public class NetMessage : Singleton<NetMessage>
    {
        public delegate void NetMessageCallBack(EventParam param);

        private Dictionary<int, NetMessageCallBack> _netMessageCallBacks = new Dictionary<int, NetMessageCallBack>();

        public void Init()
        {
        }

        public void LaunchNetMessage(EProtocol type, object message)
        {
            int ntype = (int) type;
            if (this._netMessageCallBacks.ContainsKey(ntype))
            {
                if (this._netMessageCallBacks[ntype] != null)
                {
                    EventParam param = EventParam.Get();
                    param.message = message;
                    param.type = type;
                    this._netMessageCallBacks[ntype].Invoke(param);
                    param.Release();
                }
                
            }
        }

        public void RegistNetListener(EProtocol type, NetMessageCallBack callBack)
        {
            int ntype = (int) type;
            if (_netMessageCallBacks.ContainsKey(ntype))
            {
                //_netMessageCallBacks[ntype] -= callBack;
                _netMessageCallBacks[ntype] += callBack;
            }
            else
            {
                _netMessageCallBacks.Add(ntype, callBack);
            }
        }
        public void RemoveNetListener(EProtocol type, NetMessageCallBack callBack)
        {
            int ntype = (int) type;
            if (_netMessageCallBacks.ContainsKey(ntype))
            {
                _netMessageCallBacks[ntype] -= callBack;
            }
        }
    }
}