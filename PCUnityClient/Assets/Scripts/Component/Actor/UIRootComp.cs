using assets;
using Component.Widget;
using GamePlay.Actor;
using GamePlay.Command;
using Tools;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using ZXing.Common.Detector;

namespace Component.Actor
{
    public class UIRootComp:ActorBaseComponent,IAcceptCommand
    {
        private RectTransform uIRoot = null;
        // private RawImage encode = null;
        
        public UIRootComp(IActor actor) : base(actor)
        {
        }

        public override void Init()
        {
            GameObject go = AssetsManager.Instance.LoadPrefab(PathDefine.UIRootPath);
            GameObject root = GameObject.Instantiate(go);
            uIRoot = root.GetComponent<RectTransform>();
            //encode = uIRoot.CustomGetComponent<RawImage>("WaitScan/encodeimg");
            
        }

        public override void Uninit()
        {
            GameObject.Destroy(uIRoot.gameObject);
            base.Uninit();
        }

        public void AddChildGameObject(RectTransform trans)
        {
            trans.SetParent(this.uIRoot);
            trans.localScale = Vector3.one;
            trans.anchorMin = Vector2.zero;
            trans.anchorMax = Vector2.one;
            trans.offsetMin = Vector2.zero;
            trans.offsetMax = Vector2.zero;
        }

        public void AcceptCmd(IBaseCommand cmd)
        {
            if(cmd.IsUse)return;
            UICmd uicmd = cmd as UICmd;
            if (uicmd == null)
            {
                return;
            }
            
            if (uicmd.UiState == UICmd.UIState.Close)
            {
                for (int i = 0; i < this._widgets.Count; i++)
                {
                    IAcceptCommand acceptCommand = this._widgets[i] as IAcceptCommand;
                    if (acceptCommand == null)
                    {
                        continue;
                    }
                    acceptCommand.AcceptCmd(cmd);
                }
            }
            else
            {
                switch (uicmd.UiType)
                {
                    case UICmd.UIType.EncodeUI:
                        EncodeUI encodeUI = new EncodeUI();
                        this.AddWidget(encodeUI);
                        encodeUI.AcceptCmd(cmd);
                        break;
                    case UICmd.UIType.HPUI:
                        BottomHPUI hpUI = new BottomHPUI();
                        this.AddWidget(hpUI);
                        hpUI.AcceptCmd(cmd);
                        break;
                    case UICmd.UIType.RadarUI:
                        RadarUI radarUi = new RadarUI();
                        this.AddWidget(radarUi);
                        radarUi.AcceptCmd(cmd);
                        break;
                }
            }
            cmd.IsUse = true;
        }
    }
}