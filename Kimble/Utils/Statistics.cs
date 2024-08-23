public class Statistics
{
    public int GameCount { get; set; }
    public int TurnCount { get; set; }
    public int EatCount { get; set; }
    public int SpawnCount { get; set; }
    public int MoveCount { get; set; }
    public int[] Wins { get; set; }

    public Statistics(int gameCount = 0, int turnCount = 0, int eatCount = 0, int spawnCount = 0, int moveCount = 0, int[] wins = null)
    {
        GameCount = gameCount;
        TurnCount = turnCount;
        EatCount = eatCount;
        SpawnCount = spawnCount;
        MoveCount = moveCount;
        wins = wins == null ? [0, 0, 0, 0]: wins;
        Wins = wins;
    }
}