using System;
using System.Xml.Linq;
using NCAALiveStats.ExternalData.StatCrew.Objects;
using Shared.Extensions;

namespace NCAALiveStats.ExternalData.StatCrew;

public class StatCrewBasketballParser(string filePath)
{
    private readonly string FilePath = filePath;
    private Stream DocumentFileStream => File.OpenRead(FilePath);
    private async Task<XElement> GetDocument() => await XElement.LoadAsync(DocumentFileStream, LoadOptions.None, CancellationToken.None);

    public async Task<StatCrewBasketballState> Load()
    {
        var document = await GetDocument();
        return new StatCrewBasketballState
        {
            Venue = ParseVenue(document),
            HomeTeam = StatCrewBasketballTeam.FromXml(document.Descendants("team").Where(x => x.GetStringAttr("vh") == "H").First()),
            AwayTeam = StatCrewBasketballTeam.FromXml(document.Descendants("team").Where(x => x.GetStringAttr("vh") == "V").First())
        };
    }

    private static bool ParseBool(string boolStr) => boolStr == "Y";
    public static PeriodType ParsePeriodType(string periodTypeStr) => periodTypeStr switch
    {
        "H" => PeriodType.Halves,
        "Q" => PeriodType.Quarters,
        _ => PeriodType.Periods
    };

    private static StatCrewBasketballVenue ParseVenue(XElement document)
    {
        var tag = document.Descendants("venue").First();
        var officialsTag = tag.Descendants("officials").First();
        var rulesTag = tag.Descendants("rules").First();
        var fullDtString = tag.GetStringAttr("date") + " " + tag.GetStringAttr("start");
        return new StatCrewBasketballVenue
        {
            GameId = tag.GetIntAttr("gameid"),
            VisitorId = tag.GetStringAttr("visid"),
            VisitorName = tag.GetStringAttr("visname"),
            HomeId = tag.GetStringAttr("homeid"),
            HomeName = tag.GetStringAttr("homename"),
            Location = tag.GetStringAttr("location"),
            Attendance = tag.GetIntAttr("attend"),
            IsLeagueGame = ParseBool(tag.GetStringAttr("leaguegame")),
            IsNeutralGame = ParseBool(tag.GetStringAttr("neutralgame")),
            IsNightGame = ParseBool(tag.GetStringAttr("nitegame")),
            IsPostseason = ParseBool(tag.GetStringAttr("postseason")),
            Officials = officialsTag.GetStringAttr("text").Split(", ").ToList(),
            Rules = new StatCrewBasketballRules
            {
                Periods = rulesTag.GetIntAttr("prds"),
                PeriodLength = rulesTag.GetIntAttr("minutes"),
                OvertimePeriodLength = rulesTag.GetIntAttr("minutesot"),
                FoulLimit = rulesTag.GetIntAttr("fouls"),
                PeriodType = ParsePeriodType(rulesTag.GetStringAttr("qh"))
            },
            StartTime = DateTime.Parse(fullDtString).ToLocalTime()
        };
    }
}
