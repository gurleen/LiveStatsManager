using System;
using MudBlazor;
using NCAALiveStats;

namespace LiveStatsManager.Models.TypedDataStore;

public record struct Wrestler
{
    public bool IsRanked { get; set; }
    public int Ranking { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public record struct WrestlingScorebugState
{
    public string WeightClass { get; set; }
    public int Clock { get; set; }
    public int Period { get; set; }
    public int AdvantageTime { get; set; }
    public TeamSide AdvantageSide { get; set; }
    public Wrestler HomeWrestler { get; set; }
    public Wrestler AwayWrestler { get; set; }
    public int HomeScore { get; set; }
    public int AwayScore { get; set; }

    public static WrestlingScorebugState Default()
    {
        return new WrestlingScorebugState
        {
            WeightClass = "125 LBS",
            Clock = 60 * 3,
            Period = 1,
            AdvantageTime = 0,
            AdvantageSide = TeamSide.Home,
            HomeWrestler = new Wrestler { FirstName = "JOHN", LastName = "DOE", IsRanked = false, Ranking = 0 },
            AwayWrestler = new Wrestler { FirstName = "JOEY", LastName = "APPLESEED", IsRanked = false, Ranking = 0 },
            HomeScore = 0,
            AwayScore = 0
        };
    }
}
