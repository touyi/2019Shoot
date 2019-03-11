using System.Collections.Generic;
using Component.Actor;
using UnityEngine;

namespace GamePlay.Actor
{
    public class Actor : IActor
    {
        public Dictionary<ActorComponentType, ActorBaseComponent> components =
            new Dictionary<ActorComponentType, ActorBaseComponent>();
        public ActorBaseComponent GetActorComponent(ActorComponentType type)
        {
            ActorBaseComponent baseComponent = null;
            if (this.components.TryGetValue(type, out baseComponent))
            {
                return baseComponent;
            }

            return null;
        }

        public void InsertActorComponent(ActorComponentType type, ActorBaseComponent actorBaseComponent)
        {
            if (!this.components.ContainsKey(type) && actorBaseComponent != null) 
            {
                this.components.Add(type, actorBaseComponent);
            }
        }

        public ActorBaseComponent RemoveActorComponent(ActorComponentType type)
        {
            ActorBaseComponent baseComponent = null;
            if (this.components.TryGetValue(type, out baseComponent))
            {
                this.components.Remove(type);
                return baseComponent;
            }

            return null;
        }
    }
}