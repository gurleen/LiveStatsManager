using Microsoft.EntityFrameworkCore;
using NCAADataLoader.Models;
using NCAALiveStats.ExternalData.ESPN;
using Shared.Enums;

namespace NCAADataLoader.Loaders;

public class NCAALoader
{
    private readonly ESPNLoader _espnLoader;
    private readonly StatsContext _context;

    public NCAALoader(StatsContext context)
    {
        var sport = Sport.MensBasketball;
        _espnLoader = new ESPNLoader(sport);
        _context = context;
    }

    private async Task LoadTeams()
    {
        var espnTeams = await _espnLoader.Teams();
        var teams = espnTeams.Teams
            .Select(t => t.ToDbTeam())
            .ToList();
        await _context.Teams
            .UpsertRange(teams)
            .On(t => t.TeamId)
            .RunAsync();
    }

    public async Task LoadAll()
    {
        await LoadTeams();
    }
}