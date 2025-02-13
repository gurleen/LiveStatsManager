using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration.Attributes;

namespace Shared.Objects;

public record LocalTeamInfo
{
    [Name("team_id")]
    public required int EspnId { get; init; }
    [Name("abbreviation")]
    public required string Abbreviation { get; init; }
    [Name("display_name")]
    public required string SchoolName { get; init; }
    [Name("short_name")]
    public required string ShortName { get; init; }
    [Name("mascot")]
    public required string Mascot { get; init; }
    [Name("nickname")]
    public required string Nickname { get; init; }
    [Name("team")]
    public required string Team { get; init; }
    [Name("color")]
    public required string Color { get; init; }
    [Name("alternate_color")]
    public required string AlternateColor { get; init; }
    [Name("group_id")]
    public required int GroupId { get; init; }
    [Name("conference_short_name")]
    public required string ConferenceShortName { get; init; }
    [Name("conference_name")]
    public required string ConferenceName { get; init; }
    [Name("conference_id")]
    public required int ConferenceId { get; init; }
    [Name("logo_name")]
    public required string LogoName { get; init; }
    [Name("website")]
    public required string Website { get; init; }
}

public class LocalTeamData
{
    public readonly List<LocalTeamInfo> Teams = LoadTeams();
    private readonly List<LocalTeamInfo> _customTeams = LoadCustomTeams();
    public IEnumerable<Team> CustomTeams => _customTeams.Select(Team.CustomTeam);
    private const string _teamDataPath = "espn_teams.csv";
    private const string _customTeamsPath = "custom_teams.csv";


    private static List<LocalTeamInfo> LoadTeams()
    {
        using var reader = new StreamReader(_teamDataPath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        return csv.GetRecords<LocalTeamInfo>().ToList();
    }
    
    private static List<LocalTeamInfo> LoadCustomTeams()
    {
        try
        {
            using var reader = new StreamReader(_customTeamsPath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<LocalTeamInfo>().ToList();
        }
        catch (Exception e)
        {
            return [];
        }
    }
}