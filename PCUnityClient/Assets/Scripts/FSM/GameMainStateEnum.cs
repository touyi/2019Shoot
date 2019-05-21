namespace FSM
{
    public enum GameMainState
    {
        WaitScan,
        Guide,
        InGame,
        ScoreShow,
    }

    public enum GameMainEvent
    {
        Begin,
        GO,
        GameOver,
        End,
    }
}