using NCAALiveStats.Objects;

namespace LiveStatsManager.Objects;

public class FullTeam
{
    public required TeamInfo Info { get; init; }
    public required TeamStats Stats { get; init; }
    public required List<Player> Players { get; init; }
}