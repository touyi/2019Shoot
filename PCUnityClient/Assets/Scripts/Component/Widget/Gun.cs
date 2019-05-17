using assets;
using Component.Actor;
using Tools;
using UnityEngine;

namespace Component.Widget
{
    public interface IGun
    {
        event WeapenComp.AttackCallBack OnAttackActor;
        void Fire();
        void StopFire();
        void Init(Transform parent);
        void Uninit();
        void Update(float deltaTime);
        bool Enable { get; set; }
    }

    public class LineGun : IGun
    {
        private bool enable = false;
        private Transform _gun = null;
        private blueline _bulelineControl;
        public event WeapenComp.AttackCallBack OnAttackActor;

        public void Fire()
        {
            this._bulelineControl.transform.CustomSetActive(true);
        }

        public void StopFire()
        {
            this._bulelineControl.transform.CustomSetActive(false);
        }

        public void Init(Transform parent)
        {
            // 激光器
            GameObject prefab = AssetsManager.Instance.LoadPrefab(PathDefine.LineGunPath);
            if (prefab != null)
            {
                GameObject go = GameObject.Instantiate(prefab, parent);
                this._gun = go.transform;
                
                this._gun.localPosition = Vector3.zero;
                this._gun.rotation = Quaternion.identity;
                
            }
            // 激光束
            GameObject line = AssetsManager.Instance.LoadPrefab(PathDefine.BluLinePath);
            if (line != null)
            {
                Transform dir = this._gun.CustomFind("dir");
                GameObject go = GameObject.Instantiate(line, dir);
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                this._bulelineControl = go.GetComponent<blueline>();
                if (!this._bulelineControl)
                {
                    this._bulelineControl = go.AddComponent<blueline>();
                }
                this._bulelineControl.OnAttackActor += OnAttackActorInner;
            }
        }

        private void OnAttackActorInner(long actorGid)
        {
            if (this.OnAttackActor != null)
            {
                this.OnAttackActor.Invoke(actorGid);
            }
        }

        public void Uninit()
        {
            GameObject.Destroy(this._bulelineControl.gameObject);
            GameObject.Destroy(this._gun.gameObject);
            this._bulelineControl.OnAttackActor -= OnAttackActorInner;
        }

        public void Update(float deltaTime)
        {
            if (!enable)
            {
                return;
            }
        }

        public bool Enable {
            get { return enable; }
            set
            {
                // 激活武器
                enable = value;
            }
        }
    }
}