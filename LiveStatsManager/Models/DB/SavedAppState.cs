using Shared.Enums;

namespace LiveStatsManager.Models.DB;

public class SavedAppState
{
    public required string HomeTeamId { get; set; }
    public required string AwayTeamId { get; set; }
    public required Sport Sport { get; set; }
}