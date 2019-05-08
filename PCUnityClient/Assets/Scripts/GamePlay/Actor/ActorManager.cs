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
        private Actor uiRootActor = null;
        
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
                    case ActorType.UI:
                        actor = this.BuildUIActor(buildData);
                        this.uiRootActor = actor;
                        break;
            }

            if (actor != null)
            {
                actor.Init();
                actor.Start();
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
            return actor;
        }

        private Actor BuildUIActor(ActorBuildData data)
        {
            // TODO next
            Actor actor = new Actor();
            actor.InsertActorComponent(ActorComponentType.UIRootComponent, new UIRootComp(actor));
            return actor;
        }

        public void AcceptCmd(IBaseCommand cmd)
        {
            if (cmd.CmdType == CmdType.UIRootCmd)
            {
                UICmd uicmd = cmd as UICmd;
                if (uicmd != null && uicmd.UiType == UICmd.UIType.Root && this.uiRootActor == null)
                {
                    var data = ActorBuildData.Get();
                    data.type = ActorType.UI;
                    data.BornWorldPos = Vector3.zero;
                    this.CreateActor(data);
                    data.Release();
                }
            }
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