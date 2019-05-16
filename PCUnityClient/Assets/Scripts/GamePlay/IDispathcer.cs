using System;
using Component;
using MessageSystem;
using Wrapper;

namespace GamePlay
{
    public enum GameEventDefine
    {
        None = 0,
        GameBegin = 1,
        GameEnd = 2,
        ActorLifeChange = 3,
        Count = 4,
    }

    public class EventData : Poolable<EventData>
    {
        public int intPara;
        public long longPara;
        public string strPara;
        public float floatPara;
        public object objectPara;
        protected override void Init()
        {
        }

        protected override void Uninit()
        {
        }
    }

    public delegate void GameEventCallBack(EventData data);
    public interface IDispathcer : IBaseComponent
    {
        void RegistListener(GameEventDefine eventId, GameEventCallBack callBack);
        bool RemoveListener(GameEventDefine eventId, GameEventCallBack callBack);
        void LaunchEvent(GameEventDefine eventId, EventData data);
    }
}