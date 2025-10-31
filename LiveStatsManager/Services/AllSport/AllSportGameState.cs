using Shared.GameState;

namespace LiveStatsManager.Services.AllSport;

public static class AllSportGameState
{
    public static int ParseClockStringToSeconds(this string clock)
    {
        var parts = clock.Split(':');
        return int.Parse(parts[0]) * 60 + int.Parse(parts[1]);
    }
    
    public static void UpdateAllSportData(this CurrentGameState gameState, AllSportData data)
    {
        gameState.Period = int.Parse(data.Period.Substring(0, 1));
        gameState.TimeRemaining = data.Clock.ParseClockStringToSeconds();
        gameState.ShotClock = data.ShotClock.ParseClockStringToSeconds();
        gameState.HomeScore = int.Parse(data.HomeScore);
        gameState.AwayScore = int.Parse(data.AwayScore);
        gameState.NotifyUpdate();
    }
}