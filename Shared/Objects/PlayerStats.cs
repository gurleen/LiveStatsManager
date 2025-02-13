using System.Globalization;
using Shared.Enums;

namespace Shared.Objects;

public class PlayerStats(string playerId, string teamId, Sport sport)
{
    public string PlayerId { get; init; } = playerId;
    private string TeamId { get; init; } = teamId;
    public Sport Sport { get; init; } = sport;
    public Dictionary<Stat, double> Stats { get; init; } = new();
    
    public double GetStat(Stat stat) => Math.Round(Stats.GetValueOrDefault(stat, 0), 1);

    public string GetStatDisplay(Stat stat, int roundTo = 1) =>
        Math.Round(GetStat(stat), roundTo).ToString(CultureInfo.InvariantCulture);
    public void SetStat(Stat stat, double value) => Stats[stat] = value;
}