using System.Collections.Generic;
using UnityEngine;
using Wrapper;

namespace Tools
{
    /// <summary>
    /// 定时器
    /// </summary>
    public class Timer : Singleton<Timer>
    {
        private long _idGenerator = 0;
        private Dictionary<long, FunctionParam> _functions = new Dictionary<long, FunctionParam>();
        private List<long> removeList = new List<long>();
        private class FunctionParam : Poolable<FunctionParam>
        {
            public DelayCallFuncton Functon;
            public object Param;
            public float CallTime;

            protected override void Init()
            {
                this.CallTime = -1;
                this.Param = null;
                this.Functon = null;
            }

            protected override void Uninit()
            {
                this.Init();
            }
        }
        public delegate void DelayCallFuncton(object param);

        public void Update(float deltaTime)
        {
            removeList.Clear();
            using (Dictionary<long, FunctionParam>.Enumerator item = this._functions.GetEnumerator())
            {
                while (item.MoveNext())
                {
                    if (item.Current.Value.CallTime <= Time.time)
                    {
                        removeList.Add(item.Current.Key);
                        FunctionParam fp = item.Current.Value;
                        fp.Functon.Invoke(fp.Param);
                    }
                }
            }

            using (List<long>.Enumerator item = removeList.GetEnumerator())
            {
                while (item.MoveNext())
                {
                    this._functions.Remove(item.Current);
                }
            }
        }

        public long DelayCall(float delayTime, DelayCallFuncton function, object param)
        {
            if (delayTime <= 0)
            {
                function.Invoke(param);
                return -1;
            }
            long callId = ++_idGenerator;
            FunctionParam fparam = FunctionParam.Get();
            fparam.CallTime = delayTime + Time.time;
            fparam.Functon = function;
            fparam.Param = param;
            this._functions.Add(callId, fparam);
            return callId;
        }

        public void RemoveCall(long callId)
        {
            if (this._functions.ContainsKey(callId))
            {
                this._functions.Remove(callId);
            }
        }
    }
}