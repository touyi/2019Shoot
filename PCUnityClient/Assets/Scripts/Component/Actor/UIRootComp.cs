using assets;
using GamePlay.Actor;
using GamePlay.Command;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Component.Actor
{
    public class UIRootComp:ActorBaseComponent,IAcceptCommand
    {
        private RectTransform uIRoot = null;
        private RawImage encode = null;
        public UIRootComp(IActor actor) : base(actor)
        {
        }

        public override void Init()
        {
            GameObject go = AssetsManager.Instance.LoadPrefab("UI/UIRoot");
            GameObject root = GameObject.Instantiate(go);
            uIRoot = root.GetComponent<RectTransform>();
            encode = uIRoot.CustomGetComponent<RawImage>("WaitScan/encodeimg");
        }


        public void AcceptCmd(IBaseCommand cmd)
        {
            if (cmd.CmdType == CmdType.UIRootCmd)
            {
                UICmd uicmd = cmd as UICmd;
                if (uicmd != null)
                {
                    uIRoot.CustomSetActive(uicmd.UiState == UICmd.UIState.Open);
                    if (!string.IsNullOrEmpty(uicmd.Info) && encode != null)
                    {
                        encode.texture = BarcodeCam.GetEncodeByString(uicmd.Info);
                    }
                }

                
                cmd.IsUse = true;
            }
        }
    }
}