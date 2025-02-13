namespace LiveStatsManager.Objects;

public class GraphicsData(FullTeam homeTeam, FullTeam awayTeam)
{
    public FullTeam HomeTeam { get; set; } = homeTeam;
    public FullTeam AwayTeam { get; set; } = awayTeam;
    
    public Dictionary<string, string> TemplateData()
    {
        return new Dictionary<string, string>
        {
            ["Home-Abbr"] = HomeTeam.Info.Abbreviation,
            ["Home-School-Name"] = HomeTeam.Info.SchoolName,
            ["Home-Team-Name"] = HomeTeam.Info.TeamName,
            ["color:Home-Color"] = HomeTeam.Info.PrimaryColor,
            ["img:Home-Logo"] = HomeTeam.Info.KnockoutLogoUrl,
            ["Away-Abbr"] = AwayTeam.Info.Abbreviation,
            ["Away-School-Name"] = AwayTeam.Info.SchoolName,
            ["Away-Team-Name"] = AwayTeam.Info.TeamName,
            ["color:Away-Color"] = AwayTeam.Info.PrimaryColor,
            ["img:Away-Logo"] = AwayTeam.Info.KnockoutLogoUrl
        };
    }
}