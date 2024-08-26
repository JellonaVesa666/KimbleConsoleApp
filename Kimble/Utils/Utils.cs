using Newtonsoft.Json.Linq;
using static Globals;

public class Utils
{
    public static void InitProjectPath()
    {
        // Set Project Path
        projectPath = GetProjectPath();
        if (projectPath == null)
        {
            LogError("Could not find solution path");
            ExitApplication();
        }
    }

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
                    ExitApplication();
                    return;
                }
                if (marker.Pos.Local < 0)
                {
                    LogError($"{marker.Name} has exceeded min bounds");
                    game.Stop();
                    ExitApplication();
                    return;
                }
                if (marker.AtBase() && marker.AtGoal())
                {
                    LogError($"{marker.Name} is both at goal and base");
                    game.Stop();
                    ExitApplication();
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
                    ExitApplication();
                    return;
                }
            }
        }
    }

    public static void CreateFolder(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        return;
    }
}
