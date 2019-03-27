using FSM;
using GamePlay.Actor;

namespace GamePlay
{
    public class NormalGamePlay : IGamePlay
    {
        private MainFSMStarter _fsmStarter = null;
        private ActorManager _actorManager = null;
        private LevelManager _levelManager = null;
        private bool _isRunning = false;

        public bool IsRunning
        {
            get { return _isRunning; }
        }

        public MainFSMStarter FsmStarter
        {
            get { return _fsmStarter; }
            set { _fsmStarter = value; }
        }

        public ActorManager ActorManager
        {
            get { return _actorManager; }
            set { _actorManager = value; }
        }

        public LevelManager LevelManager
        {
            get { return _levelManager; }
            set { _levelManager = value; }
        }

        public void Start()
        {
            _actorManager.Init();
            _fsmStarter.Init();
            
            _actorManager.Start();
            _fsmStarter.Start();

            _isRunning = true;
        }

        public void Update(float deltaTime)
        {
            ActorManager.Update(deltaTime);
            _fsmStarter.Update(deltaTime);
        }

        public void Init()
        {
            _levelManager.InitScence();
        }

        public void Uninit()
        {
            // 销毁
            _fsmStarter.Uninit();
            _actorManager.Uninit();
        }
    }
}