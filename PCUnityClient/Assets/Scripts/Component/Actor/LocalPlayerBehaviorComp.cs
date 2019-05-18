using System.Globalization;
using assets;
using GamePlay.Actor;
using NetInput;
using Tools;
using UnityEditor.Rendering;
using UnityEngine;
using Wrapper;

namespace Component.Actor
{
    public class LocalPlayerBehaviorComp : GameObjectComp
    {
        private Transform _targetTrans = null;
        private Transform _weapen = null;
        public Camera camera = null;
        public LocalPlayerBehaviorComp(IActor actor) : base(actor)
        {
        }

        public Transform WeapenTrans
        {
            get { return _weapen; }
        }

        public override void Init()
        {
            this.SetGameObjPath(PathDefine.LocalPlayerPath);
            base.Init();
            Transform parent = GameMain.Instance.CurrentGamePlay.LevelManager.GetLocalPlayerBase(0);
            this._targetTrans = this.target;
            this._targetTrans.SetParent(parent);
            if (this._targetTrans)
            {
                this._targetTrans.localPosition = Vector3.zero;
                this._targetTrans.rotation = Quaternion.identity;
            }

            this.camera = this.target.GetComponent<Camera>();

            this._weapen = this._targetTrans.CustomFind("weapenPos");
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            UpdateInput();
        }

        public override void Uninit()
        {
            this.camera = null;
            this._weapen = null;
            this._targetTrans = null;
            base.Uninit();
        }

        private void UpdateInput()
        {
            if (CurrentInput.CurInput == null)
            {
                return;
            }

            if (this._weapen)
            {
                Vector3 screenPos = CurrentInput.CurInput.GetAxis3D(InputKeyType.DirVector);
                if (this.camera != null)
                {
                    Vector3 worldPos = this.camera.ScreenToWorldPoint(screenPos);
                    this._weapen.LookAt(worldPos);
                }
                else
                {
                    Debug.LogError("camera null");
                }
                
            }
            

        }
    }
}