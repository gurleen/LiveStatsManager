using System;

namespace NCAALiveStats.ExternalData.StatCrew.Objects;

public enum PeriodType
{
    Halves,
    Quarters,
    Periods
}

public readonly record struct StatCrewBasketballRules
{
    public int Periods { get; init; }
    public int PeriodLength { get; init; }
    public int OvertimePeriodLength { get; init; }
    public int FoulLimit { get; init; }
    public PeriodType PeriodType { get; init; }
}

public readonly record struct StatCrewBasketballVenue
{
    public int GameId { get; init; }
    public string VisitorId { get; init; }
    public string VisitorName { get; init; }
    public string HomeId { get; init; }
    public string HomeName { get; init; }
    public DateTime StartTime { get; init; }
    public string Location { get; init; }
    public int Attendance { get; init; }
    public bool IsLeagueGame { get; init; }
    public bool IsNeutralGame { get; init; }
    public bool IsNightGame { get; init; }
    public bool IsPostseason { get; init; }
    public List<string> Officials { get; init; }
    public StatCrewBasketballRules Rules { get; init; }
}
