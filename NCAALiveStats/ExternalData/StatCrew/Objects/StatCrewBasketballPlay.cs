using System;
using System.Xml.Linq;
using AngleSharp.Text;
using Microsoft.Extensions.ObjectPool;
using Shared.Extensions;

namespace NCAALiveStats.ExternalData.StatCrew.Objects;

public enum StatCrewBasketballAction
{
    UNKNOWN,
    ASSIST,
    BLOCK,
    FOUL,
    GOOD,
    MISS,
    REBOUND,
    STEAL,
    SUB,
    TIMEOUT,
    TURNOVER
}

public enum StatCrewBasketballActionType
{
    UNKNOWN,
    THREE_POINTER,
    DUNK,
    FREE_THROW,
    JUMPER,
    LAYUP,
    TIP_IN,
    DEAD_BALL,
    DEFENSE,
    OFFENSE,
    IN,
    OUT,
    THIRTY_SECOND_TIMEOUT,
    MEDIA_TIMEOUT,
    TEAM_TIMEOUT
}

public enum StatCrewBasketballPlaySide { LEFT, RIGHT }

public readonly record struct StatCrewBasketballPlay
{
    private static readonly Dictionary<string, string> ActionTypeReplacements = new()
    {
        {"3PTR", "THREE_POINTER"},
        {"FT", "FREE_THROW"},
        {"TIPIN", "TIP_IN"},
        {"DEADB", "DEAD_BALL"},
        {"DEF", "DEFENSE"},
        {"OFF", "OFFENSE"},
        {"30SEC", "THIRTY_SECOND_TIMEOUT"},
        {"MEDIA", "MEDIA_TIMEOUT"},
        {"TEAM", "TEAM_TIMEOUT"}
    };
    public readonly TeamSide TeamSide { get; init; }
    public readonly int JerseyNumber { get; init; }
    public readonly int Period { get; init; }
    public readonly int Time { get; init; }
    public readonly string ClockDisplay
    {
        get
        {
            var span = TimeSpan.FromSeconds(Time);
            return $"{span.Minutes}:{span.Seconds:D2}";
        }
    }
    public readonly StatCrewBasketballAction Action { get; init; }
    public readonly string ActionDisplay => Action.ToString();
    public readonly StatCrewBasketballActionType ActionType { get; init; }
    public readonly string ActionTypeDisplay => ActionType.ToString();
    public readonly StatCrewBasketballPlaySide PlaySide { get; init; }

    public bool IsTeamTakenTimeout()
    {
        return Action == StatCrewBasketballAction.TIMEOUT &&
        (ActionType == StatCrewBasketballActionType.THIRTY_SECOND_TIMEOUT || ActionType == StatCrewBasketballActionType.TEAM_TIMEOUT);
    }

    private static int GetJerseyNumber(XElement tag)
    {
        if (tag.GetStringAttr("uni") == "TM") return -1;
        else return tag.GetIntAttr("uni");
    }

    private static int GetTime(XElement tag)
    {
        try
        {
            var time = tag.GetStringAttr("time").Split(":");
            var minutes = int.Parse(time[0]);
            var seconds = int.Parse(time[1]);
            return (minutes * 60) + seconds;
        }
        catch
        {
            return 0;
        }
    }

    private static StatCrewBasketballAction GetAction(XElement tag)
    {
        var action = tag.GetStringAttr("action", "UNKNOWN");
        if (Enum.TryParse(typeof(StatCrewBasketballAction), action, true, out var result))
        {
            return (StatCrewBasketballAction)result;
        }
        return StatCrewBasketballAction.UNKNOWN;
    }

    private static StatCrewBasketballActionType GetActionType(XElement tag)
    {
        var actionType = tag.GetStringAttr("type", "UNKNOWN");
        if(ActionTypeReplacements.TryGetValue(actionType, out var newValue))
        {
            actionType = newValue;
        }
        if (Enum.TryParse(typeof(StatCrewBasketballActionType), actionType, true, out var result))
        {
            return (StatCrewBasketballActionType)result;
        }
        return StatCrewBasketballActionType.UNKNOWN;
    }

    public static StatCrewBasketballPlay FromXml(XElement tag, int period) => new()
    {
        Period = period,
        TeamSide = tag.GetStringAttr("vh") == "H" ? TeamSide.Home : TeamSide.Away,
        JerseyNumber = GetJerseyNumber(tag),
        Time = GetTime(tag),
        Action = GetAction(tag),
        ActionType = GetActionType(tag),
        PlaySide = tag.GetStringAttr("side") == "left" ? StatCrewBasketballPlaySide.LEFT : StatCrewBasketballPlaySide.RIGHT
    };
}