public enum Color
{
    Red,
    Blue,
    Violet,
    Green
}

public enum Skill
{
    MarkerEater,
    AmbushStart,
    SafeStart,
}

public class Team
{
    public Color? Color { get; set; } = null;
    public Marker[]? Markers { get; set; } = null;
    public Skill[]? Skills { get; set; } = null;
    public int? StartSlot { get; set; } = null;
    public float? RiskTaking { get; set; } = null;

    public Team(Color color, int teamSize, int[] skills, int startSlot, float riskTaking)
    {
        Color = color;
        Markers = new Marker[teamSize];
        for (int i = 0; i < teamSize; i++)
        {
            string name = $"{color}{(i + 1)}";
            Markers[i] = new(name);
        }
        Skills = [Skill.MarkerEater, Skill.AmbushStart, Skill.SafeStart];
        StartSlot = startSlot;
        RiskTaking = 0.5f;
    }
}

