public enum Color
{
    Red,
    Green,
    Blue,
    Yellow
}

public enum Skill
{
    MarkerEater, // Tries to eat other team markers when possible.
    AmbushStart, // Tires to eat other team marker is current team spawn slot.
    SafeStart,   // Avoid spawning marker when chance to get eaten is higher than risk taking float.
}

public class Team
{
    public Color Color { get; set; }
    public Marker[] Markers { get; set; }
    public Skill[] Skills { get; set; }
    public int StartSlot { get; set; }
    public float RiskTaking { get; set; }

    public Team(Color color, int teamSize, int[] skills, int startSlot, float riskTaking)
    {
        Color = color;

        Markers = new Marker[teamSize];
        for (int i = 0; i < teamSize; i++)
        {
            string name = $"{color}{(i + 1)}";
            Markers[i] = new(name);
        }

        Skills = new Skill[skills.Length];
        for (int i = 0; i < skills.Length; i++)
        {
            Skills[i] = (Skill)i;
        }

        StartSlot = startSlot;
        RiskTaking = riskTaking;
    }

    /// <summary>
    /// Check if team has markers on the board.
    /// </summary>
    /// <returns>bool</returns>
    public bool HasMarkersOnBoard()
    {
        return Markers.Where(marker => marker.OnBoard()).Count() > 0;
    }

    /// <summary>
    /// Get all team markers on board.
    /// </summary>
    /// <returns>Marker[]</returns>
    public Marker[] GetMarkersOnBoard()
    {
        return Markers.Where(marker => marker.OnBoard()).ToArray();
    }

    /// <summary>
    /// Check if team has markers at base.
    /// </summary>
    /// <returns>bool</returns>
    public bool HasMarkersAtBase()
    {
        return Markers.Where(marker => marker.AtBase()).ToArray().Length > 0;
    }

    /// <summary>
    /// Get one team marker from the base
    /// </summary>
    /// <returns>Marker</returns>
    public Marker GetMarkerFromBase()
    {
        return Markers.Where(marker => marker.Pos.Local == 0).FirstOrDefault();
    }

    public bool HasMarkerAtStartSlot()
    {
        return Markers.Where(marker => marker.Pos.Local == 1).Count() > 0;
    }

    public bool MoveSlotOccupied(int newPos)
    {
        return Markers.Where(marker => marker.Pos.Local == newPos).Count() > 0;
    }

    public bool GoalSlotOccupied(int slot)
    {
        return Markers.Where(marker => marker.GoalSlot == slot).Count() > 0;
    }

    public bool HasSkill(Skill skill)
    {
        return Skills.Where(s => s == skill).Count() > 0;
    }

    public Marker[] GetMarkersInRange(int minRange, int maxRange)
    {
        return Markers.Where(marker => marker.Pos.Local >= minRange && marker.Pos.Local <= maxRange).ToArray();
    }

    public bool CheckWinCondition()
    {
        return Markers.Where(marker => marker.AtGoal()).Count() == 4;
    }
}

