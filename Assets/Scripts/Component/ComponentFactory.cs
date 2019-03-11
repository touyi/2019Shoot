using Component.Actor;
using GamePlay.Actor;

namespace Component
{
    public static class ComponentFactory
    {
        public static ActorBaseComponent CreateActorComponent(ActorComponentType type, IActor actor)
        {
            ActorBaseComponent baseComponent = null;
            switch (type)
            {
                // TODO
                case ActorComponentType.AttackComponent:
                    break;
                case ActorComponentType.MoveComponent:
                    break;
            }

            if (baseComponent != null)
            {
                baseComponent.Init();
            }
            return null;
        }

        public static void DestoryComponent(IBaseComponent baseComponent)
        {
            baseComponent.Uninit();
        }
    }
}