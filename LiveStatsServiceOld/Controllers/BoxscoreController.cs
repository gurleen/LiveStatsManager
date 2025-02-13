using Microsoft.AspNetCore.Mvc;
using NCAALiveStats;
using NCAALiveStats.Messages;

namespace LiveStatsService.Controllers;

[ApiController]
[Route("api/boxscore")]
public class BoxscoreController(GameState gameState) : ControllerBase
{
    [HttpGet]
    [Route("{team}")]
    public TeamStats GetBoxscore(TeamSide team) => gameState.GetTeamStats(team);
    
    [HttpGet]
    [Route("{team}/player/{shirt}")]
    public PlayerStats GetPlayerStats(TeamSide team, string shirt) => gameState.GetPlayerStats(team, shirt);
    
    [HttpGet]
    [Route("{team}/player")]
    public Dictionary<string, PlayerStats> GetPlayerStats(TeamSide team) => gameState.PlayerStatsByShirt(team);
}