using LiveStatsManager.Models;
using LiveStatsManager.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.GameState;
using Shared.Objects;

namespace LiveStatsManager.Controllers;

[ApiController]
public class AppStateController(TeamDataRepository repo, AppState state, CurrentGameState gameState) : ControllerBase
{
    [HttpGet("/api/state/homeTeam")]
    public FullTeam HomeTeam() => repo.GetFullTeam(state.Sport, state.HomeTeam.Id);

    [HttpGet("/api/state/homeTeam/legacy")]
    public List<LegacyPlayer> HomeTeamLegacyJson() => repo
        .PlayersForTeam(state.Sport, state.HomeTeam.Id)
        .Select(p => p.AsLegacyPlayer())
        .ToList();
    
    [HttpGet("/api/state/awayTeam")]
    public FullTeam AwayTeam() => repo.GetFullTeam(state.Sport, state.AwayTeam.Id);
    
    [HttpGet("/api/state/awayTeam/legacy")]
    public List<LegacyPlayer> AwayTeamLegacyJson() => repo
        .PlayersForTeam(state.Sport, state.AwayTeam.Id)
        .Select(p => p.AsLegacyPlayer())
        .ToList();
    
    [HttpGet("/api/state/graphicsData")]
    public Dictionary<string, string> GraphicsData() => state.GraphicsData.TemplateData();
    
    [HttpGet("/api/state/game")]
    public CurrentGameState GameState() => gameState;
}