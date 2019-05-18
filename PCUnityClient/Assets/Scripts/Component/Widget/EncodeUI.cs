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
        public void Init(ActorBaseComponent parentComp)
        {
            GameObject go = AssetsManager.Instance.LoadPrefab(PathDefine.EncodeUIPath);
            GameObject root = GameObject.Instantiate(go);
            encode = root.GetComponent<RectTransform>();
            encodeImg = encode.CustomGetComponent<RawImage>("encodeimg");
            UIRootComp comp = parentComp as UIRootComp;
            comp.AddChildGameObject(this.encode);
        }

        public void Uninit()
        {
            if(this.encode!=null)GameObject.Destroy(encode.gameObject);
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

            cmd.IsUse = true;
        }
    }
}