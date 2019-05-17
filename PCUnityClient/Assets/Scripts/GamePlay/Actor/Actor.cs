using System.Collections.Generic;
using Component.Actor;
using GamePlay.Command;
using UnityEditor.Purchasing;
using UnityEngine;

namespace GamePlay.Actor
{
    
    
    public class Actor : IActor, IAcceptCommand
    {
        private bool isStart = false;
        private Dictionary<ActorComponentType, ActorBaseComponent> components =
            new Dictionary<ActorComponentType, ActorBaseComponent>();

        public ActorType ActorType { get; set; }
        public long ActorGid { get; set; }

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
                if (isStart)
                {
                    actorBaseComponent.Init();
                    actorBaseComponent.Start();
                }
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

        public void Init()
        {
            this.IsNeedRecover = false;
            using (Dictionary<ActorComponentType, ActorBaseComponent>.Enumerator item = this.components.GetEnumerator())
            {
                while (item.MoveNext())
                {
                    item.Current.Value.Init();
                }
            }
        }

        public void Start()
        {
            using (Dictionary<ActorComponentType, ActorBaseComponent>.Enumerator item = this.components.GetEnumerator())
            {
                while (item.MoveNext())
                {
                    item.Current.Value.Start();
                }
            }

            isStart = true;
        }

        public void Uninit()
        {
            using (Dictionary<ActorComponentType, ActorBaseComponent>.Enumerator item = this.components.GetEnumerator())
            {
                while (item.MoveNext())
                {
                    item.Current.Value.Uninit();
                }
            }

            isStart = false;
        }

        public bool IsNeedRecover { get; set; }

        public void AcceptCmd(IBaseCommand cmd)
        {
            if (cmd.IsUse)
            {
                return;
            }
            using (Dictionary<ActorComponentType, ActorBaseComponent>.Enumerator item = this.components.GetEnumerator())
            {
                while (item.MoveNext())
                {
                    IAcceptCommand iAcceptCommand = item.Current.Value as IAcceptCommand;
                    if (iAcceptCommand == null)
                    {
                        continue;
                    }

                    iAcceptCommand.AcceptCmd(cmd);
                }
            }
        }
    }
}