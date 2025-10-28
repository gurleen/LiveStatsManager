using System;
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
    public int Clock { get; set; }
    private readonly TimeSpan ClockSpan => TimeSpan.FromSeconds(Clock);
    public readonly string ClockDisplay => ClockSpan.ToString(@"m\:ss");
    public int ShotClock { get; set; }
    public int Period { get; set; }
    public readonly string PeriodDisplay => Period.DisplayWithSuffix();
    public required TeamGameState HomeTeam { get; set; }
    public required TeamGameState AwayTeam { get; set; }
}