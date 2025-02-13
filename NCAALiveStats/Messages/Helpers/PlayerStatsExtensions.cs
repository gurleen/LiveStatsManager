using Shared.Objects;

namespace NCAALiveStats.Messages.Helpers;

public static class PlayerStatsExtensions
{
    public static float GetStat(this PlayerStats stats, Stat stat) => stat switch
    {
        Stat.TechnicalFouls => stats.TechnicalFouls,
        Stat.Minutes => (float)stats.Minutes,
        Stat.Rebounds => stats.TotalRebounds,
        Stat.Fouls => stats.PersonalFouls,
        Stat.Points => stats.Points,
        Stat.FieldGoalsMade => stats.FieldGoalsMade,
        Stat.FieldGoalsAttempted => stats.FieldGoalsAttempted,
        Stat.ThreePointersMade => stats.ThreePointersMade,
        Stat.ThreePointersAttempted => stats.ThreePointersAttempted,
        Stat.FreeThrowsMade => stats.FreeThrowsMade,
        Stat.FreeThrowsAttempted => stats.FreeThrowsAttempted,
        Stat.Assists => stats.Assists,
        Stat.Turnovers => stats.Turnovers,
        Stat.Steals => stats.Steals,
        Stat.Blocks => stats.Blocks,
        _ => 0
    };
}