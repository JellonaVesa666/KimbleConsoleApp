using Newtonsoft.Json.Linq;
using System;

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

    public static JObject GetJObject(string path) 
    {
        return JObject.Parse(File.ReadAllText(@$"{path}"));
    }
}
