using System.Globalization;
using AngleSharp.Text;
using Flurl.Http;
using LiveStatsManager.FileWatcher;
using Shared.Enums;
using Shared.Extensions;

namespace LiveStatsManager.Services.AllSport;

public class AllSportData
{
    private readonly string DataLine;
    public string Clock { get; init; }
    public string ClockDisplay => Clock.TrimStart('0');
    public string ShotClock { get; init; }
    public string HomeScore { get; init; }
    public string AwayScore { get; init; }
    public string Period { get; set; }
    public int ClockSeconds
    {
        get
        {
            try
            {
                if (Clock.Contains(":"))
                {
                    var clockSplit = Clock.Split(":");
                    var minutes = clockSplit[0].SafeParseInt();
                    var seconds = clockSplit[1].SafeParseInt();
                    return (60 * minutes) + seconds;
                }
                else
                {
                    var clockSplit = Clock.Split(".");
                    var seconds = clockSplit[0].SafeParseInt();
                    return seconds;
                }
            }
            catch(Exception)
            {
                return 0;
            }
        }
    }
    public int ShotClockSeconds => ShotClock.SafeParseInt();
    public int HomeScoreInt => HomeScore.SafeParseInt();
    public int AwayScoreInt => AwayScore.SafeParseInt();
    public int PeriodInt => Period[..1].SafeParseInt();
    
    private string GetRange(int start, int end) => 
        DataLine.Substring(start, end - start).StripLeadingTrailingSpaces();

    public AllSportData(string line, Sport sport = Sport.MensBasketball)
    {
        DataLine = line;
        Clock = GetRange(1, 8);
        if(Clock.StartsWith("0:"))
            Clock = Clock[2..];
        else if(Clock.StartsWith('0') && !Clock.StartsWith("0."))
            Clock = Clock[1..];
        ShotClock = GetRange(9, 11);
        HomeScore = GetRange(13, 16);
        AwayScore = GetRange(16, 19);
        Period = PeriodText(sport);
    }

    private string PeriodText(Sport sport)
    {
        var regulationPeriods = sport.NumPeriods();
        var period = int.Parse(GetRange(29, 30));
        var periodsPastReg = period - regulationPeriods;
        return periodsPastReg switch
        {
            < 1 => period.DisplayWithSuffix(),
            1 => "OT",
            > 1 => $"OT{periodsPastReg}"
        };
    }

    public IEnumerable<DataPair> DataPairs() =>
    [
        new("Clock", Clock),
        new("Shot-Clock", ShotClock),
        new("Period", Period)
    ];
}