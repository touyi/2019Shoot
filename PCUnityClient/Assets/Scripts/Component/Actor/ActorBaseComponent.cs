using GamePlay.Actor;
using UnityEngine;
using Wrapper;

namespace Component.Actor
{
    public enum ActorComponentType
    {
        PlayerBehaviorComponent,
        WeapenComponent,
        AttackComponent,
        ActorGameObjectComponent,
        BornPosSetComponent,
        ActorInfoComponent,
        UIRootComponent,
    }
    public class ActorBaseComponent : IBaseComponent
    {
        protected WeakRef<IActor> _actor = new WeakRef<IActor>();

        public bool Enable = true;

        public ActorBaseComponent(IActor actor)
        {
            if (actor != null)
            {
                this._actor.Ref = actor;
            }
            else
            {
                Debug.LogError("actor is null !");
                return;
            }
        }

        #region 子类关注

        /// <summary>
        /// 做自己数据的初始化 尽量避免获取其他组件的数据
        /// </summary>
        public virtual void Init()
        {
            
        }

        /// <summary>
        /// 所有组件已经初始化完成 可以获取其他组件数据
        /// </summary>
        public virtual void Start()
        {
        }

        public virtual void Uninit()
        {
        }

        public virtual void Update(float deltaTime)
        {
        }
        #endregion
    }
}