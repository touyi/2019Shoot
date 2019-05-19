using System.Collections.Generic;
using assets;
using Component.Actor;
using GamePlay;
using GamePlay.Actor;
using GamePlay.Command;
using GamePool.OBJ;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Component.Widget
{
    public class RadarUI : IWidget, IAcceptCommand
    {
        private const float ScaleRate = 4;
        private const float MaxRange = 60;
        
        private RectTransform root;
        private RectTransform radarPad;
        private Dictionary<long, RectTransform> actorPoints = new Dictionary<long, RectTransform>();
        public void Init(ActorBaseComponent parentComp)
        {
            GameObject prefab = AssetsManager.Instance.LoadPrefab(PathDefine.RadarUIPath);
            GameObject go = GameObject.Instantiate(prefab);
            this.root = go.GetComponent<RectTransform>();
            this.radarPad = this.root.CustomFind("Radar/Rader") as RectTransform;
            UIRootComp comp = parentComp as UIRootComp;
            comp.AddChildGameObject(root);
            GameMain.Instance.CurrentGamePlay.Dispathcer.RegistListener(GameEventDefine.ActorCreated,
                this.OnActorCreated);
            GameMain.Instance.CurrentGamePlay.Dispathcer.RegistListener(GameEventDefine.ActorDestory,
                this.OnActorDestory);
            GPGameObjectPool.ReFormPoolObject<GPRadarPoint>(10);
        }

        public void Uninit()
        {
            GameMain.Instance.CurrentGamePlay.Dispathcer.RemoveListener(GameEventDefine.ActorCreated,
                this.OnActorCreated);
            GameMain.Instance.CurrentGamePlay.Dispathcer.RemoveListener(GameEventDefine.ActorDestory,
                this.OnActorDestory);
            using (Dictionary<long, RectTransform>.Enumerator item = this.actorPoints.GetEnumerator())
            {
                while (item.MoveNext())
                {
                    GPGameObjectPool.Return(item.Current.Value.gameObject);
                }
            }
            GPGameObjectPool.ReFormPoolObject<GPRadarPoint>(0);
            this.radarPad = null;
            if (this.root)
            {
                GameObject.Destroy(root.gameObject);
            }
        }

        public void Update(float deltaTime)
        {
            using (Dictionary<long, RectTransform>.Enumerator item = this.actorPoints.GetEnumerator())
            {
                while (item.MoveNext())
                {
                    long actorGid = item.Current.Key;
                    RectTransform point = item.Current.Value;
                    this.CaculateRadarPos(actorGid, point);
                }
            }
        }

        private void CaculateRadarPos(long targetGid, RectTransform targetPoint)
        {
            GamePlay.Actor.Actor actor = GameMain.Instance.CurrentGamePlay.ActorManager.GetActorByGid(targetGid);
            if (actor == null)
            {
                return;
            }

            GameObjectComp targetComp = actor.ActorObjectComp();
            GameObjectComp srcComp = GameMain.Instance.CurrentGamePlay.ActorManager.LocalPlayer.ActorObjectComp();
            if (targetPoint == null || srcComp == null)
            {
                return;
            }

            Vector3 relaVec = srcComp.Target.worldToLocalMatrix.MultiplyPoint(targetComp.Target.position);
            Vector2 rela2D = new Vector2(relaVec.x, relaVec.z) / ScaleRate;
            if (Vector2.Distance(rela2D, Vector2.zero) > MaxRange)
            {
                targetPoint.CustomSetActive(false);
            }
            else
            {
                targetPoint.CustomSetActive(true);
                targetPoint.anchoredPosition = rela2D;
            }
            

        }

        public void AcceptCmd(IBaseCommand cmd)
        {
        }

        private void OnActorCreated(EventData data)
        {
            GamePlay.Actor.Actor actor = GameMain.Instance.CurrentGamePlay.ActorManager.GetActorByGid(data.longPara);
            if (actor.ActorType == ActorType.Enemy && !this.actorPoints.ContainsKey(actor.ActorGid))
            {
                // Bug FixUpdateq驱动的GameObject可能延迟导致渲染出错
                GameObject point = GPGameObjectPool.Creat<GPRadarPoint>(Vector3.zero, Quaternion.identity);
                Image img = point.GetComponent<Image>();
                img.color = Color.red;
                point.transform.CustomSetParent(this.radarPad);
                RectTransform rectTrans = point.GetComponent<RectTransform>();
                if (rectTrans != null)
                {
                    this.actorPoints.Add(data.longPara, point.GetComponent<RectTransform>());
                }
                else
                {
                    Debug.LogError("radarui OnActorCreated");
                }
                
            }
        }

        private void OnActorDestory(EventData data)
        {
            RectTransform point = null;
            if (this.actorPoints.TryGetValue(data.longPara, out point))
            {
                this.actorPoints.Remove(data.longPara);
                GPGameObjectPool.Return(point.gameObject);
            }
        }
    }
}