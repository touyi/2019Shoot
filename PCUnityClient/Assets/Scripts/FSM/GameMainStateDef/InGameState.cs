using System.Collections;
using System.Collections.Generic;
using GamePlay;
using GamePlay.Actor;
using GamePlay.Command;
using GamePool.OBJ;
using Message;
using MessageSystem;
using NetInput;
using Protocol;
using Tools;
using UnityEngine;
using Wrapper;
using CmdType = Message.CmdType;

namespace FSM.GameMainStateDef
{
    // TODO
    public class InGameState : BaseState
    {
        private bool lastFireState = false;
        private int remainEnemy = 0;
        private int currentWave = 0;
        private long localPlayerGid = 0;
        private WaveInfo _waveInfo = new WaveInfo();
        
        private WeakRef<StateMachine<GameMainState, GameMainEvent>> _fsm =
            new WeakRef<StateMachine<GameMainState, GameMainEvent>>();
        public void Enter()
        {
            Debug.Log("Enter:" + this.GetType().ToString());
            currentWave = 0;
            lastFireState = false;
            remainEnemy = 0;
            GameMain.Instance.CurrentGamePlay.Dispathcer.RegistListener(GameEventDefine.GameEnd, this.OnGameEnd);
            GameMain.Instance.CurrentGamePlay.Dispathcer.RegistListener(GameEventDefine.ActorLifeChange,
                this.OnActorLifeChange);
            NormalGamePlay gamePlay = GameMain.Instance.CurrentGamePlay as NormalGamePlay;
            if (gamePlay == null)
            {
                Debug.Log("gameplay is null");
                return;
            }

            ActorManager actorManager = gamePlay.ActorManager;
            ActorBuildData data = ActorBuildData.Get();
            data.BornWorldPos = gamePlay.LevelManager.GetLocalPlayerPos(0);
            data.type = ActorType.LocalPlayer;
            data.HP = 100;
            data.MaxHp = data.HP;
            data.Power = 10;
            this.localPlayerGid = actorManager.CreateActor(data);
            data.Release();

            CMDHelper.AcceptUICmdToActorManager(UICmd.UIType.HPUI, UICmd.UIState.Open);
            CMDHelper.AcceptUICmdToActorManager(UICmd.UIType.RadarUI, UICmd.UIState.Open);

            GPGameObjectPool.ReFormPoolObject<GPExplosion>(10);
        }

        public void Execute()
        {
            bool isFire = CurrentInput.CurInput.GetKey(InputKeyType.Fire);
            if (isFire && (lastFireState != isFire))
            {
                InputCmd cmd = InputCmd.Get();
                cmd.Action_Type = InputCmd.ActionType.Fire;
                GameMain.Instance.CurrentGamePlay.ActorManager.AcceptCmd(cmd);
                cmd.Release();
            }
            else if (!isFire && lastFireState != isFire)
            {
                InputCmd cmd = InputCmd.Get();
                cmd.Action_Type = InputCmd.ActionType.StopFire;
                GameMain.Instance.CurrentGamePlay.ActorManager.AcceptCmd(cmd);
            }
            lastFireState = isFire;

            if (remainEnemy <= 0)
            {
                // 产生新波次
                var datas = this._waveInfo.ProductWaveEnemyDatas(++currentWave);
                remainEnemy = datas.Count;
                Debug.Log(remainEnemy);
                for (int i = 0; i < remainEnemy; i++)
                {
                    GameMain.Instance.CurrentGamePlay.ActorManager.CreateActor(datas[i]);
                }
            }
        }

        public void Exit()
        {
            GameMain.Instance.CurrentGamePlay.ActorManager.DestoryActorByType(ActorType.Enemy);
            GameMain.Instance.CurrentGamePlay.ActorManager.DestoryActorByGid(this.localPlayerGid);
            GPGameObjectPool.ReFormPoolObject<GPExplosion>(0);
            GameMain.Instance.CurrentGamePlay.Dispathcer.RemoveListener(GameEventDefine.GameEnd, this.OnGameEnd);
            GameMain.Instance.CurrentGamePlay.Dispathcer.RemoveListener(GameEventDefine.ActorLifeChange,
                this.OnActorLifeChange);
            CMDHelper.AcceptUICmdToActorManager(UICmd.UIType.HPUI, UICmd.UIState.Close);
            CMDHelper.AcceptUICmdToActorManager(UICmd.UIType.RadarUI, UICmd.UIState.Close);
            
        }

        public void RegistToFsm(StateMachine<GameMainState, GameMainEvent> fsm)
        {
            fsm.In(GameMainState.InGame)
                .On(GameMainEvent.GameOver).GoTo(GameMainState.ScoreShow)
                .On(GameMainEvent.End).GoTo(GameMainState.ScoreShow)
                .Attach(this);

            this._fsm.Ref = fsm;
        }

        private void OnActorLifeChange(EventData data)
        {
            if (data.floatPara <= 0 && GameMain.Instance.CurrentGamePlay.ActorManager.IsEnemy(data.longPara))
            {
                remainEnemy--;
            }
        }

        private void OnGameEnd(EventData param)
        {
            this._fsm.Ref.Fire(GameMainEvent.GameOver);
        }
    }
}