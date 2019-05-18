using System.Collections.Generic;
using GamePlay.Command;
using Message;
using MessageSystem;
using Protocol;
using CmdType = Message.CmdType;

namespace GamePlay
{
    public class NormalDispatcher : IDispathcer
    {
        Dictionary<int, GameEventCallBack> _eventCallBacks = new Dictionary<int, GameEventCallBack>();
        public void Init()
        {
            NetMessage.Instance.RegistNetListener(EProtocol.NetCmd, this.OnNetMessage);
        }

        public void Start()
        {
        }

        public void Uninit()
        {
            NetMessage.Instance.RemoveNetListener(EProtocol.NetCmd, this.OnNetMessage);
        }

        public void Update(float deltaTime)
        {
        }

        private void OnNetMessage(EventParam param)
        {
            if (param.type != EProtocol.NetCmd) return;
            CommandList list = param.message as CommandList;
            for (int i = 0; i < list.commandDatas.Count; i++)
            {
                Message.Command cmd = list.commandDatas[i];
                switch (cmd.ctype)
                {
                        case CmdType.GameBegin:
                            this.LaunchEvent(GameEventDefine.GameBegin, null);
                            break;
                        case CmdType.GameEnd:
                            this.LaunchEvent(GameEventDefine.GameEnd, null);
                            break;
                }
            }
        }

        public void RegistListener(GameEventDefine eventId, GameEventCallBack callBack)
        {
            int id = (int) eventId;
            if (this._eventCallBacks.ContainsKey(id))
            {
                this._eventCallBacks[id] += callBack;
            }
            else
            {
                this._eventCallBacks.Add(id, callBack);
            }
        }

        public bool RemoveListener(GameEventDefine eventId, GameEventCallBack callBack)
        {
            int id = (int)eventId;
            if (this._eventCallBacks.ContainsKey(id))
            {
                this._eventCallBacks[id] -= callBack;
                if (this._eventCallBacks[id] == null)
                {
                    this._eventCallBacks.Remove(id);
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// 瞬时同步发送
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="data"></param>
        public void LaunchEvent(GameEventDefine eventId, EventData data)
        {
            int id = (int) eventId;
            GameEventCallBack callback = null;
            if (this._eventCallBacks.TryGetValue(id, out callback))
            {
                if(callback != null)
                {
                    callback.Invoke(data);
                }
            }
        }
    }
}