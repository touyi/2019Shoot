using System.Collections.Generic;
using assets;
using Component;
using Component.Actor;
using UnityEngine;

namespace GamePlay.Actor
{
    public class ActorBuildData
    {
        public ActorType type;
        public Vector3 BornWorldPos;
    }
    public class ActorManager
    {
        // TODO
        List<Actor> _actors = new List<Actor>();

        private Actor localPlayer = null;

        public Actor CreateActor(ActorBuildData buildData)
        {
            Actor actor = null;
            
            switch (buildData.type)
            {
                    case ActorType.LocalPlayer:
                        actor = this.BuildLocalPlayer(buildData.BornWorldPos);
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

        public void Update(float deltaTime)
        {
            
        }

        public Actor LocalPlayer
        {
            get { return this.localPlayer; }
        }

        private Actor BuildLocalPlayer(Vector3 worldPos)
        {
            Actor actor = new Actor();
            // TODO组装
//            actor.InsertActorComponent();
            return null;
        }
    }
}