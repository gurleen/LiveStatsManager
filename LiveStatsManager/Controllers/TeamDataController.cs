using Microsoft.AspNetCore.Mvc;

namespace LiveStatsManager.Controllers;

public class TeamDataController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}