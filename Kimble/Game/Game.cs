public enum State
{
    Roll,
    Thinking,
    Spawn,
    Move,
    Arrive,
    EndTurn,
}

public class Game
{
    public int Turn { get; set; }
    public int TeamsCount { get; set; }
    public int LapSize { get; set; }
    public int GoalSlots { get; set; }
    public int MinGoalRange { get; set; }
    public int MaxGoalRange { get; set; }
    public bool Update { get; set; }
    public State? State{ get; set; }

    public Game(Settings settings)
    {
        Turn = 0;
        Update = false;
        State = global::State.Roll;

        LapSize = settings.LapSize;

        TeamsCount = settings.TeamsCount;

        GoalSlots = settings.GoalSlots;
        MinGoalRange = settings.LapSize - 5;
        MaxGoalRange = settings.LapSize + settings.GoalSlots;
    }

    public void Start()
    {
        Update = true;
    }

    public void Stop() 
    { 
        Update = false;
    }

    public int NextTurn()
    {
        Turn++;
        return Turn;
    }

    public Team PickTeam(Dictionary<int, Team> teams)
    {
        Team team = teams![Turn % teams.Count];
        Console.WriteLine($"{team.Color} team turn");
        return team;
    }
}