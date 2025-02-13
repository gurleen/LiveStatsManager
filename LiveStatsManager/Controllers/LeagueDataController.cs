using LiveStatsManager.Models;
using LiveStatsManager.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Objects;

namespace LiveStatsManager.Controllers;

[ApiController]
public class LeagueDataController(TeamDataRepository teamRepo, AppState appState) : ControllerBase
{
    [Route("/api/standings/{confId:int}")]
    public IEnumerable<FullTeam> Standings(int confId)
    {
        return teamRepo.GetStandings(appState.Sport, confId);
    }
    
    [Route("/api/scoreboard")]
    public async Task<List<GameStatus>> Scoreboard()
    {
        return await teamRepo.GetScoreboard(appState.Sport);
    }
}