using System.Collections.Generic;
using Component.Actor;
using UnityEditor.Purchasing;
using UnityEngine;

namespace GamePlay.Actor
{
    public enum ActorType
    {
        LocalPlayer,
        Enemy,
    }
    
    public class Actor : IActor
    {
        public ActorType ActorType;
        private Dictionary<ActorComponentType, ActorBaseComponent> components =
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
                actorBaseComponent.Init();
            }
        }

        public ActorBaseComponent RemoveActorComponent(ActorComponentType type)
        {
            ActorBaseComponent baseComponent = null;
            if (this.components.TryGetValue(type, out baseComponent))
            {
                this.components.Remove(type);
                baseComponent.Uninit();
                return baseComponent;
            }

            return null;
        }

        public void Update(float deltaTime)
        {
            using (Dictionary<ActorComponentType, ActorBaseComponent>.Enumerator item = this.components.GetEnumerator())
            {
                while (item.MoveNext())
                {
                    item.Current.Value.Update(deltaTime);
                }
            }
        }
    }
}