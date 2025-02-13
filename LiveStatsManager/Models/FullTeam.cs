using Shared.Objects;

namespace LiveStatsManager.Models;

public class FullTeam
{
    public required TeamInfo Info { get; init; }
    public required TeamStats Stats { get; init; }
    public required List<Player> Players { get; init; }
}