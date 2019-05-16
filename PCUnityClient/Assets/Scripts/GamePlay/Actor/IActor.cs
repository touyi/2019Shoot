﻿using Component.Actor;

namespace GamePlay.Actor
{
    public interface IActor
    {
        long ActorGid { get; set; }
        ActorBaseComponent GetActorComponent(ActorComponentType type);
        void InsertActorComponent(ActorComponentType type, ActorBaseComponent actorBaseComponent);
        ActorBaseComponent RemoveActorComponent(ActorComponentType type);
        void Update(float deltaTime);
        void Init();
        void Start();
        void Uninit();
        bool IsNeedRecover { get; set; }
    }
}