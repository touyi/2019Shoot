﻿using Component.Actor;

namespace GamePlay.Actor
{
    public interface IActor
    {
        ActorBaseComponent GetActorComponent(ActorComponentType type);
        void InsertActorComponent(ActorComponentType type, ActorBaseComponent actorBaseComponent);
        ActorBaseComponent RemoveActorComponent(ActorComponentType type);
    }
}