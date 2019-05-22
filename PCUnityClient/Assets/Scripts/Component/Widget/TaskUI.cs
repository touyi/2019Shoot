using assets;
using Component.Actor;
using GamePlay.Command;
using Tools;
using UnityEngine;

namespace Component.Widget
{
    public class TaskUI : IWidget, IAcceptCommand
    {
        private RectTransform task = null;
        private ActorBaseComponent _baseComponent = null;
        public void Init(ActorBaseComponent parentComp)
        {
            GameObject prefab = AssetsManager.Instance.LoadPrefab(StringDefine.TaskUIPath);
            GameObject go = GameObject.Instantiate(prefab);
            this.task = go.GetComponent<RectTransform>();
            UIRootComp comp = parentComp as UIRootComp;
            comp.AddChildGameObject(this.task);
            _baseComponent = parentComp;
        }

        public void Uninit()
        {
            if (this.task)
            {
                GameObject.Destroy(this.task.gameObject);
            }
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

            if (uiCmd.UiType != UICmd.UIType.TaskUI)
            {
                return;
            }

            this.task.CustomSetActive(uiCmd.UiState == UICmd.UIState.Open);

            if (uiCmd.UiState == UICmd.UIState.Close)
            {
                this._baseComponent.RemoveWidget(this);
            }

            cmd.IsUse = true;
        }
    }
}