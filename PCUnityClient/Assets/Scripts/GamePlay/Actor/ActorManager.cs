using System.Collections.Generic;
using System.ComponentModel.Design;
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
        public float MaxHp;
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

        public List<Actor> Actors
        {
            get { return _actors; }
        }

        public Actor LocalPlayer
        {
            get { return this.localPlayer; }
        }
        

        public long CreateActor(ActorBuildData buildData)
        {
            Actor actor = null;
            
            switch (buildData.type)
            {
                    case ActorType.LocalPlayer:
                    case ActorType.EmptyLocalPlayer:
                        if (this.localPlayer != null)
                        {
                            // 暂时不支持多个localplayer
                            return -1;
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
                actor.ActorType = buildData.type;
                actor.Init();
                actor.Start();
                this._actors.Add(actor);
                GameMain.Instance.CurrentGamePlay.Dispathcer.LaunchEvent(GameEventDefine.ActorCreated, actor.ActorGid);
                return actor.ActorGid;
            }

            return -2;

        }

        public Actor GetActorByGid(long actorGid)
        {
            for (int i = 0; i < this._actors.Count; i++)
            {
                if (this._actors[i].ActorGid == actorGid)
                {
                    return this._actors[i];
                }
            }

            return null;
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
                GameMain.Instance.CurrentGamePlay.Dispathcer.LaunchEvent(GameEventDefine.ActorDestory,
                    _deleteActors[i].ActorGid);
            }
            
            for (int i = 0; i < _deleteActors.Count; i++)
            {
                this.RemoveInner(this._deleteActors[i]);
            }
            this._deleteActors.Clear();
        }

        public void DestoryActorByType(ActorType type)
        {
            for (int i = this._actors.Count - 1; i >= 0; i--)
            {
                if (this._actors[i].ActorType == type)
                {
                    this._actors[i].IsNeedRecover = true;
                }
            }
        }

        public void DestoryActorByGid(long actorGid)
        {
            for (int i = 0; i < this._actors.Count; i++)
            {
                if (this._actors[i].ActorGid == actorGid)
                {
                    this._actors[i].IsNeedRecover = true;
                }
            }
        }

        public void DestoryActorImmediately(long actorGid, bool isSendEvent)
        {
            for (int i = 0; i < this._actors.Count; i++)
            {
                if (this._actors[i].ActorGid == actorGid)
                {
                    var actor = this._actors[i];
                    this._actors.RemoveAt(i);
                    if (isSendEvent)
                    {
                        GameMain.Instance.CurrentGamePlay.Dispathcer.LaunchEvent(GameEventDefine.ActorDestory,
                            actorGid);
                    }
                    this.RemoveInner(actor);
                }
            }
        }

        public void DestoryActorImmediately(Actor actor, bool isSendEvent)
        {
            if (actor == null)
            {
                return;
            }
            this.DestoryActorImmediately(actor.ActorGid, isSendEvent);
        }

        private void RemoveInner(Actor actor)
        {
            if (actor == this.localPlayer)
            {
                this.localPlayer = null;
            }

            if (actor == this.uiRootActor)
            {
                this.uiRootActor = null;
            }
            actor.Uninit();
        }

        public void Uninit()
        {
            // TODO
        }

        public bool IsEnemy(long actorGid)
        {
            for (int i = 0; i < this._actors.Count; i++)
            {
                if (this._actors[i].ActorGid == actorGid && this._actors[i].ActorType == ActorType.Enemy)
                {
                    return true;
                }
            }
            
            for (int i = 0; i < this._deleteActors.Count; i++)
            {
                if (this._deleteActors[i].ActorGid == actorGid && this._deleteActors[i].ActorType == ActorType.Enemy)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 这个应该放在工厂里面 这里临时做法 TODO
        /// </summary>
        /// <param name="worldPos"></param>
        /// <returns></returns>
        private Actor BuildLocalPlayer(ActorBuildData data)
        {
            Actor actor = new Actor();
            if (data.type != ActorType.EmptyLocalPlayer)
            {
                actor.InsertActorComponent(ActorComponentType.PlayerBehaviorComponent, new LocalPlayerBehaviorComp(actor));
                actor.InsertActorComponent(ActorComponentType.WeapenComponent, new WeapenComp(actor));
                var comp = new ActorDataComp(actor);
                comp.SetValue(data.HP,data.MaxHp,data.Power);
                actor.InsertActorComponent(ActorComponentType.ActorDataComponent, comp);
            }
            else
            {
                actor.InsertActorComponent(ActorComponentType.ActorGameObjectComponent,
                    new GameObjectComp(actor, PathDefine.LocalPlayerPath));
                
                SetBornWorldPosComp comp = new SetBornWorldPosComp(actor);
                comp.TempSetBornPos(GameMain.Instance.CurrentGamePlay.LevelManager.GetLocalPlayerPos());
                actor.InsertActorComponent(ActorComponentType.BornPosSetComponent, comp);
            }
            
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
            dataComp.CurrentHp = data.HP;
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
            if (cmd.CmdType == CmdType.UICmd)
            {
                UICmd uicmd = cmd as UICmd;
                if (uicmd != null && this.uiRootActor == null)
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