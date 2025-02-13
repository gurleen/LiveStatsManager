using System.Text.Json.Serialization;
using Shared.Objects;

namespace NCAALiveStats.ExternalData.ESPN.Objects;

public class ESPNPlayerStats
{
    [JsonPropertyName("pagination")]
    public required ESPNPagination Pagination { get; set; }
    [JsonPropertyName("athletes")]
    public required List<ESPNPlayerStatsRecord> Records { get; set; }
}

public class ESPNPlayerStatsRecord
{
    [JsonPropertyName("athlete")]
    public required ESPNPlayerSummary Player { get; set; }
    [JsonPropertyName("categories")]
    public required List<ESPNPlayerStatsCategory> Categories { get; set; }

    public List<Tuple<Stat, double>> AllStats() => 
        Categories.SelectMany(c => c.MapValues()).ToList();
}

public static class ESPNPlayerStatsCategoryType
{
    public const string General = "general";
    public const string Offensive = "offensive";
    public const string Defensive = "defensive";
}

public class ESPNPlayerStatsCategory
{
    private readonly List<Stat> GeneralStats =
    [
        Stat.GamesPlayed, Stat.AvgMinutes, Stat.AvgFouls, Stat.TechnicalFouls, Stat.Ejections, Stat.DoubleDoubles,
        Stat.TripleDoubles, Stat.PER, Stat.Minutes, Stat.Rebounds, Stat.Fouls, Stat.ReboundsPerGame
    ];

    private readonly List<Stat> OffensiveStats = 
    [
        Stat.PointsPerGame, Stat.FGMadePerGame, Stat.FGAttemptedPerGame, Stat.FieldGoalPercentage, 
        Stat.ThreePointersMadePerGame, Stat.ThreePointersAttemptedPerGame, Stat.ThreePointPercentage,
        Stat.FreeThrowsAttempted, Stat.FreeThrowsAttempted, Stat.FreeThrowPercentage, Stat.AssistsPerGame,
        Stat.TurnoversPerGame, Stat.Points, Stat.FieldGoalsMade, Stat.FieldGoalsAttempted, 
        Stat.ThreePointersMade, Stat.ThreePointersAttempted, Stat.FreeThrowsMade, Stat.FreeThrowsAttempted,
        Stat.Assists, Stat.Turnovers
    ];
    
    private readonly List<Stat> DefensiveStats = 
    [
        Stat.BlocksPerGame, Stat.StealsPerGame, Stat.Blocks, Stat.Steals
    ];
    
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    [JsonPropertyName("displayName")]
    public required string DisplayName { get; set; }
    [JsonPropertyName("values")]
    public required List<double> Values { get; set; }
    
    private List<Stat> GetStatHeaders()
    {
        return Name switch
        {
            ESPNPlayerStatsCategoryType.General => GeneralStats,
            ESPNPlayerStatsCategoryType.Offensive => OffensiveStats,
            ESPNPlayerStatsCategoryType.Defensive => DefensiveStats,
            _ => []
        };
    }

    public List<Tuple<Stat, double>> MapValues()
    {
        var headers = GetStatHeaders();
        return headers.Zip(Values).Select(x => new Tuple<Stat, double>(x.First, x.Second)).ToList();
    }
}