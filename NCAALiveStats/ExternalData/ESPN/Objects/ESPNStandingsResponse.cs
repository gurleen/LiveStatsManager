using System.Text.Json.Serialization;

namespace NCAALiveStats.ExternalData.ESPN.Objects;

public class ESPNStandingsResponse
{
    [JsonPropertyName("children")]
    public required List<ESPNConferenceStandings> Conferences { get; set; }
}

public class ESPNConferenceStandings
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    [JsonPropertyName("standings")]
    public required ESPNConferenceStandingsWrapper _standings { private get; set; }
    [JsonIgnore]
    public List<ESPNConferenceStandingsEntry> Teams => _standings.Standings;
}

public class ESPNConferenceStandingsWrapper
{
    [JsonPropertyName("entries")]
    public required List<ESPNConferenceStandingsEntry> Standings { get; set; }
}

public class ESPNConferenceStandingsEntry
{
    [JsonPropertyName("team")]
    public required ESPNTeam Team { get; set; }
    [JsonPropertyName("stats")]
    public required List<ESPNTeamStat> Stats { get; set; }
    
    public double GetEntry(string statName) => 
        Stats.Find(s => s.StatType == statName)?.Value ?? 0.0;
}

public class ESPNTeamStat
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    [JsonPropertyName("displayName")]
    public string? DisplayName { get; set; }
    [JsonPropertyName("type")]
    public required string StatType { get; set; }
    [JsonPropertyName("abbreviation")]
    public required string Abbreviation { get; set; }
    [JsonPropertyName("value")] 
    public double Value { get; set; } = 0.0;
}

public static class ESPNTeamStatType
{
    public const string Wins = "wins";
    public const string Losses = "losses";
    public const string HomeWins = "home_wins";
    public const string HomeLosses = "home_losses";
    public const string ConferenceWins = "vsconf_wins";
    public const string ConferenceLosses = "vsconf_losses";
}