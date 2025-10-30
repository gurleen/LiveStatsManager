using Shared.Enums;
using Shared.Extensions;
using Shared.Objects;

namespace Shared.GameState;

public class CurrentGameState
{
    private readonly SynchronizationContext _syncContext = SynchronizationContext.Current 
                                                           ?? new SynchronizationContext();
    public event Func<Task> OnUpdate;
    
    public Sport Sport { get; set; } = Sport.MensBasketball;
    public int Period { get; set; } = 1;
    public int TimeRemaining { get; set; } = 20 * 60;
    public int ShotClock { get; set; } = 30;
    public int HomeScore { get; set; }
    public int AwayScore { get; set; }
    
    public Dictionary<string, PlayerStats> PlayerStats { get; set; } = new();
    public Dictionary<Stat, double> HomeStats { get; set; } = new();
    public Dictionary<Stat, double> AwayStats { get; set; } = new();
    
    public string Clock => $"{TimeRemaining / 60}:{TimeRemaining % 60:00}";
    public string FullPeriodName => GetFullPeriodName();

    public void NotifyUpdate()
    {
        _syncContext.Post(_ => OnUpdate?.Invoke(), null);
    }
    
    private int PeriodLength => Sport switch
    { 
        Sport.MensBasketball => 20 * 60,
        Sport.WomensBasketball => 10 * 60,
        _ => 30
    };
    
    private string PeriodName => Sport switch
    {
        Sport.MensBasketball => "Half",
        Sport.WomensBasketball => "Quarter",
        _ => "Period"
    };

    private string CurrentPeriodName => Period.DisplayWithSuffix() + " " + PeriodName;

    private string GetFullPeriodName()
    {
        if (TimeRemaining == 0)
            return "End of " + CurrentPeriodName;
        if (TimeRemaining == PeriodLength)
            return "Start of " + CurrentPeriodName;
        return CurrentPeriodName;
    }
}