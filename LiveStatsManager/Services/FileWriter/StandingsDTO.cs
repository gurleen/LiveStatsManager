using LiveStatsManager.Models;
using Shared.Enums;

namespace LiveStatsManager.Services.FileWriter;

public record StandingsDTO
{
    public string Id { get; init; }
    public Sport Sport { get; init; }
    public string SchoolName { get; init; }
    public string TeamName { get; init; }
    public string Abbreviation { get; init; }
    public string PrimaryColor { get; init; }
    public string PrimaryTextColor { get; init; }
    public string SecondaryColor { get; init; }
    public string SecondaryTextColor { get; init; }
    public string LogoUrl { get; init; }
    public string WhiteLogoUrl { get; init; }
    public int Wins { get; init; }
    public int Losses { get; init; }
    public string RecordDisplay { get; init; }
    public double WinPercentage { get; init; }
    public string WinPercentageDisplay { get; init; }
    public int ConferenceWins { get; init; }
    public int ConferenceLosses { get; init; }
    public string ConferenceRecordDisplay { get; init; }
    public double ConferenceWinPercentage { get; init; }
    public string ConferenceWinPercentageDisplay { get; init; }

    public StandingsDTO(FullTeam team, Sport sport)
    {
        Id = team.Info.Id;
        Sport = sport;
        SchoolName = team.Info.SchoolName;
        TeamName = team.Info.TeamName;
        Abbreviation = team.Info.Abbreviation;
        PrimaryColor = team.Info.PrimaryColor;
        PrimaryTextColor = team.Info.PrimaryTextColor;
        SecondaryColor = team.Info.SecondaryColor;
        SecondaryTextColor = team.Info.SecondaryTextColor;
        LogoUrl = team.Info.FullLogoUrl;
        WhiteLogoUrl = team.Info.KnockoutLogoUrl;
        Wins = team.Stats.Overall.Wins;
        Losses = team.Stats.Overall.Losses;
        RecordDisplay = team.Stats.Overall.RecordDisplay;
        WinPercentage = team.Stats.Overall.WinPercentage;
        WinPercentageDisplay = team.Stats.Overall.WinPercentageDisplay;
        ConferenceWins = team.Stats.Conference.Wins;
        ConferenceLosses = team.Stats.Conference.Losses;
        ConferenceRecordDisplay = team.Stats.Conference.RecordDisplay;
        ConferenceWinPercentage = team.Stats.Conference.WinPercentage;
        ConferenceWinPercentageDisplay = team.Stats.Conference.WinPercentageDisplay;
    }
}