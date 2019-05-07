using System.Collections.Generic;
using assets;
using Component.Widget;
using GamePlay.Actor;
using GamePlay.Command;
using NetInput;
using UnityEngine;

namespace Component.Actor
{
    public class WeapenComp : ActorBaseComponent, IAcceptCommand
    {
        private IGun _currentGun = null;
        private List<IGun> _guns = new List<IGun>();
        public WeapenComp(IActor actor) : base(actor)
        {
        }

        public override void Init()
        {
            LocalPlayerBehaviorComp comp =
                this._actor.Ref.GetActorComponent(ActorComponentType.PlayerBehaviorComponent) as LocalPlayerBehaviorComp;
            IGun gun = new LineGun();
            gun.Init(comp.WeapenTrans);
            this._guns.Add(gun);
            gun.Enable = true;
            this._currentGun = gun;
            gun.StopFire();
        }

        public override void Start()
        {
        }

        public override void Update(float deltaTime)
        {
            if (this._currentGun == null)
            {
                return;
            }
//            if (NetInput.CurrentInput.CurInput.GetKey(InputKeyType.Fire))
//            {
//                this._currentGun.Fire();
//            }
//            else
//            {
//                this._currentGun.StopFire();
//            }
        }

        public void AcceptCmd(IBaseCommand cmd)
        {
            if (cmd.CmdType != CmdType.InputCmd)
            {
                return;
            }
            InputCmd icmd = cmd as InputCmd;
            if (icmd == null)
            {
                return;
            }

            switch (icmd.Action_Type)
            {
                    case InputCmd.ActionType.Fire:
                        this._currentGun.Fire();
                        break;
                    case InputCmd.ActionType.StopFire:
                        this._currentGun.StopFire();
                        break;
            }

            icmd.IsUse = true;
        }
    }

    
}