using System.Text.Json.Serialization;

namespace NCAALiveStatsListener.Messages;

public record ConnectionParameters
{
    public readonly string type = "parameters";

    public required string types { get; init; }
};