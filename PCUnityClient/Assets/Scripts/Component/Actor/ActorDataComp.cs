using GamePlay;
using GamePlay.Actor;
using GamePlay.Command;

namespace Component.Actor
{
    public class ActorDataComp : ActorBaseComponent,IAcceptCommand
    {
        private float maxHp = 0;
        private float currentHp = 0;
        private float power = 0;

        public float MaxHp
        {
            get { return maxHp; }
            set { maxHp = value; }
        }

        public float CurrentHp
        {
            get { return currentHp; }
            set
            {
                currentHp = value;
                EventData data = EventData.Get();
                data.longPara = this._actor.Ref.ActorGid;
                data.floatPara = currentHp;
                GameMain.Instance.CurrentGamePlay.Dispathcer.LaunchEvent(GameEventDefine.ActorLifeChange, data);
                data.Release();
            }
        }

        public float Power
        {
            get { return power; }
            set { power = value; }
        }

        public void SetValue(float hp, float maxHp, float power)
        {
            this.currentHp = hp;
            this.maxHp = maxHp;
            this.power = power;
        }

        public ActorDataComp(IActor actor) : base(actor)
        {
        }

        public void AcceptCmd(IBaseCommand cmd)
        {
            if (cmd.CmdType != CmdType.AttackCmd)
            {
                return;
            }
            
            AttackCmd attackCmd = cmd as AttackCmd;
            if (attackCmd == null || attackCmd.DesActor != this._actor.Ref.ActorGid)
            {
                return;
            }

            this.CurrentHp -= attackCmd.Demage;
        }
    }
}