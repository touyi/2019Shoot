using GamePlay.Actor;

namespace Component.Actor
{
    public class ActorDataComp : ActorBaseComponent
    {
        private float hp = 0;
        private float power = 0;

        public float Hp
        {
            get { return hp; }
            set { hp = value; }
        }

        public float Power
        {
            get { return power; }
            set { power = value; }
        }

        public ActorDataComp(IActor actor) : base(actor)
        {
        }
    }
}