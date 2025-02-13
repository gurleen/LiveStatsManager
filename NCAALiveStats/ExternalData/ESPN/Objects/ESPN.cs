using System.Text.Json.Serialization;

namespace NCAALiveStats.ExternalData.ESPN.Objects;

public class ESPNPagination
{
    [JsonPropertyName("count")]
    public int Count { get; set; }
    [JsonPropertyName("limit")]
    public int Limit { get; set; }
    [JsonPropertyName("page")]
    public int Page { get; set; }
    [JsonPropertyName("pages")]
    public int Pages { get; set; }
    [JsonPropertyName("next")]
    public string? Next { get; set; }
}