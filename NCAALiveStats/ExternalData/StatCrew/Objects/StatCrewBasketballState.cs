using System;
using Shared.Enums;

namespace NCAALiveStats.ExternalData.StatCrew.Objects;

public record struct StatCrewBasketballState
{
    public StatCrewBasketballVenue Venue { get; init; }
    public StatCrewBasketballTeam HomeTeam { get; init; }
    public StatCrewBasketballTeam AwayTeam { get; init; }
    public List<StatCrewBasketballPlay> Plays { get; init; }

    private int? _homeTimeoutsRemaining;
    public int HomeTimeoutsRemaining => _homeTimeoutsRemaining ??= GetTimeoutsRemaining(TeamSide.Home);

    private int? _awayTimeoutsRemaining;
    public int AwayTimeoutsRemaining => _awayTimeoutsRemaining ??= GetTimeoutsRemaining(TeamSide.Away);

    private int GetTimeoutsRemaining(TeamSide side)
    {
        return Venue.Sport == Sport.WomensBasketball ? GetTimeoutsRemainingWomens(side) : GetTimeoutsRemainingMens(side);
    }

    private int? _homeFouls;
    public int HomeFouls => _homeFouls ??= GetCurrentPeriodTeamFouls(TeamSide.Home);

    private int? _awayFouls;
    public int AwayFouls => _awayFouls ??= GetCurrentPeriodTeamFouls(TeamSide.Away);

    private int GetTimeoutsTakenByHalf(TeamSide side, bool firstHalf)
    {
        if(Venue.Sport == Sport.WomensBasketball)
        {
            if(firstHalf) return Plays.Where(x => x.IsTeamTakenTimeout() && x.TeamSide == side && x.Period < 3).Count();
            else          return Plays.Where(x => x.IsTeamTakenTimeout() && x.TeamSide == side && x.Period > 2).Count();
        }
        else
        {
            if(firstHalf) return Plays.Where(x => x.IsTeamTakenTimeout() && x.TeamSide == side && x.Period == 1).Count();
            else          return Plays.Where(x => x.IsTeamTakenTimeout() && x.TeamSide == side && x.Period == 2).Count();
        }
    }

    private int GetTimeoutsRemainingMens(TeamSide side)
    {
        var firstHalfTimeouts = GetTimeoutsTakenByHalf(side, true);
        var secondHalfTimeouts = GetTimeoutsTakenByHalf(side, false);
        return Math.Min(4 - firstHalfTimeouts, 3) - secondHalfTimeouts;
    }

    private int GetTimeoutsRemainingWomens(TeamSide side)
    {
        return 4 - GetTimeoutsTakenByHalf(side, true) - GetTimeoutsTakenByHalf(side, false);
    }

    public int GetCurrentPeriodTeamFouls(TeamSide side)
    {
        var currentPeriod = Plays.Select(x => x.Period).Max();
        return Plays.Count(x => x.Period == currentPeriod && x.TeamSide == side && x.Action == StatCrewBasketballAction.FOUL);
    }
}
