using LiveStatsManager.Services.StatCrew;
using Microsoft.AspNetCore.Mvc;
using NCAALiveStats;
using NCAALiveStats.ExternalData.StatCrew.Objects;
using NCAALiveStats.Messages;

namespace LiveStatsManager.Controllers;

[ApiController]
[Route("api/boxscore")]
public class BoxscoreController(StatCrewDataService statCrewData) : ControllerBase
{
    [HttpGet]
    [Route("{team}")]
    public StatCrewBasketballState GetBoxscore(TeamSide team) => statCrewData.GameState;
    
    [HttpGet]
    [Route("{team}/player/{shirt}")]
    public StatCrewBasketballPlayer GetPlayerStats(TeamSide team, int shirt)
    {
        var teamData = team == TeamSide.Home ? statCrewData.GameState.HomeTeam : statCrewData.GameState.AwayTeam;
        return teamData.Players.First(x => x.JerseyNumber == shirt);
    }
}