using Shared.Enums;
using Shared.Utilities;

namespace Shared.Objects;

public enum TeamGender
{
    Men,
    Women
}

public record ConferenceInfo
{
    public required string Name { get; init; }
    public required string Abbreviation { get; init; }
    public required int EspnId { get; init; }
}

public enum CoachType
{
    HeadCoach,
    AssistantCoach,
    AssociateHeadCoach,
    DirectorOfBasketballOperations,
    GraduateAssistant,
    StrengthAndConditioningCoach,
    AthleticTrainer,
    VideoCoordinator,
    Manager,
}

public record CoachInfo
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required CoachType Type { get; init; }
    public string FullName => $"{FirstName} {LastName}";
}

public record TeamInfo
{
    public required string Id { get; init; }
    public required string SchoolName { get; init; }
    public required string TeamName { get; init; }
    public required string Abbreviation { get; init; }
    public required string TeamLogo { get; init; }
    public required string PrimaryColor { get; init; }
    public required string SecondaryColor { get; init; }
    public string PrimaryTextColor => ColorUtils.GetBestTextColor(PrimaryColor);
    public string SecondaryTextColor => ColorUtils.GetBestTextColor(SecondaryColor);
    public required ConferenceInfo Conference { get; init; }
    public required string Website { get; init; }
    public string FullLogoUrl => $"https://images.dragonstv.io/logos/{TeamLogo}";
    public string KnockoutLogoUrl => $"https://images.dragonstv.io/logos-knockout/{TeamLogo}";

    public static TeamInfo CustomTeamInfo(LocalTeamInfo local) =>
        new()
        {
            Id = local.EspnId.ToString(),
            SchoolName = local.ShortName,
            TeamName = local.Mascot,
            Abbreviation = local.Abbreviation,
            TeamLogo = local.LogoName,
            PrimaryColor = "#" + local.Color,
            SecondaryColor = "#" + local.AlternateColor,
            Conference = new ConferenceInfo
            {
                Name = local.ConferenceName,
                Abbreviation = local.ConferenceShortName,
                EspnId = local.ConferenceId
            },
            Website = local.Website
        };
}

public class TeamRecord(int wins, int losses)
{
    public int Wins { get; init; } = wins;
    public int Losses { get; init; } = losses;
    public int Games => Wins + Losses;
    public double WinPercentage => Math.Round((double)Wins / Games, 3);
    public string RecordDisplay => $"{Wins}-{Losses}";
    public string WinPercentageDisplay => $"{WinPercentage:P1}";
}

public class TeamStats
{
    public required string Id { get; set; }
    public required Sport Sport { get; set; }
    public required TeamRecord Overall { get; init; }
    public required TeamRecord Conference { get; init; }
    public required TeamRecord Home { get; init; }
    public TeamRecord Away => new(Overall.Wins - Home.Wins, Overall.Losses - Home.Losses);

    public static TeamStats Empty()
    {
        return new TeamStats
        {
            Overall = new TeamRecord(0, 0),
            Conference = new TeamRecord(0, 0),
            Home = new TeamRecord(0, 0),
            Sport = Sport.Wrestling,
            Id = string.Empty
        };
    }
}

public class Team
{
    public string Id => Info.Id;
    public string Name => Info.SchoolName + " " + Info.TeamName;
    public required TeamInfo Info { get; init; }

    public static Team CustomTeam(LocalTeamInfo local)
    {
        return new Team
        {
            Info = TeamInfo.CustomTeamInfo(local)
        };
    }
}