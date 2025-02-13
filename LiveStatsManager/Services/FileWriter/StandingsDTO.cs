namespace LiveStatsManager.Services.FileWriterService;

public record StandingsDTO
{
    public required string Id { get; init; }
    public required string SchoolName { get; init; }
    public required string TeamName { get; init; }
    public required string Abbreviation { get; init; }
    public required string PrimaryColor { get; init; }
    public required string SecondaryColor { get; init; }
    public required string LogoUrl { get; init; }
    public required string WhiteLogoUrl { get; init; }
    public required int Wins { get; init; }
    public required int Losses { get; init; }
    public required string RecordDisplay { get; init; }
    public required float WinPercentage { get; init; }
    public required string WinPercentageDisplay { get; init; }
    public required int ConferenceWins { get; init; }
    public required int ConferenceLosses { get; init; }
    public required string ConferenceRecordDisplay { get; init; }
    public required float ConferenceWinPercentage { get; init; }
    public required string ConferenceWinPercentageDisplay { get; init; }
}