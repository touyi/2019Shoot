using assets;
using Component.Actor;
using Tools;
using UnityEngine;

namespace Component.Widget
{
    public interface IGun
    {
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
            GameObject prefab = AssetsManager.Instance.LoadPrefab("Gun/linegun");
            if (prefab != null)
            {
                GameObject go = GameObject.Instantiate(prefab, parent);
                this._gun = go.transform;
                
                this._gun.localPosition = Vector3.zero;
                this._gun.rotation = Quaternion.identity;
                
            }
            // 激光束
            GameObject line = AssetsManager.Instance.LoadPrefab("bullet/plasma_beam_blue");
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

            }
        }

        public void Uninit()
        {
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