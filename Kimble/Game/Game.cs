public enum State
{
    Roll,
    Thinking,
    Spawn,
    Move,
    Arrive,
    Validate,
    WinCheck,
    EndTurn,
}

public class Game
{
    public int Turn { get; set; }
    public int TeamsCount { get; set; }
    public int Iterations { get; set; }
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

        Iterations = settings.Iterations;
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

    public void Iterate()
    {
        Turn = 0;
        State = global::State.Roll;
        Iterations--;
    } 

    public int NextTurn()
    {
        Turn++;
        return Turn;
    }

    public Team GetStartTeam(Dictionary<int, Team> teams)
    {
        Team team = teams[0 % teams.Count];
        Console.WriteLine($"{team.Color} team turn");
        return team;
    }

    public Team GetNextTeam(Dictionary<int, Team> teams)
    {
        Team team = teams[Turn % teams.Count];
        Console.WriteLine($"{team.Color} team turn");
        return team;
    }
}