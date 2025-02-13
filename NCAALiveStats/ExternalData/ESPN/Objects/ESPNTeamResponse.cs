using System.Text.Json.Serialization;

namespace NCAALiveStats.ExternalData.ESPN.Objects;

public class ESPNTeamResponse
{
    [JsonPropertyName("sports")]
    public required List<ESPNSport> Sports { get; set; }
    
    [JsonIgnore]
    public IEnumerable<ESPNTeam> Teams => Sports
        .SelectMany(s => 
            s.Leagues.SelectMany(l => 
                l.Teams.Select(t => 
                    t.Team)));
}

public class ESPNSport
{
    [JsonPropertyName("leagues")]
    public required List<ESPNLeague> Leagues { get; set; }
}

public class ESPNLeague
{
    [JsonPropertyName("teams")]
    public required List<ESPNTeamWrapper> Teams { get; set; }
}

public class ESPNTeamWrapper
{
    [JsonPropertyName("team")]
    public required ESPNTeam Team { get; set; }
}

public class ESPNTeam
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }
    [JsonPropertyName("slug")]
    public string? Slug { get; set; }
    [JsonPropertyName("abbreviation")]
    public required string Abbreviation { get; set; }
    [JsonPropertyName("displayName")]
    public required string DisplayName { get; set; }
    [JsonPropertyName("shortDisplayName")]
    public required string ShortDisplayName { get; set; }
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    [JsonPropertyName("location")]
    public required string SchoolFullName { get; set; }
    [JsonPropertyName("color")]
    public string? PrimaryColor { get; set; }
    [JsonPropertyName("alternateColor")]
    public string? SecondaryColor { get; set; }
}