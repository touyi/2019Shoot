using assets;
using GamePlay.Actor;
using Mono;
using UnityEngine;

namespace Component.Actor
{
    public class GameObjectComp : ActorBaseComponent
    {

        protected Transform target = null;
        protected string prefabPath = null;

        public Transform Target
        {
            get { return target; }
        }

        public GameObjectComp(IActor actor) : base(actor)
        {
        }

        protected override void UninitComponent()
        {
            GameObject.Destroy(this.target.gameObject);
        }

        public GameObjectComp(IActor actor, string path) : base(actor)
        {
            this.SetGameObjPath(path);
        }

        public void SetGameObjPath(string path)
        {
            prefabPath = path;
        }

        public override void Init()
        {
            if (string.IsNullOrEmpty(prefabPath))
            {
                return;
            }
            GameObject prefab = AssetsManager.Instance.LoadPrefab(prefabPath);
            GameObject go = GameObject.Instantiate(prefab);
            if (go != null)
            {
                var info = go.AddComponent<ActorInfoWithGameObject>();
                info.ActorGid = this._actor.Ref.ActorGid;
                this.target = go.transform;
            }
        }
    }
}