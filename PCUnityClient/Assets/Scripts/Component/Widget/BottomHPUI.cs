using assets;
using Component.Actor;
using GamePlay;
using GamePlay.Command;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Component.Widget
{
    public class BottomHPUI : IWidget, IAcceptCommand
    {
        private UIRootComp _parentComp = null;
        private RectTransform root = null;
        private Slider _slider = null;
        public void Init(ActorBaseComponent parentComp)
        {
            this._parentComp = parentComp as UIRootComp;
            
            GameObject prefab = AssetsManager.Instance.LoadPrefab(StringDefine.BottomHPPath);
            if (prefab != null && this._parentComp != null)
            {
                GameObject go = GameObject.Instantiate(prefab);
                root = go.GetComponent<RectTransform>();
                this._parentComp.AddChildGameObject(root);
                this._slider = this.root.CustomGetComponent<Slider>("HealthySlider");
            }
            else
            {
                Debug.LogError("BottomHPUI Init error");
            }

            GameMain.Instance.CurrentGamePlay.Dispathcer.RegistListener(GameEventDefine.ActorLifeChange,
                this.OnActorLifeChange);
        }

        public void OnActorLifeChange(EventData data)
        {
            GamePlay.Actor.Actor actor = GameMain.Instance.CurrentGamePlay.ActorManager.LocalPlayer;
            if (actor != null && data.longPara == actor.ActorGid && this._slider != null)
            {
                ActorDataComp comp = actor.ActorData();
                if (comp != null)
                {
                    float value = comp.CurrentHp / comp.MaxHp;
                    this._slider.value = value;
                }
                
            }
        }

        public void Uninit()
        {
            GameMain.Instance.CurrentGamePlay.Dispathcer.RemoveListener(GameEventDefine.ActorLifeChange,
                this.OnActorLifeChange);
            if (this.root)
            {
                GameObject.Destroy(this.root.gameObject);
            }
        }

        public void Update(float deltaTime)
        {
            
        }

        public void AcceptCmd(IBaseCommand cmd)
        {
            
            if (cmd.IsUse)
            {
                return;
            }
            UICmd uiCmd = cmd as UICmd;

            if (uiCmd == null || uiCmd.UiType != UICmd.UIType.HPUI) return;
            if (uiCmd.UiState == UICmd.UIState.Close)
            {
                this._parentComp.RemoveWidget(this);
            }
            else
            {
                this.root.CustomSetActive(true);
            }
            cmd.IsUse = true;
        }
    }
}