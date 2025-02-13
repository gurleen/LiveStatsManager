namespace Shared.Objects;

public record GameStatus
{
    public required string Id { get; init; }
    public required Team HomeTeam { get; init; }
    public required string HomeScore { get; init; }
    public required Team AwayTeam { get; init; }
    public required string AwayScore { get; init; }
    public required string ClockDisplay { get; init; }
    public required string Status { get; init; }
}