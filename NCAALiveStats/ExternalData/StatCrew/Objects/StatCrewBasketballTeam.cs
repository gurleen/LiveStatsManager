using System;
using System.Xml.Linq;
using Shared.Extensions;

namespace NCAALiveStats.ExternalData.StatCrew.Objects;

public readonly record struct StatCrewBasketballLineScore
{
    public int Period { get; init; }
    public int Score { get; init; }

    public static StatCrewBasketballLineScore FromXml(XElement tag) => new()
    {
        Period = tag.GetIntAttr("prd"),
        Score = tag.GetIntAttr("score")
    };
}

public readonly record struct ShotStats
{
    public int Made { get; init; }
    public int Attempted { get; init; }
    public readonly double Percentage => Attempted == 0 ? 0 : (double)Made / Attempted;
    public readonly string PercentageDisplay => string.Format("{0:P1}", Percentage);
}

public readonly record struct ReboundStats
{
    public int Offensive { get; init; }
    public int Defensive { get; init; }
    public readonly int Total => Offensive + Defensive;
}

public readonly record struct FoulStats
{
    public int Personal { get; init; }
    public int Team { get; init; }
}

public readonly record struct SpecialStats
{
    public int PointsFromTurnovers { get; init; }
    public int PointsInPaint { get; init; }
    public int PointsFastBreak { get; init; }
    public int PointsFromBench { get; init; }
    public int Ties { get; init; }
    public int Leads { get; init; }
    public int TimeSpentLeading { get; init; }
    public int LargestLead { get; init; }

    public static SpecialStats FromXml(XElement tag) => new()
    {
        PointsFromTurnovers = tag.GetIntAttr("pts_to"),
        PointsInPaint = tag.GetIntAttr("pts_paint"),
        PointsFastBreak = tag.GetIntAttr("pts_fastb"),
        PointsFromBench = tag.GetIntAttr("pts_bench"),
        Ties = tag.GetIntAttr("ties"),
        Leads = tag.GetIntAttr("leads"),
        TimeSpentLeading = tag.GetIntAttr("lead_time"),
        LargestLead = tag.GetIntAttr("large_lead")
    };
}

public readonly record struct StatCrewBasketballStats
{
    public ShotStats FieldGoals { get; init; }
    public ShotStats ThreePointers { get; init; }
    public ShotStats FreeThrows { get; init; }
    public ReboundStats Rebounds { get; init; }
    public FoulStats Fouls { get; init; }
    public int Blocks { get; init; }
    public int Steals { get; init; }
    public int Assists { get; init; }
    public int Turnovers { get; init; }
    public int Minutes { get; init; }

    public static StatCrewBasketballStats FromXml(XElement tag) => new()
    {
        FieldGoals = new ShotStats
        {
            Made = tag.GetIntAttr("fgm"),
            Attempted = tag.GetIntAttr("fga")
        },
        ThreePointers = new ShotStats
        {
            Made = tag.GetIntAttr("fgm3"),
            Attempted = tag.GetIntAttr("fga3")
        },
        FreeThrows = new ShotStats
        {
            Made = tag.GetIntAttr("ftm"),
            Attempted = tag.GetIntAttr("fta")
        },
        Rebounds = new ReboundStats
        {
            Offensive = tag.GetIntAttr("oreb"),
            Defensive = tag.GetIntAttr("dreb")
        },
        Fouls = new FoulStats
        {
            Personal = tag.GetIntAttr("pf"),
            Team = tag.GetIntAttr("tf")
        },
        Blocks = tag.GetIntAttr("blk"),
        Steals = tag.GetIntAttr("stl"),
        Assists = tag.GetIntAttr("ast"),
        Turnovers = tag.GetIntAttr("to")
    };
}

public readonly record struct StatCrewBasketballTeamTotals
{
    public StatCrewBasketballStats Stats { get; init; }
    public SpecialStats Special { get; init; }
}
public readonly record struct StatCrewBasketballStatsByPeriod
{
    public int Period { get; init; }
    public StatCrewBasketballStats Stats { get; init; }

    public static StatCrewBasketballStatsByPeriod FromXml(XElement tag) => new()
    {
        Period = tag.GetIntAttr("prd"),
        Stats = StatCrewBasketballStats.FromXml(tag)
    };
}

public readonly record struct StatCrewBasketballPlayer
{
    public int JerseyNumber { get; init; }
    public int GamesPlayed { get; init; }
    public int GamesStarted { get; init; }
    public string Position { get; init; }
    public bool IsOnCourt { get; init; }
    public StatCrewBasketballStats Totals { get; init; }
    public List<StatCrewBasketballStatsByPeriod> PeriodStats { get; init; }

    public static StatCrewBasketballPlayer FromXml(XElement tag) => new()
    {
        JerseyNumber = tag.GetIntAttr("uni"),
        GamesPlayed = tag.GetIntAttr("gp"),
        GamesStarted = tag.GetIntAttr("gs"),
        Position = tag.GetStringAttr("pos"),
        IsOnCourt = tag.GetStringAttr("oncourt") == "Y",
        Totals = tag.Descendants("stats").Select(StatCrewBasketballStats.FromXml).First(),
        PeriodStats = tag.Descendants("statsbyprd").Select(StatCrewBasketballStatsByPeriod.FromXml).ToList()
    };
}

public readonly record struct StatCrewBasketballTeam
{
    public List<StatCrewBasketballLineScore> LineScore { get; init; }
    public StatCrewBasketballTeamTotals Totals { get; init; }
    public List<StatCrewBasketballStatsByPeriod> PeriodStats { get; init; }
    public List<StatCrewBasketballPlayer> Players { get; init; }

    public static StatCrewBasketballTeam FromXml(XElement tag) => new()
    {
        LineScore = tag.Descendants("linescore").Descendants("lineprd").Select(StatCrewBasketballLineScore.FromXml).ToList(),
        Totals = new()
        {
            Stats = tag.Descendants("totals").Descendants("stats").Select(StatCrewBasketballStats.FromXml).First(),
            Special = tag.Descendants("totals").Descendants("special").Select(SpecialStats.FromXml).First()
        },
        PeriodStats = tag.Descendants("totals").Descendants("statsbyprd").Select(StatCrewBasketballStatsByPeriod.FromXml).ToList(),
        Players = tag.Descendants("player").Select(StatCrewBasketballPlayer.FromXml).ToList()
    };
}
