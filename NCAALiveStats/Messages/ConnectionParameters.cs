using System.Text.Json.Serialization;

namespace NCAALiveStats.Messages;

public record ConnectionParameters
{
    [JsonPropertyName("type")]
    public string MessageType => "parameters";

    [JsonPropertyName("types")]
    public required string Types { get; init; }
    
    [JsonPropertyName("playbyplayOnConnect")]
    public int PlayByPlayOnConnect { get; init; } = 1;
};