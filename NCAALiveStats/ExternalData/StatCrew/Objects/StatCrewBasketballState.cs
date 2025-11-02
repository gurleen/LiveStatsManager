using System;

namespace NCAALiveStats.ExternalData.StatCrew.Objects;

public readonly record struct StatCrewBasketballState
{
    public StatCrewBasketballVenue Venue { get; init; }
    public StatCrewBasketballTeam HomeTeam { get; init; }
    public StatCrewBasketballTeam AwayTeam { get; init; }
}
