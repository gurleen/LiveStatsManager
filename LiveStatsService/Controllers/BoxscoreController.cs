using Microsoft.AspNetCore.Mvc;
using NCAALiveStatsListener;
using NCAALiveStatsListener.Messages;

namespace LiveStatsService.Controllers;

[ApiController]
[Route("api/boxscore")]
public class BoxscoreController : ControllerBase
{
    private readonly NCAAListener _ncaaListener;
    
    public BoxscoreController(NCAAListener ncaaListener)
    {
        _ncaaListener = ncaaListener;
    }
    
    public BoxScore Index()
    {
        return _ncaaListener.BoxScore;
    }
}