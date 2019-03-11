using GamePlay.Actor;
using UnityEngine;

namespace Component.Actor
{
    public enum ActorComponentType
    {
        MoveComponent,
        AttackComponent,
    }
    public class ActorBaseComponent : IBaseComponent
    {
        private IActor _actor;

        #region 子类关注
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

        public ActorBaseComponent(IActor actor)
        {
            if (actor != null)
            {
                this._actor = actor;
            }
            else
            {
                Debug.LogError("actor is null !");
            }
        }
    }
}