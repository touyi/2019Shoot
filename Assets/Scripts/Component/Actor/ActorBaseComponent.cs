using GamePlay.Actor;
using UnityEngine;
using Wrapper;

namespace Component.Actor
{
    public enum ActorComponentType
    {
        PlayerMoveComponent,
        AttackComponent,
    }
    public class ActorBaseComponent : IBaseComponent
    {
        private WeakRef<IActor> _actor = new WeakRef<IActor>();

        #region 子类关注
        public void InitTargetActor(IActor actor)
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

        public virtual void Init()
        {
            
        }

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