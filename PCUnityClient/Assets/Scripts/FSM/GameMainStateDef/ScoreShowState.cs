using GamePlay;
using GamePlay.Actor;
using GamePlay.Command;
using Tools;
using Wrapper;

namespace FSM.GameMainStateDef
{
    public class ScoreShowState : BaseState
    {
        private float timer = 10f;
        private long localPlayerGid = -1;
        private WeakRef<StateMachine<GameMainState, GameMainEvent>> _fsm =
            new WeakRef<StateMachine<GameMainState, GameMainEvent>>();
        public void Enter()
        {
            this.timer = 10f;
            NormalGamePlay gamePlay = GameMain.Instance.CurrentGamePlay;
            ActorManager actorManager = gamePlay.ActorManager;
            ActorBuildData data = ActorBuildData.Get();
            data.BornWorldPos = gamePlay.LevelManager.GetLocalPlayerPos(0);
            data.type = ActorType.EmptyLocalPlayer;
            this.localPlayerGid = actorManager.CreateActor(data);
            data.Release();
            
            CMDHelper.AcceptUICmdToActorManager(UICmd.UIType.ScoreUI, UICmd.UIState.Open);
        }

        public void Execute(float deltaTime)
        {
            timer -= deltaTime;
            if (timer <= 0)
            {
                this._fsm.Ref.Fire(GameMainEvent.End);
            }
        }

        public void Exit()
        {
            CMDHelper.AcceptUICmdToActorManager(UICmd.UIType.ScoreUI, UICmd.UIState.Close);
            GameMain.Instance.CurrentGamePlay.ActorManager.DestoryActorByGid(this.localPlayerGid);
        }

        public void RegistToFsm(StateMachine<GameMainState, GameMainEvent> fsm)
        {
            fsm.In(GameMainState.ScoreShow)
                .On(GameMainEvent.End).GoTo(GameMainState.WaitScan)
                .Attach(this);
            this._fsm.Ref = fsm;
        }
    }
}