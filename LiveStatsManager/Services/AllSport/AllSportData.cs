using AngleSharp.Text;
using Shared.Extensions;

namespace Shared.Objects;

public class AllSportData
{
    private readonly string DataLine;
    public string Clock { get; init; }
    public string ShotClock { get; init; }
    public string HomeScore { get; init; }
    public string AwayScore { get; init; }
    public string Period { get; set; }
    
    private string GetRange(int start, int end) => 
        DataLine.Substring(start, end - start).StripLeadingTrailingSpaces();

    public AllSportData(string line)
    {
        DataLine = line;
        Clock = GetRange(1, 8);
        if(Clock.StartsWith("0:"))
            Clock = Clock[2..];
        ShotClock = GetRange(9, 11);
        HomeScore = GetRange(13, 16);
        AwayScore = GetRange(16, 19);
        Period = int.Parse(GetRange(29, 30)).DisplayWithSuffix();
    }

    public IEnumerable<DataPair> DataPairs() =>
    [
        new("Clock", Clock),
        new("Shot-Clock", ShotClock),
        new("fade:Home-Score", HomeScore),
        new("fade:Away-Score", AwayScore),
        new("Period", Period)
    ];
}