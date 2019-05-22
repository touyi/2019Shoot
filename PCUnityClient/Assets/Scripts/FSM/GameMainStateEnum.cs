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
        Go,
        GameOver,
        End,
    }
}