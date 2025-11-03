using System.Text.Json.Serialization;

namespace NCAALiveStats.ExternalData.ESPN.Objects;

public class ESPNPlayerResponse
{
    [JsonPropertyName("items")]
    public required List<ESPNPlayer> Players { get; set; }
}

public class ESPNPlayerSummary
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }
    [JsonPropertyName("teamId")]
    public string? TeamId { get; set; }
    [JsonPropertyName("position")]
    public required ESPNPlayerPosition Position { get; set; }
}

public class ESPNPlayer
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; }
    [JsonPropertyName("lastName")]
    public string LastName { get; set; }
    [JsonPropertyName("displayName")]
    public required string DisplayName { get; set; }
    [JsonPropertyName("shortName")]
    public required string ShortName { get; set; }
    [JsonPropertyName("jersey")] 
    public string Jersey { get; set; } = string.Empty;
    [JsonPropertyName("experience")]
    public ESPNPlayerExperience? Experience { get; set; }
    [JsonPropertyName("height")]
    public double? Height { get; set; }
    [JsonPropertyName("displayHeight")]
    public string? DisplayHeight { get; set; }
    [JsonPropertyName("birthPlace")]
    public ESPNPlayerBirthPlace? BirthPlace { get; set; }
}

public class ESPNPlayerBirthPlace
{
    [JsonPropertyName("city")]
    public string City { get; set; }
    [JsonPropertyName("state")]
    public string? State { get; set; }
    [JsonPropertyName("country")]
    public string Country { get; set; }
}

public class ESPNPlayerPosition
{
    [JsonPropertyName("abbreviation")]
    public required string Abbreviation { get; set; }
    [JsonPropertyName("name")]
    public required string Name { get; set; }
}

public class ESPNPlayerExperience
{
    [JsonPropertyName("abbreviation")]
    public required string Abbreviation { get; set; }
    [JsonPropertyName("displayValue")]
    public required string DisplayValue { get; set; }
}