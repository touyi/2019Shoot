using System.Xml.XPath;
using assets;
using Component.Actor;
using GamePlay.Command;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Component.Widget
{
    public class ScoreUI : IWidget, IAcceptCommand
    {
        private RectTransform score = null;
        private ActorBaseComponent _baseComponent = null;
        public void Init(ActorBaseComponent parentComp)
        {
            GameObject prefab =  AssetsManager.Instance.LoadPrefab(StringDefine.ScoreUIPath);
            GameObject go = GameObject.Instantiate(prefab);
            this.score = go.GetComponent<RectTransform>();
            UIRootComp comp = parentComp as UIRootComp;
            comp.AddChildGameObject(this.score);
            this._baseComponent = comp;
            Text scoreText = this.score.CustomGetComponent<Text>("Panel/Text");
            if (scoreText != null)
            {
                int cur = 0;
                int max = 0;
                GameMain.Instance.CurrentGamePlay.DataProvider.GetIntData(StringDefine.CurrentScoreKey, out cur);
                GameMain.Instance.CurrentGamePlay.DataProvider.GetIntData(StringDefine.MaxScoreKey, out max);
                if (max < cur)
                {
                    max = cur;
                    GameMain.Instance.CurrentGamePlay.DataProvider.SetIntData(StringDefine.MaxScoreKey, max);
                }

                scoreText.text = string.Format("你的分数：{0} 历史最高分：{1}", cur, max);
            }
        }

        public void Uninit()
        {
            if (this.score)
            {
                GameObject.Destroy(this.score.gameObject);
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

            if (uiCmd.UiType != UICmd.UIType.ScoreUI)
            {
                return;
            }

            this.score.CustomSetActive(uiCmd.UiState == UICmd.UIState.Open);

            if (uiCmd.UiState == UICmd.UIState.Close)
            {
                this._baseComponent.RemoveWidget(this);
            }

            cmd.IsUse = true;
        }
    }
}