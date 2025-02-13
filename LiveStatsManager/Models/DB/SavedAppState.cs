using NCAALiveStats.Objects;

namespace LiveStatsManager.Objects.DB;

public class SavedAppState
{
    public required string HomeTeamId { get; set; }
    public required string AwayTeamId { get; set; }
    public required Sport Sport { get; set; }
}