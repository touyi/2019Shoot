using FSM;
using GamePlay.Actor;

namespace GamePlay
{
    public static class GamePlayBuilder
    {
        public static NormalGamePlay BuildNormalGamePlay()
        {
            NormalGamePlay gamePlay = new NormalGamePlay();
            gamePlay.ActorManager = new ActorManager();
            gamePlay.FsmStarter = new MainFSMStarter();
            gamePlay.LevelManager = new LevelManager();
            gamePlay.Dispathcer = new NormalDispatcher();
            gamePlay.DataProvider = new GlobalDataProvider();
            return gamePlay;
        }
    }
}