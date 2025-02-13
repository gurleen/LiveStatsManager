using System.Text.Json.Serialization;

namespace NCAALiveStats.Messages;

public record ConnectionParameters
{
    public readonly string type = "parameters";

    public required string types { get; init; }
};