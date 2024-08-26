using Newtonsoft.Json.Linq;

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
        EndStatistics
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

    public static void LogStatisticAlert(StatisticAlert alert, Statistics statistics)
    {
        if (alert == StatisticAlert.EndStatistics)
        {
            var wins  = statistics.Wins.OrderBy(x => x.Value).Reverse().ToArray();

            Console.WriteLine($"\n----------Game Count {statistics.GameCount}-----------");

            Console.WriteLine("\n----------Average Statistics-----------");
            Console.WriteLine($"Turn count:{statistics.TurnCount / statistics.GameCount}");
            Console.WriteLine($"Spawns count:{statistics.SpawnCount / statistics.GameCount}");
            Console.WriteLine($"Move count:{statistics.MoveCount / statistics.GameCount}");
            Console.WriteLine($"Eat count:{statistics.EatCount / statistics.GameCount}");
            
            Console.WriteLine("\n----------Total Statistics-----------");
            Console.WriteLine($"Turn count:{statistics.TurnCount}");
            Console.WriteLine($"Spawns count:{statistics.SpawnCount}");
            Console.WriteLine($"Move count:{statistics.MoveCount}");
            Console.WriteLine($"Eat count:{statistics.EatCount}");

            Console.WriteLine("\n----------Wins By Team-----------");
            foreach (var win in wins) Console.WriteLine(win);
        }
    }
}
