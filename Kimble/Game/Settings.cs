public class Settings
{
    public int Iterations {  get; set; }
    public int LapSize { get; set; }
    public int GoalSlots { get; set; }
    public int TeamsCount { get; set; }
    public int TeamMarkers { get; set; }
    public List<Dictionary<string, float>> TeamRiskTaking { get; set; }
    public List<Dictionary<string, int[]>> TeamSkills { get; set; }

    public Settings(int iterations = 1,int lapSize = 28, int goalSlots = 4, int teamsCount = 4, int teamMarkers = 4, List<Dictionary<string, float>> teamRiskTaking = null, List<Dictionary<string, int[]>> teamSkills = null)
    {
        Iterations = iterations;
        LapSize = lapSize;
        GoalSlots = goalSlots;
        TeamsCount = teamsCount;
        TeamMarkers = teamMarkers;
        TeamRiskTaking = teamRiskTaking == null ? new() : teamRiskTaking;
        TeamSkills = teamSkills == null ? new() : teamSkills;
    }
}