using System.Net.Mime;
using LiveStatsManager.Models;
using LiveStatsManager.Services;
using Microsoft.AspNetCore.Mvc;
using NCAALiveStats.ExternalData;
using NCAALiveStats.ExternalData.Sidearm;
using Optional;
using Shared.Objects;

namespace LiveStatsManager.Controllers;

[ApiController]
public class TeamDataController(TeamDataRepository teamRepo, AppState state) : ControllerBase
{
    [HttpGet("/api/teams")]
    public List<Team> Teams() => teamRepo.Teams;
    
    [HttpGet("/api/teams/{teamId}")]
    public FullTeam Team(string teamId) => teamRepo.GetFullTeam(state.Sport, teamId);
    
    [HttpGet("/api/teams/{teamId}/players")]
    public IEnumerable<Player> TeamPlayers(string teamId) => teamRepo.Players.Where(p => p.TeamId == teamId);
    
    [HttpGet("/api/players")]
    public List<Player> Players() => teamRepo.Players;

    [HttpGet("/api/players/{playerId:int}")]
    public Option<Player> Player(int playerId) => teamRepo.GetPlayer(playerId);

    [HttpGet("/api/players/{playerId:int}/lines")]
    public List<PlayerStats> PlayerLines(int playerId) => teamRepo.GetPlayerLines(playerId);
    
    [HttpGet("/api/lines")]
    public List<PlayerStats> Lines() => teamRepo.StatLines;

    [HttpGet("/api/teams/{teamId}/headshots")]
    public async Task<List<PlayerHeadshot>> TeamHeadshots(string teamId) =>
        await teamRepo.GetHeadshots(state.Sport, teamId);

    [HttpGet("/api/teams/{teamId}/headshots/download")]
    public async Task<FileResult> DownloadHeadshots(string teamId)
    {
        var headshots = await teamRepo.GetHeadshots(state.Sport, teamId);
        var zippedImages = await SidearmLoader.DownloadImages(headshots);
        var fileName = $"{teamId}-headshots.zip";
        return File(zippedImages.ToArray(), MediaTypeNames.Application.Zip, fileName);
    }
}