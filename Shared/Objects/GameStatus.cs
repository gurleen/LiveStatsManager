namespace Shared.Objects;

public record Scoreboard
{
    public required string Id { get; init; }
    public required Team HomeTeam { get; init; }
    public required Team AwayTeam { get; init; }
    public required string ClockDisplay { get; init; }
    public required string Status { get; init; }
}