public class Game
{
    public int Turn { get; set; } = 0;
    public int TeamsCount { get; set; } = 0;
    public int LapSize { get; set; } = 0;
    public int GoalSlots { get; set; } = 0;
    public int MinGoalRange { get; set; } = 0;
    public int MaxGoalRange { get; set; } = 0;
    public bool Update { get; set; } = false;

    public Game(int lapSize, int teamsCount, int goalSlots)
    {
        Turn = 0;

        LapSize = lapSize;

        TeamsCount = teamsCount;

        GoalSlots = goalSlots;
        MinGoalRange = lapSize - 5;
        MaxGoalRange = lapSize + goalSlots;
    }

    public int NextTurn() 
    {
        Turn++;
        return Turn;
    }

    public void Start()
    {
        Update = true;
    }

    public void Stop() 
    { 
        Update = false;
    }
}