using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NCAALiveStats.ExternalData.ESPN.Objects;
using Shared.Enums;

namespace NCAADataLoader.Models;

public class Team
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int TeamId { get; set; }
    public Sport Sport { get; init; }
    public required string SchoolName { get; init; }
    public required string TeamName { get; init; }
    public required string Abbreviation { get; init; }
    public required string TeamLogoFileName { get; init; }
    public required string PrimaryColor { get; init; }
    public required string SecondaryColor { get; init; }
    public required string Website { get; init; }
}

public static class ESPNTeamExtensions
{
    public static Team ToDbTeam(this ESPNTeam response)
    {
        return new Team
        {
            TeamId = int.Parse(response.Id),
            SchoolName = response.ShortDisplayName,
            TeamName = response.Name,
            Abbreviation = response.Abbreviation,
            PrimaryColor = response.PrimaryColor ?? string.Empty,
            SecondaryColor = response.SecondaryColor ?? string.Empty,
            TeamLogoFileName = response.Slug ?? string.Empty,
            Website = string.Empty
        };
    }
}