using GamePlay.Actor;

namespace GamePlay
{
    public class NormalGamePlay : IGamePlay
    {
        public ActorManager ActorManager = null;
        // TODO
        public void Start()
        {
            throw new System.NotImplementedException();
        }

        public void Update(float deltaTime)
        {
            ActorManager.Update(deltaTime);
        }

        public void Init()
        {
            throw new System.NotImplementedException();
        }

        public void Uninit()
        {
            throw new System.NotImplementedException();
        }
    }
}