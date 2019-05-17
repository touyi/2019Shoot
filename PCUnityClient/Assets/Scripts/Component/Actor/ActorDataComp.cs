using GamePlay;
using GamePlay.Actor;
using GamePlay.Command;

namespace Component.Actor
{
    public class ActorDataComp : ActorBaseComponent,IAcceptCommand
    {
        private float hp = 0;
        private float power = 0;

        public float Hp
        {
            get { return hp; }
            set
            {
                hp = value;
                EventData data = EventData.Get();
                data.longPara = this._actor.Ref.ActorGid;
                data.floatPara = hp;
                GameMain.Instance.CurrentGamePlay.Dispathcer.LaunchEvent(GameEventDefine.ActorLifeChange, data);
                data.Release();
            }
        }

        public float Power
        {
            get { return power; }
            set { power = value; }
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

            this.Hp -= attackCmd.Demage;
        }
    }
}