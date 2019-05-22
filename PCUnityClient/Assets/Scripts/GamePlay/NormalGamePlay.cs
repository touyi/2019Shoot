using FSM;
using GamePlay.Actor;
using GamePlay.Command;
using GamePool.OBJ;
using UnityEngine;

namespace GamePlay
{
    public class NormalGamePlay : IGamePlay, IAcceptCommand
    {
        private MainFSMStarter _fsmStarter = null;
        private ActorManager _actorManager = null;
        private LevelManager _levelManager = null;
        private IDispathcer _dispathcer = null;
        private IDataProvider _dataProvider = null;
        private bool _isRunning = false;

        public IDataProvider DataProvider
        {
            get { return _dataProvider; }
            set { _dataProvider = value; }
        }

        public IDispathcer Dispathcer
        {
            get { return _dispathcer; }
            set { _dispathcer = value; }
        }

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
            
            _levelManager.Start();
            _dispathcer.Start();
            _actorManager.Start();
            _fsmStarter.Start();
            
            _isRunning = true;
        }

        public void Update(float deltaTime)
        {
            _dispathcer.Update(deltaTime);
            ActorManager.Update(deltaTime);
            _fsmStarter.Update(deltaTime);
        }

        public void Init()
        {
            _dispathcer.Init();
            _levelManager.InitScence();
            

        }

        public void Uninit()
        {
            // 销毁
            _fsmStarter.Uninit();
            _actorManager.Uninit();
            _dispathcer.Uninit();
        }

        public void AcceptCmd(IBaseCommand cmd)
        {
            if (cmd.IsUse == true)
            {
                return;
            }

            if (cmd.CmdType == CmdType.EffectCmd)
            {
                EffectCmd effectCmd = cmd as EffectCmd;
                GPGameObjectPool.Creat<GPExplosion>(effectCmd.PlayWorldPos, Quaternion.identity);
                effectCmd.IsUse = true;
                return;
            }
            this._actorManager.AcceptCmd(cmd);
        }
    }
}