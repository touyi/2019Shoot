using assets;
using GamePlay.Actor;

namespace Component.Actor
{
    public class LocalPlayerMove : ActorBaseComponent
    {
        public LocalPlayerMove(IActor actor) : base(actor)
        {
        }
        
        public override void Init()
        {
            // TODO 666
            // AssetsManager.Instance.LoadPrefab()
        }

        public override void Start()
        {
        }

        public override void Uninit()
        {
        }

        public override void Update(float deltaTime)
        {
        }
    }
}