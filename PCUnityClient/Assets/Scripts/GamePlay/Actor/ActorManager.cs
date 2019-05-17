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
        public float HP;
        public float Power;
    }
    public class ActorManager : IProcess, IAcceptCommand
    {
        // TODO
        List<Actor> _actors = new List<Actor>();
        List<Actor> _deleteActors = new List<Actor>();

        private Actor localPlayer = null;
        private Actor uiRootActor = null;
        private long assignID = 0;
        
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
                        actor = this.BuildEnemyActor(buildData);
                        break;
                    case ActorType.UI:
                        actor = this.BuildUIActor(buildData);
                        this.uiRootActor = actor;
                        break;
            }

            if (actor != null)
            {
                actor.ActorGid = ++assignID;
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
            for (int i = 0; i < _deleteActors.Count; i++)
            {
                this._deleteActors[i].Uninit();
            }
            this._deleteActors.Clear();
            using (List<Actor>.Enumerator item = this._actors.GetEnumerator())
            {
                while (item.MoveNext())
                {
                    if (item.Current != null)
                    {
                        item.Current.Update(deltaTime);
                        if (item.Current.IsNeedRecover)
                        {
                            _deleteActors.Add(item.Current);
                        }
                    }
                }
            }

            for (int i = _deleteActors.Count - 1; i >=0; i--)
            {
                this._actors.Remove(_deleteActors[i]);
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
            var comp = new ActorDataComp(actor);
            comp.Hp = data.HP;
            comp.Power = data.Power;
            actor.InsertActorComponent(ActorComponentType.ActorDataComponent, comp);
            return actor;
        }

        private Actor BuildEnemyActor(ActorBuildData data)
        {
            Actor actor = new Actor();
            actor.InsertActorComponent(ActorComponentType.ActorGameObjectComponent,
                new GameObjectComp(actor, PathDefine.EnemyPrefabPath));
            
            var bornComp = new SetBornWorldPosComp(actor);
            bornComp.TempSetBornPos(data.BornWorldPos);
            actor.InsertActorComponent(ActorComponentType.BornPosSetComponent, bornComp);
            
            var dataComp = new ActorDataComp(actor);
            dataComp.Hp = data.HP;
            dataComp.Power = data.Power;
            actor.InsertActorComponent(ActorComponentType.ActorDataComponent, dataComp);
            if (this.localPlayer != null)
            {
                GameObjectComp goComp =
                    this.localPlayer.GetActorComponent(ActorComponentType.PlayerBehaviorComponent) as GameObjectComp;
                if (goComp != null)
                {
                    actor.InsertActorComponent(ActorComponentType.PlayerBehaviorComponent,
                        new FollowTargetComp(actor, goComp.Target));
                }
                else
                {
                    Debug.LogError("build enemy fail,gocomp is null");
                }
            }
            else
            {
                Debug.LogError("localPlayer is null");
            }
            
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