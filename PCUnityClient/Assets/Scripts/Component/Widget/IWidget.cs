using Component.Actor;

namespace Component.Widget
{
    public interface IWidget
    {
        void Init(ActorBaseComponent parentComp);
        void Uninit();
        void Update(float deltaTime);
    }
}