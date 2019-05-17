using GamePlay.Actor;

namespace Component.Actor
{
    public static class ActorHelp
    {
        public static ActorDataComp ActorData(this IActor actor)
        {
            if (actor == null)
            {
                return null;
            }

            var data = actor.GetActorComponent(ActorComponentType.ActorDataComponent) as ActorDataComp;
            return data;
        }
    }
}