using GamePlay.Actor;
using UnityEngine;

namespace Component.Actor
{
    public static class ActorHelp
    {
        public static ActorDataComp ActorData(this IActor actor)
        {
            if (actor == null)
            {
                return null;
            }

            var data = actor.GetActorComponent(ActorComponentType.ActorDataComponent) as ActorDataComp;
            return data;
        }

        public static GameObjectComp ActorObjectComp(this IActor actor)
        {
            if (actor.ActorType == ActorType.LocalPlayer)
            {
                return actor.GetActorComponent(ActorComponentType.PlayerBehaviorComponent) as GameObjectComp;
            }
            else
            {
                return actor.GetActorComponent(ActorComponentType.ActorGameObjectComponent) as GameObjectComp;
            }
        }
    }
}