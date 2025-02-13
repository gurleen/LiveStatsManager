using Shared.Enums;

namespace Shared.Objects;

public record LegacyPlayer(string name, string pos, string year, string jersey_num);

public class Player
{
    public required string Id { get; set; }
    public required string TeamId { get; set; }
    public required Sport Sport { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    public required string JerseyNumber { get; set; }
    public required string Position { get; set; }
    public required string Experience { get; set; }
    public required string Height { get; set; }
    public required string Hometown { get; set; }

    public LegacyPlayer AsLegacyPlayer() => new(FullName, Position, Experience, JerseyNumber);
}