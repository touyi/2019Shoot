using System.Globalization;
using assets;
using GamePlay.Actor;
using NetInput;
using Tools;
using UnityEngine;
using Wrapper;

namespace Component.Actor
{
    public class LocalPlayerBehaviorComp : ActorBaseComponent
    {
        private Transform _targetTrans = null;
        private Transform _weapen = null;
        public LocalPlayerBehaviorComp(IActor actor) : base(actor)
        {
        }

        public Transform WeapenTrans
        {
            get { return _weapen; }
        }

        public override void Init()
        {
            GameObject prefab = AssetsManager.Instance.LoadPrefab("Actor/KeyBordMainPlayer");
            if (prefab != null)
            {
                Transform parent = GameMain.Instance.CurrentGamePlay.LevelManager.GetLocalPlayerBase(0);
                GameObject go = GameObject.Instantiate(prefab, parent);
                this._targetTrans = go.transform;
                if (this._targetTrans)
                {
                    this._targetTrans.localPosition = Vector3.zero;
                    this._targetTrans.rotation = Quaternion.identity;
                }
            }

            this._weapen = this._targetTrans.CustomFind("weapenPos");
        }

        public override void Start()
        {
        }

        public override void Uninit()
        {
        } 

        public override void Update(float deltaTime)
        {
            UpdateInput();
        }

        private void UpdateInput()
        {
            if (CurrentInput.CurInput == null)
            {
                return;
            }

            if (this._weapen)
            {
                Vector3 dir = CurrentInput.CurInput.GetAxis3D(InputKeyCode.DirVector);
                this._weapen.LookAt(this._weapen.position + dir);
            }
            

        }
    }
}