using System.Text.Json.Serialization;
using Shared.Enums;
using Shared.Extensions;

namespace NCAALiveStats.ExternalData.Sidearm;

public class SidearmRosterResponse
{
    [JsonPropertyName("players")]
    public required List<SidearmRosterPlayer> Players { get; set; }
}

public class SidearmRosterPlayer
{
    [JsonPropertyName("firstName")]
    public required string FirstName { get; set; }
    [JsonPropertyName("lastName")]
    public required string LastName { get; set; }
    [JsonPropertyName("jerseyNumber")]
    public required string JerseyNumber { get; set; }
    [JsonPropertyName("image")]
    public required SidearmRosterImage Image { get; set; }
    
    public string FullName => $"{FirstName} {LastName}";
    
    public string IdForSport(Sport sport) => sport switch
    {
        Sport.Wrestling => FullName.GenerateSlug(),
        _ => JerseyNumber
    };
}

public class SidearmRosterImage
{
    [JsonPropertyName("absoluteUrl")]
    public required string Url { get; set; }
}