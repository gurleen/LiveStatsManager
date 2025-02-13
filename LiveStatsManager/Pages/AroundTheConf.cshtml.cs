using LiveStatsManager.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Objects;
using Svg;

namespace LiveStatsManager.Pages;

public class AroundTheConf(TeamDataRepository teamRepo, AppState appState) : PageModel
{
    private List<GameStatus> games = [];
    private const string GraphicPath = "wwwroot/graphics/AroundTheConf.png";

    public async Task OnGet()
    {
        games = await teamRepo.GetScoreboard(appState.Sport);
    }

    public async Task<string> GetGraphic()
    {
        return await System.IO.File.ReadAllTextAsync(GraphicPath);
    }
}