using Newtonsoft.Json.Linq;
using static Utils;

internal class Program
{
    private static string? _projectPath = null;
    private static JObject? _settings = null;
    private static Game? _game = null;
    private static Dice? _dice = null;

    private static Dictionary<int, Team>? _teams = null;

    static void Main(string[] args)
    {
        OnStart();
        OnUpdate();
    }

    public static void OnStart()
    {
        _projectPath = GetProjectPath();

        if (_projectPath == null)
        {
            LogError("Could not find solution path");
            ExitApplication();
        }

        string settingsPath = $"{_projectPath}" + @"\Settings\\GameSettings.json";
        _settings = GetJObject(settingsPath);

        if (_settings == null)
        {
            LogError("Could not initialize game settings");
            ExitApplication();
        }

        _game = new(
            lapSize: _settings!.Value<int>("lapSize"),
            goalSlots: _settings!.Value<int>("goalSlots"),
            teamsCount: _settings!.Value<int>("teamsCount")
        );


        _teams = new Dictionary<int, Team>();

        for (int i = 0; i < _game.TeamsCount; i++)
        {
            int globalOffset = _game.LapSize / _game.TeamsCount * i;
            _teams[i] = new(
                color: (Color)i,
                teamSize: _settings.Value<int>("teamMarkers"),
                skills: [0, 1, 2],
                startSlot: globalOffset + 1,
                riskTaking: 0.5f
            );
        }

        _dice = new();


        if (_projectPath != null &&
            _settings != null &&
            _game != null &&
            _teams != null &&
            _dice != null
        )
        {
            _game.Start();
            return;
        }

        LogError("Something went wrong when seting game up");
        ExitApplication();
    }

    public static void OnUpdate()
    {
        while (_game!.Update)
        {
            Console.WriteLine("Closing Program Announcement");
            Thread.Sleep(5000);


            _game.Stop();
            ExitApplication();
        }
    }
}