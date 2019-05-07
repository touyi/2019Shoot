using System.Collections.Generic;
using assets;
using Component;
using Component.Actor;
using GamePlay.Command;
using Tools;
using UnityEngine;
using Wrapper;

namespace GamePlay.Actor
{
    public class ActorBuildData : Poolable<ActorBuildData>
    {
        public ActorType type;
        public Vector3 BornWorldPos;
    }
    public class ActorManager : IProcess, IAcceptCommand
    {
        // TODO
        List<Actor> _actors = new List<Actor>();

        private Actor localPlayer = null;
        
        public Actor LocalPlayer
        {
            get { return this.localPlayer; }
        }

        public Actor CreateActor(ActorBuildData buildData)
        {
            Actor actor = null;
            
            switch (buildData.type)
            {
                    case ActorType.LocalPlayer:
                        if (this.localPlayer != null)
                        {
                            // 暂时不支持多个localplayer
                            return null;
                        }
                        actor = this.BuildLocalPlayer(buildData);
                        this.localPlayer = actor;
                        break;
                    case ActorType.Enemy:
                        break;
            }

            if (actor != null)
            {
                this._actors.Add(actor);
            }

            return actor;
        }

        public void Init()
        {
        }

        public void Start()
        {
        }

        public void Update(float deltaTime)
        {
            using (List<Actor>.Enumerator item = this._actors.GetEnumerator())
            {
                while (item.MoveNext())
                {
                    if (item.Current != null)
                    {
                        item.Current.Update(deltaTime);
                    }
                }
            }
        }

        public void Uninit()
        {
        }

        

        /// <summary>
        /// 这个应该放在工厂里面 这里临时做法 TODO
        /// </summary>
        /// <param name="worldPos"></param>
        /// <returns></returns>
        private Actor BuildLocalPlayer(ActorBuildData data)
        {
            Actor actor = new Actor();
            // TODO组装
            actor.InsertActorComponent(ActorComponentType.PlayerBehaviorComponent, new LocalPlayerBehaviorComp(actor));
            actor.InsertActorComponent(ActorComponentType.WeapenComponent, new WeapenComp(actor));
            actor.Init();
            actor.Start();
            return actor;
        }

        public void AcceptCmd(IBaseCommand cmd)
        {
            if (cmd.IsUse) return;
            for (int i = 0; i < this._actors.Count; i++)
            {
                this._actors[i].AcceptCmd(cmd);
                if (cmd.IsUse)
                {
                    break;
                }
            }
        }
    }
}