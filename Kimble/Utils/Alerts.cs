using Newtonsoft.Json.Linq;
using static Utils;
using static Globals;

public class Alerts
{

    public enum MarkerAlert
    {
        Spawn,
        Move,
        Eat,
        Goal,
    }

    public enum StatisticAlert
    {
        Win
    }

    public static void LogMarkerAlert(MarkerAlert alert, Marker? marker)
    {
        if (alert == MarkerAlert.Spawn)
        {
            Console.WriteLine($"SPAWN {marker.Name} | Global Pos:{marker.Pos.Global}  /  Local Pos:{marker.Pos.Local}");
        }
        if (alert == MarkerAlert.Move)
        {
            Console.WriteLine($"MOVE {marker.Name} | Global Pos:{marker.Pos.Global}  /  Local Pos:{marker.Pos.Local}");
        }
        if (alert == MarkerAlert.Eat)
        {
            Console.WriteLine($"EAT | {marker.Name} ate other marker on board");
        }
        if (alert == MarkerAlert.Goal)
        {
            Console.WriteLine($"GOAL {marker.Name} | Slot:{marker.GoalSlot}");
        }
    }

    public static void LogStatisticAlert(StatisticAlert alert, Team team, Statistics statistics)
    {
        CreateFolder($"{projectPath}" + @"\Logs");

        string statisticsPath = $"{projectPath}" + @"\Logs\Statistics.json";
        JObject statisticsJObject = GetJObject(statisticsPath);

        if (alert == StatisticAlert.Win)
        {
            Statistics totalStatistics = new Statistics(
                  gameCount: statisticsJObject.Value<int>("gameCount"),
                  turnCount: statisticsJObject.Value<int>("turnCount"),
                  eatCount: statisticsJObject.Value<int>("eatCount"),
                  spawnCount: statisticsJObject.Value<int>("spawnCount"),
                  moveCount: statisticsJObject.Value<int>("moveCount"),
                  wins: statisticsJObject["wins"].ToObject<int[]>()
            );


            int averageTurnCount = totalStatistics.TurnCount / totalStatistics.GameCount;
            int averageEatCount = totalStatistics.EatCount / totalStatistics.GameCount;
            int averageSpawnCount = totalStatistics.SpawnCount / totalStatistics.GameCount;
            int averageMoveCount = totalStatistics.MoveCount / totalStatistics.GameCount;


            Console.WriteLine("\n-------------Winner--------------");
            Console.WriteLine($"Team {team.Color} has won the game !");
            Console.WriteLine("\n----------Game Statistics-----------");
            Console.WriteLine($"Turn count:{statistics.TurnCount}");
            Console.WriteLine($"Spawns count:{statistics.SpawnCount}");
            Console.WriteLine($"Move count:{statistics.MoveCount}");
            Console.WriteLine($"Eat count:{statistics.EatCount}");

            Console.WriteLine("\n----------Total Statistics-----------");
            Console.WriteLine($"Total games:{totalStatistics.GameCount}");
            Console.WriteLine($"Average turn count per game:{averageTurnCount}");
            Console.WriteLine($"Average move count per game:{averageMoveCount}");
            Console.WriteLine($"Average spawn count per game:{averageSpawnCount}");
            Console.WriteLine($"Average eat count per game:{averageEatCount}");
        }
    }
}
