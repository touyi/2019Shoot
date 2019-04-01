using System.Collections.Generic;
using assets;
using Component.Widget;
using GamePlay.Actor;
using NetInput;
using UnityEngine;

namespace Component.Actor
{
    public class WeapenComp : ActorBaseComponent
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
            if (NetInput.CurrentInput.CurInput.GetKey(InputKeyCode.Fire))
            {
                this._currentGun.Fire();
            }
            else
            {
                this._currentGun.StopFire();
            }
        }
    }

    
}