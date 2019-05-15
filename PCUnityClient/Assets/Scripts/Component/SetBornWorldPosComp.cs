using Component.Actor;
using GamePlay.Actor;
using UnityEngine;

namespace Component
{
    public class SetBornWorldPosComp : ActorBaseComponent
    {
        private Vector3 bornPos;
        public SetBornWorldPosComp(IActor actor) : base(actor)
        {
        }

        /// <summary>
        /// 临时使用
        /// </summary>
        /// <param name="worldPos"></param>
        public void TempSetBornPos(Vector3 worldPos)
        {
            bornPos = worldPos;
        }

        public override void Start()
        {
            base.Start();
            var comp = this._actor.Ref.GetActorComponent(ActorComponentType.ActorGameObjectComponent) as GameObjectComp;
            if (comp != null)
            {
                comp.Target.position = bornPos;
            }
        }
    }
}