using assets;
using Component.Actor;
using GamePlay.Command;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Component.Widget
{
    public class EncodeUI : IWidget, IAcceptCommand
    {
        private RectTransform encode = null;
        private RawImage encodeImg = null;
        private UIRootComp _parentComp = null;
        public void Init(ActorBaseComponent parentComp)
        {
            GameObject go = AssetsManager.Instance.LoadPrefab(PathDefine.EncodeUIPath);
            GameObject root = GameObject.Instantiate(go);
            encode = root.GetComponent<RectTransform>();
            encodeImg = encode.CustomGetComponent<RawImage>("encodeimg");
            _parentComp = parentComp as UIRootComp;
            _parentComp.AddChildGameObject(this.encode);
        }

        public void Uninit()
        {
            if(this.encode!=null)GameObject.Destroy(encode.gameObject);
            this.encode = null;
            this.encodeImg = null;
            this._parentComp = null;
        }

        public void Update(float deltaTime)
        {
        }

        public void AcceptCmd(IBaseCommand cmd)
        {
            UICmd uiCmd = cmd as UICmd;
            if (uiCmd == null)
            {
                return;
            }

            if (uiCmd.UiType != UICmd.UIType.EncodeUI)
            {
                return;
            }

            this.encode.CustomSetActive(uiCmd.UiState == UICmd.UIState.Open);
            if (!string.IsNullOrEmpty(uiCmd.Info) && encodeImg != null)
            {
                encodeImg.texture = BarcodeCam.GetEncodeByString(uiCmd.Info);
            }

            if (uiCmd.UiState == UICmd.UIState.Close)
            {
                this._parentComp.RemoveWidget(this);
            }

            cmd.IsUse = true;
        }
    }
}