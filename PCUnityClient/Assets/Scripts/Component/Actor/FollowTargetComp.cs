using System;
using GamePlay.Actor;
using GamePlay.Command;
using UnityEngine;

namespace Component.Actor
{
    public class FollowTargetComp : ActorBaseComponent
    {
        private Transform followTarget = null;
        private Transform navTarget = null;
        private const float Damping = 1f;
        private const float FlySpeed = 25.6f;
        private const float AttackDistance = 10;
        public FollowTargetComp(IActor actor, Transform target) : base(actor)
        {
            this.SetTarget(target);
        }

        public override void Uninit()
        {
            followTarget = null;
            navTarget = null;
            base.Uninit();
        }

        private void SetTarget(Transform target)
        {
            this.followTarget = target;
        }

        public override void Start()
        {
            GameObjectComp goComp =
                this._actor.Ref.GetActorComponent(ActorComponentType.ActorGameObjectComponent) as GameObjectComp;
            if (goComp != null)
            {
                this.navTarget = goComp.Target;
            }
            else
            {
                Debug.LogError("goComp.Target is null");
            }
            
        }

        public override void Update(float deltaTime)
        {
            if (this.navTarget != null && this.followTarget != null) // 平滑转向
            {
                Quaternion rota = Quaternion.LookRotation(followTarget.position - this.navTarget.position);
                this.navTarget.rotation = Quaternion.Slerp(this.navTarget.rotation, rota, deltaTime * Damping);
                this.navTarget.position += this.navTarget.forward * FlySpeed * deltaTime;
                if (Vector3.Distance(this.navTarget.position, followTarget.position) < AttackDistance)
                {
                    this.ProcessAttack();
                }
            }
            
        }

        private void ProcessAttack()
        {
            var data = this._actor.Ref.GetActorComponent(ActorComponentType.ActorDataComponent) as ActorDataComp;
            if (data != null)
            {
                AttackCmd cmd = AttackCmd.Get();
                cmd.Demage = data.Power;
                cmd.SrcActor = this._actor.Ref;
                // TODO Temp 目标应该使用Actor而不是使用GameObject 导致这里只能强行获取LocalPlayer 
                cmd.DesActor = GameMain.Instance.CurrentGamePlay.ActorManager.LocalPlayer;
                GameMain.Instance.CurrentGamePlay.ActorManager.LocalPlayer.AcceptCmd(cmd);
                cmd.Release();
            }

            this._actor.Ref.IsNeedRecover = true;
            EffectCmd effectCmd = EffectCmd.Get();
            effectCmd.PlayWorldPos = this.navTarget.position;
            effectCmd.EffectPath = String.Empty; // TODO 爆炸特效路径
            GameMain.Instance.CurrentGamePlay.AcceptCmd(effectCmd);
            effectCmd.Release();
        }
    }
}