using Microsoft.AspNetCore.Mvc;
using NCAALiveStats;
using NCAALiveStats.Messages;

namespace LiveStatsService.Controllers;

[ApiController]
[Route("api/boxscore")]
public class BoxscoreController(RawGameState rawGameState) : ControllerBase
{
    [HttpGet]
    [Route("{team}")]
    public TeamStats GetBoxscore(TeamSide team) => rawGameState.GetTeamStats(team);
    
    [HttpGet]
    [Route("{team}/player/{shirt}")]
    public PlayerStats GetPlayerStats(TeamSide team, string shirt) => rawGameState.GetPlayerStats(team, shirt);
    
    [HttpGet]
    [Route("{team}/player")]
    public Dictionary<string, PlayerStats> GetPlayerStats(TeamSide team) => rawGameState.PlayerStatsByShirt(team);
}