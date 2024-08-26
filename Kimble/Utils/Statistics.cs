using Newtonsoft.Json.Linq;
using static Utils;
using static Globals;

public class Statistics
{
    public int GameCount { get; set; }
    public int TurnCount { get; set; }
    public int EatCount { get; set; }
    public int SpawnCount { get; set; }
    public int MoveCount { get; set; }
    public Dictionary<Color, int> Wins { get; set; }

    public Statistics(int teamsCount)
    {
        GameCount = 0;
        TurnCount = 0;
        EatCount = 0;
        SpawnCount = 0;
        MoveCount = 0;
        Wins = new();
        for (int i = 0; i < teamsCount; i++)
        {
            Wins.Add((Color)i, 0);
        }
    }

    public void Save()
    {
        CreateFolder($"{projectPath}" + @"\Logs");

        string statisticsPath = $"{projectPath}" + @"\Logs\Statistics.json";
        JObject statisticsJObject = GetJObject(statisticsPath);
        JObject? saveObject = null;

        saveObject = JObject.FromObject(new
        {
            gameCount = this.GameCount,
            AverageTurnCount = this.TurnCount / this.GameCount,
            AverageEatCount = this.EatCount / this.GameCount,
            AverageSpawnCount = this.SpawnCount / this.GameCount,
            AverageMoveCount = this.MoveCount / this.GameCount,
            turnCount = this.TurnCount,
            eatCount = this.EatCount,
            spawnCount = this.SpawnCount,
            moveCount = this.MoveCount,
            wins = this.Wins
        });

        if (saveObject != null)
        {
            try
            {
                File.WriteAllText(statisticsPath, saveObject.ToString());
            }
            catch (Exception ex)
            {
                LogError("Something went wrong when saving statistics");
            }
        }
    }
}