using System.Collections.Generic;

namespace GamePlay
{
    public class NormalDispatcher : IDispathcer
    {
        Dictionary<int, GameEventCallBack> _eventCallBacks = new Dictionary<int, GameEventCallBack>();
        public void Init()
        {
        }

        public void Start()
        {
        }

        public void Uninit()
        {
        }

        public void Update(float deltaTime)
        {
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
    }
}