using LiveStatsManager.Models;
using LiveStatsManager.Models.TypedDataStore;
using LiveStatsManager.Services;
using LiveStatsManager.Services.DataStore;
using Microsoft.AspNetCore.Mvc;
using Shared.Objects;

namespace LiveStatsManager.Controllers;

[ApiController]
public class AppStateController(TeamDataRepository repo, AppState state, TypedDataStore typedDataStore) : ControllerBase
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
    public GraphicsData GraphicsData() => state.GraphicsData;

    [HttpGet("/api/state/graphicsData/legacy")]
    public Dictionary<string, string> GraphicsDataLegacy() => state.GraphicsData.TemplateData();

    [HttpGet("/api/state/game")]
    public GameState GameState() => typedDataStore.GameState;

    [HttpPost("/api/state/game/toggleHomeSlider")]
    public bool ToggleHomeSlider()
    {
        typedDataStore.GameState = typedDataStore.GameState with
        {
            ScorebugState = typedDataStore.GameState.ScorebugState with
            {
                HomeSlider = typedDataStore.GameState.ScorebugState.HomeSlider with
                {
                    Playing = !typedDataStore.GameState.ScorebugState.HomeSlider.Playing
                }
            }
        };

        return typedDataStore.GameState.ScorebugState.HomeSlider.Playing;
    }

    [HttpGet("/api/state/game/setHomeSliderPlayer/{number}")]
    public string SetHomeSliderPlayer(int number)
    {
        typedDataStore.GameState = typedDataStore.GameState with
        {
            ScorebugState = typedDataStore.GameState.ScorebugState with
            {
                HomeSlider = typedDataStore.GameState.ScorebugState.HomeSlider with
                {
                    PlayerNumber = number,
                    Playing = false
                }
            }
        };

        return "OK";
    }

    [HttpPost("/api/state/game/toggleAwaySlider")]
    public bool ToggleAwaySlider()
    {
        typedDataStore.GameState = typedDataStore.GameState with
        {
            ScorebugState = typedDataStore.GameState.ScorebugState with
            {
                AwaySlider = typedDataStore.GameState.ScorebugState.AwaySlider with
                {
                    Playing = !typedDataStore.GameState.ScorebugState.AwaySlider.Playing
                }
            }
        };

        return typedDataStore.GameState.ScorebugState.AwaySlider.Playing;
    }

    [HttpGet("/api/state/game/setAwaySliderPlayer/{number}")]
    public string SetAwaySliderPlayer(int number)
    {
        typedDataStore.GameState = typedDataStore.GameState with
        {
            ScorebugState = typedDataStore.GameState.ScorebugState with
            {
                AwaySlider = typedDataStore.GameState.ScorebugState.AwaySlider with
                {
                    PlayerNumber = number,
                    Playing = false
                }
            }
        };

        return "OK";
    }

    [HttpPost("/api/state/game/toggleTextSlider")]
    public bool ToggleTextSlider()
    {
        typedDataStore.GameState = typedDataStore.GameState with
        {
            ScorebugState = typedDataStore.GameState.ScorebugState with
            {
                TextSliderState = typedDataStore.GameState.ScorebugState.TextSliderState with
                {
                    Playing = !typedDataStore.GameState.ScorebugState.TextSliderState.Playing
                }
            }
        };

        return typedDataStore.GameState.ScorebugState.TextSliderState.Playing;
    }

    [HttpPost("/api/state/game/setTextSlider")]
    public string SetTextSlider(SliderPreset preset)
    {
        typedDataStore.GameState = typedDataStore.GameState with
        {
            ScorebugState = typedDataStore.GameState.ScorebugState with
            {
                TextSliderState = typedDataStore.GameState.ScorebugState.TextSliderState with
                {
                    Title = preset.Title,
                    Subtitle = preset.Subtitle
                }
            }
        };

        return "OK";
    }

}

public record struct SliderPreset(string Title, string Subtitle);