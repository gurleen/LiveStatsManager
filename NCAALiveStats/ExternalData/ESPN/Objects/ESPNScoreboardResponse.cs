using System.Text.Json.Serialization;

namespace NCAALiveStats.ExternalData.ESPN.Objects;

public class ESPNScoreboardResponse
{
    [JsonPropertyName("events")]
    public List<ESPNEvent> Events { get; set; }
}

public class ESPNEvent
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("competitions")]
    public List<ESPNCompetition> Competitions { get; set; }

    public ESPNCompetition Competition => Competitions.First();
}

public class ESPNCompetition
{
    [JsonPropertyName("competitors")]
    public List<ESPNCompetitor> Competitors { get; set; }
    
    [JsonPropertyName("status")]
    public ESPNEventStatus Status { get; set; }
}

public class ESPNCompetitor
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("homeAway")]
    public string HomeAway { get; set; }
    
    [JsonPropertyName("score")]
    public string Score { get; set; }
}

public class ESPNEventStatus
{
    [JsonPropertyName("displayClock")]
    public string ClockDisplay { get; set; }
    
    [JsonPropertyName("type")]
    public ESPNEventStatusType Type { get; set; }
}

public class ESPNEventStatusType
{
    [JsonPropertyName("shortDetail")]
    public string ShortDetail { get; set; }
}