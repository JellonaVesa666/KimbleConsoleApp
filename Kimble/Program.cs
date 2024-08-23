using Newtonsoft.Json.Linq;
using static Utils;
using static Globals;
using static Alerts;

internal class Program
{
    private static Team? _team;
    private static Marker? _activeMarker;
    private static Dictionary<int, Team>? _teams;

    static void Main(string[] args)
    {
        Initialize();
        CheckComponents();
        OnStart();
        OnUpdate();
    }

    /// <summary>
    /// Initialize essential data and components.
    /// </summary>
    public static void Initialize()
    {
        // Set Project Path
        projectPath = GetProjectPath();



        if (projectPath == null)
        {
            LogError("Could not find solution path");
            ExitApplication();
        }

        // Set Settings
        string settingsPath = $"{projectPath}" + @"\Settings\GameSettings.json";
        JObject settingsJObject = GetJObject(settingsPath);

        if (settingsJObject == null)
        {
            LogError("Could not initialize game settings");
            ExitApplication();
        }

        foreach (var item in settingsJObject)
        {
            string[] settingsProps = ["LapSize", "GoalSlots", "TeamsCount", "TeamMarkers", "TeamRiskTaking", "TeamSkills"];
            bool match = settingsProps.FirstOrDefault(props => props == item.Key && item.Value != null).Length > 0;

            if (!match)
            {
                LogError("Settings is missing properties, could not initialize game settings");
                ExitApplication();
            }
        }

        // Set Settings
        settings = new(
            lapSize: settingsJObject.Value<int>("LapSize"),
            goalSlots: settingsJObject.Value<int>("GoalSlots"),
            teamsCount: settingsJObject.Value<int>("TeamsCount"),
            teamMarkers: settingsJObject.Value<int>("TeamMarkers"),
            teamRiskTaking: settingsJObject["TeamRiskTaking"].ToObject<List<Dictionary<string, float>>>(),
            teamSkills: settingsJObject["TeamSkills"].ToObject<List<Dictionary<string, int[]>>>()
        );

        // Set Game
        game = new(settings);

        // Set Teams
        _teams = new();
        for (int i = 0; i < game.TeamsCount; i++)
        {
            string color = ((Color)i).ToString();
            int[] teamSkills = settings.TeamSkills[i][color];
            float teamRiskTaking = settings.TeamRiskTaking[i][color];

            Console.WriteLine(teamSkills);
            Console.WriteLine(teamRiskTaking);

            int globalOffset = game.LapSize / game.TeamsCount * i;
            _teams[i] = new(
                color: (Color)i,
                teamSize: settings.TeamMarkers,
                skills: teamSkills,
                startSlot: globalOffset + 1,
                riskTaking: teamRiskTaking
            );
        }

        // Set Dice
        dice = new();

        statistics = new();
    }

    /// <summary>
    /// Check initialization for essential components.
    /// </summary>
    public static void CheckComponents()
    {
        // Check essential components, naive check
        if (projectPath == null ||
            settings == null ||
            game == null ||
            _teams == null ||
            dice == null
        )
        {
            LogError("Something went wrong when seting game up");
            ExitApplication();
        }
    }

    /// <summary>
    /// Game start, setup.
    /// </summary>
    public static void OnStart()
    {
        _team = game.PickTeam(_teams);

        if (_team == null)
        {
            LogError("Something went wrong when picking starting team");
            ExitApplication();
        }

        game.Start();
    }

    /// <summary>
    /// Game update, loop.
    /// </summary>
    public static void OnUpdate()
    {
        while (game.Update && game.Turn < 5000)
        {
            switch (game.State)
            {
                case State.Roll:
                    dice.Roll();
                    Console.WriteLine($"Roll:{dice.Value}");
                    game.State = State.Thinking;
                    break;

                case State.Thinking:
                    var result = Thinking();
                    _activeMarker = result.Marker;
                    game.State = result.State;
                    break;

                case State.Spawn:
                    statistics.SpawnCount++;
                    _activeMarker.Spawn(_team.StartSlot);
                    LogMarkerAlert(MarkerAlert.Spawn, _activeMarker);
                    CheckIntersect(_activeMarker);
                    game.State = State.Roll;
                    break;

                case State.Move:
                    statistics.MoveCount++;
                    _activeMarker.Move(dice.Value);
                    LogMarkerAlert(MarkerAlert.Move, _activeMarker);
                    CheckIntersect(_activeMarker);
                    game.State = State.EndTurn;
                    break;

                case State.Arrive:
                    _activeMarker.Move(dice.Value);
                    LogMarkerAlert(MarkerAlert.Goal, _activeMarker);
                    game.State = State.EndTurn;
                    break;

                case State.EndTurn:
                    ValidateMarkers(_teams, _team);
                    if (_team.CheckWinCondition())
                    {
                        statistics.GameCount++;
                        statistics.Wins[(int)_team.Color]++;
                        SaveStatistics(game, _team);
                        LogStatisticAlert(StatisticAlert.Win, _team, statistics);
                        game.Stop();
                        break;
                    }
                    game.NextTurn();
                    statistics.TurnCount++;
                    _team = game.PickTeam(_teams);
                    game.State = State.Roll;
                    break;
            }
        }

        ExitApplication();
    }

    private static Marker? MarkerEater()
    {
        Marker[] markersOnBoard = _team.GetMarkersOnBoard();

        foreach (KeyValuePair<int, Team> entry in _teams)
        {
            if (_team.Color != entry.Value.Color)
            {
                foreach (Marker marker in markersOnBoard)
                {
                    int globalPos = marker.Pos.Global + dice.Value;
                    if (globalPos <= game.LapSize)
                    {
                        Marker? targetMarker = _teams[entry.Key].Markers.FirstOrDefault
                            (m => m.OnBoard() && (m.Pos.Global % game.LapSize) == (globalPos % game.LapSize));

                        if (targetMarker != null)
                        {
                            return marker;
                        }
                    }
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Process dice roll, find next suitable state, depending on team skills and risk taking.
    /// </summary>
    /// <returns>State, Marker</returns>
    public static (State State, Marker Marker) Thinking()
    {
        // Skill logic releated to eating markers
        if (_team.HasMarkersOnBoard())
        {
            if (_team.HasSkill(Skill.MarkerEater))
            {
                Marker[] markersOnBoard = _team.GetMarkersOnBoard();
                if (markersOnBoard.Length > 0)
                {
                    Marker? marker = MarkerEater();
                    if (marker != null)
                    {
                        return (State.Move, marker);
                    }
                }
            }
        }

        // Skill logic releated to spawning new markers
        if (dice.Value == 6 && !_team.HasMarkerAtStartSlot() && _team.HasMarkersAtBase())
        {
            // TODO: Create variable to prioritize between these two skills if team has both

            // Check for skill "Safe Start"
            if (_team.HasSkill(Skill.SafeStart))
            {
                float probability = SafeSpawn();
                if (probability <= _team.RiskTaking)
                {
                    Marker marker = _team.GetMarkerFromBase();
                    return (State.Spawn, marker);
                }

            }
            // Check for skill "Ambush Start"
            if (_team.HasSkill(Skill.AmbushStart))
            {
                if (AmbushStart())
                {
                    Marker marker = _team.GetMarkerFromBase();
                    return (State.Spawn, marker);
                }
            }
        }

        // Basic spawn logic
        if (!_team.HasMarkersOnBoard())
        {
            if (dice.Value == 6)
            {
                Marker marker = _team.GetMarkerFromBase();
                return (State.Spawn, marker);
            }
        }
        // Basic movement logic
        else
        {
            Marker[] markersInGoalRange = _team.GetMarkersInRange(game.MinGoalRange, game.MaxGoalRange);
            if (markersInGoalRange.Length > 0)
            {
                Marker marker = null;

                marker = TryReachGoal(markersInGoalRange);
                if (marker != null)
                {
                    return (State.Arrive, marker);
                }

                marker = TryMoveCloserToGoal(markersInGoalRange);
                if (marker != null)
                {
                    return (State.Move, marker);
                }
            }

            Marker[] outOfGoalRange = _team.Markers.Where(marker => marker.OnBoard() && marker.Pos.Local + dice.Value <= game.LapSize).ToArray();
            foreach (Marker marker in outOfGoalRange)
            {
                int localPos = marker.Pos.Local + dice.Value;
                if (!_team.MoveSlotOccupied(localPos))
                {
                    return (State.Move, marker);
                }
            }
        }

        return (State.EndTurn, null);
    }

    private static float SafeSpawn()
    {
        int markers = 0;
        foreach (KeyValuePair<int, Team> entry in _teams)
        {
            if (_team.Color != entry.Value.Color)
            {
                foreach (var marker in entry.Value.Markers)
                {
                    if (marker.OnBoard())
                    {
                        int maxRoll = 6;
                        int globalPos = marker.Pos.Global + maxRoll;
                        int offset = globalPos > game.LapSize ? game.LapSize : 0;

                        if (globalPos - (offset + _team.StartSlot) >= 0)
                        {
                            markers++;
                        }
                    }
                }
            }
        }

        if (markers > 0)
        {
            return CalculateProbability(markers);
        }

        return 0;
    }

    private static bool AmbushStart()
    {
        foreach (KeyValuePair<int, Team> entry in _teams)
        {
            if (_team.Color != entry.Value.Color)
            {
                Marker? match = entry.Value.Markers.FirstOrDefault(marker => marker.Pos.Global % game.LapSize == _team.StartSlot);
                if (match != null)
                {
                    return true;
                }

            }
        }
        return false;
    }

    private static Marker TryReachGoal(Marker[] markers)
    {
        foreach (Marker marker in markers)
        {
            // Relative offset between final 9 slots and marker position
            int offset = game.LapSize - marker.Pos.Local + game.GoalSlots;

            // Difference between dice roll and offset
            int difference = offset - dice.Value;

            // Check if dice value is within bounds
            if (difference >= 0)
            {
                // Calculate goal slot
                int slot = game.GoalSlots - difference;

                // Check if slot is within goal range
                if (slot >= 1 && slot <= 4)
                {
                    if (!_team.GoalSlotOccupied(slot))
                    {
                        marker.GoalSlot = slot;
                        return marker;
                    }
                }
            }
        }
        return null;
    }

    private static Marker TryMoveCloserToGoal(Marker[] markers)
    {
        foreach (Marker marker in markers)
        {
            // Relative offset between final 9 slots and marker position
            int offset = game.LapSize - marker.Pos.Local + game.GoalSlots;

            // Difference between dice roll and offset
            int difference = offset - dice.Value;

            // Check if dice value is within bounds
            if (difference >= 0)
            {
                // Calculate goal slot
                int slot = game.GoalSlots - difference;

                if (slot == 0)
                {
                    if (!_team.MoveSlotOccupied(marker.Pos.Local + dice.Value))
                    {
                        return marker;
                    }
                }
            }
        }
        return null;
    }


    private static void CheckIntersect(Marker marker)
    {
        foreach (KeyValuePair<int, Team> entry in _teams)
        {
            if (_team.Color != entry.Value.Color)
            {
                Marker targetMarket = _teams[entry.Key].Markers.FirstOrDefault(m => m.OnBoard() && (m.Pos.Global % game.LapSize) == (marker.Pos.Global % game.LapSize));
                if (targetMarket != null)
                {
                    statistics.EatCount++;
                    targetMarket.Reset();
                    LogMarkerAlert(MarkerAlert.Eat, marker);
                }
            }
        }
    }
}