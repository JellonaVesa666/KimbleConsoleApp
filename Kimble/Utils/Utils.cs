using Newtonsoft.Json.Linq;
using static Globals;

public class Utils
{
    public static void LogError(string message)
    {
        Console.WriteLine($"Error: {message}");
    }

    public static void ExitApplication()
    {
        while (true)
        {
            Console.WriteLine("\n");
            Console.WriteLine("Press enter to exit application...");

            ConsoleKeyInfo CurrentInputKey = Console.ReadKey();

            if (CurrentInputKey.Key == ConsoleKey.Enter)
            {
                Console.WriteLine("Exit ...");
                Environment.Exit(0);
            }
        }
    }

    public static string? GetProjectPath()
    {
        try
        {
            return Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.IndexOf("bin"));
        }
        catch
        {
            try
            {
                return Directory.GetCurrentDirectory();
            }
            catch
            {
                return null;
            }
        }
    }

    public static JObject? GetJObject(string path) 
    {
        try
        {
            return JObject.Parse(File.ReadAllText(@$"{path}"));
        }
        catch
        {
            return null;
        }
    }

    public static float CalculateProbability(int rolls)
    {
        return (float)Math.Min(1 - Math.Pow(5.0 / 6.0, rolls), 1);
    }

    public static void SaveStatistics(Game game, Team team)
    {
        string statisticsPath = $"{projectPath}" + @"\Logs\Statistics.json";
        JObject statisticsJObject = GetJObject(statisticsPath);

        JObject? saveObject = null;

        if (statisticsJObject == null)
        {
            saveObject = JObject.FromObject(new
            {
                gameCount = statistics.GameCount,
                turnCount = statistics.TurnCount,
                eatCount = statistics.EatCount,
                spawnCount = statistics.SpawnCount,
                moveCount = statistics.MoveCount,
                wins = statistics.Wins
            });
        }
        else if (statisticsJObject != null)
        {
            foreach (var item in statisticsJObject)
            {
                string[] settingsProps = ["gameCount", "turnCount", "eatCount", "spawnCount", "moveCount", "wins"];
                bool match = settingsProps.FirstOrDefault(props => props == item.Key && item.Value != null).Length > 0;

                if (!match)
                {
                    if (File.Exists(statisticsPath))
                    {
                        File.Delete(statisticsPath);
                    }
                    LogError("Saving statistics failed, missing Statistics properties");
                    ExitApplication();
                }
            }

            // Load JSON data to temp statistics object
            Statistics tempStatistics = new Statistics(
                gameCount: statisticsJObject.Value<int>("gameCount"),
                turnCount: statisticsJObject.Value<int>("turnCount"),
                eatCount: statisticsJObject.Value<int>("eatCount"),
                spawnCount: statisticsJObject.Value<int>("spawnCount"),
                moveCount: statisticsJObject.Value<int>("moveCount"),
                wins: statisticsJObject["wins"].ToObject<int[]>()
            );

            // Add current game statics to temp statistics object
            tempStatistics.GameCount += statistics.GameCount;
            tempStatistics.TurnCount += statistics.TurnCount;
            tempStatistics.EatCount += statistics.EatCount;
            tempStatistics.SpawnCount += statistics.SpawnCount;
            tempStatistics.MoveCount += statistics.MoveCount;
            tempStatistics.Wins[(int)team.Color]++;

            saveObject = JObject.FromObject(new
            {
                gameCount = tempStatistics.GameCount,
                turnCount = tempStatistics.TurnCount,
                eatCount = tempStatistics.EatCount,
                spawnCount = tempStatistics.SpawnCount,
                moveCount = tempStatistics.MoveCount,
                wins = tempStatistics.Wins
            });
        }

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

    public static void ValidateMarkers(Dictionary<int, Team> teams, Team team)
    {
        foreach (KeyValuePair<int, Team> entry in teams)
        {
            foreach (Marker marker in teams[entry.Key].Markers)
            {
                if (marker.Pos.Local > game.MaxGoalRange)
                {
                    LogError($"{marker.Name} has exceeded max bounds");
                    game.Stop();
                    return;
                }
                if (marker.Pos.Local < 0)
                {
                    LogError($"{marker.Name} has exceeded min bounds");
                    game.Stop();
                    return;
                }
                if (marker.AtBase() && marker.AtGoal())
                {
                    LogError($"{marker.Name} is both at goal and base");
                    game.Stop();
                    return;
                }
            }

            var test = team.Markers.Where(marker => marker.AtGoal()).ToArray();
            if (test.Length == 4)
            {
                if (test.Distinct().Count() < 4)
                {
                    LogError($"Team {team.Color} has two or more markers occupying same goal slot");
                    game.Stop();
                    return;
                }
            }
        }
    }
}
