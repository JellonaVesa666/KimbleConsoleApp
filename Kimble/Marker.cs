using static Utils;

public class Position
{
    // Global position is used to compare markers positions between teams.
    public int? Global { get; set; } = null;
    // Local position is used for checking lap progress for marker.
    public int? Local { get; set; } = null;

    public Position()
    {
        Global = 0;
        Local = 0;
    }
}

public class Marker
{
    public string? Name { get; set; } = null;
    public Position? Pos { get; set; } = new();
    public int? GoalSlot { get; set; } = null;

    public Marker(string name)
    {
        Name = name;
        Pos = new Position();
        GoalSlot = 0;
    }

    public bool AtGoal()
    {
        return GoalSlot > 0;
    }

    public bool OnBoard()
    {
        return Pos?.Local > 0 && GoalSlot == 0;
    }

    public bool AtBase()
    {
        return Pos?.Local == 0 && GoalSlot == 0;
    }

    public void Spawn(int startSlot)
    {
        try
        {
            Pos!.Global = startSlot;
            Pos!.Local = 1;
        }
        catch
        {
            LogError("Something went wrong while spawning marker");
        }
    }

    public void Move(int movePos)
    {
        try
        {
            Pos!.Global += movePos;
            Pos!.Local += movePos;
        }
        catch
        {
            LogError("Something went wrong while moving marker");
        }
    }

    public void Reset()
    {
        try
        {
            Pos!.Global = 0;
            Pos!.Local = 0;
        }
        catch
        {
            LogError("Something went wrong while reseting marker");
        }
    }
}