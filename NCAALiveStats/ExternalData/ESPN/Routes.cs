using Shared.Enums;

namespace NCAALiveStats.ExternalData.ESPN;

public static class Routes
{
    public const string TeamsUrl = "teams";
    private static string SportSlug(Sport sport) => sport == Sport.MensBasketball ? "mens-college-basketball" : "womens-college-basketball";
    public static string PlayersUrl(Sport sport) =>
        $"https://sports.core.api.espn.com/v3/sports/basketball/{SportSlug(sport)}/athletes";
    public static string TeamRosterUrl(int teamId) => $"teams/{teamId}/roster";
    public const string PlayerStatsUrl = "statistics/byathlete";
    public const string StandingsUrl = "standings";
    public const string ScoreboardUrl = "scoreboard";
}