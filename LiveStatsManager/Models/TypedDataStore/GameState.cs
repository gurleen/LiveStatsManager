using Shared.Enums;
using Shared.Extensions;

namespace LiveStatsManager.Models.TypedDataStore;

public record struct TeamGameState
{
    public int Score { get; set; }
    public int Timeouts { get; set; }
    public int Fouls { get; set; }
    public bool Bonus { get; set; }
}

public record struct GameState
{
    public Sport Sport { get; set; }
    public int Clock { get; set; }
    public string ClockDisplay { get; set; }
    public int ShotClock { get; set; }
    public int Period { get; set; }
    public readonly string FullPeriodName => GetFullPeriodName();
    public readonly string PeriodDisplay => Period.DisplayWithSuffix();
    public required TeamGameState HomeTeam { get; set; }
    public required TeamGameState AwayTeam { get; set; }

    private readonly int PeriodLength => Sport switch
    { 
        Sport.MensBasketball => 20 * 60,
        Sport.WomensBasketball => 10 * 60,
        _ => 30
    };
    
    private readonly string PeriodName => Sport switch
    {
        Sport.MensBasketball => "Half",
        Sport.WomensBasketball => "Quarter",
        _ => "Period"
    };

    private readonly string CurrentPeriodName => Period.DisplayWithSuffix() + " " + PeriodName;

    private readonly string GetFullPeriodName()
    {
        if (Clock == 0)
            return "End of " + CurrentPeriodName;
        if (Clock == PeriodLength)
            return "Start of " + CurrentPeriodName;
        return CurrentPeriodName;
    }
}