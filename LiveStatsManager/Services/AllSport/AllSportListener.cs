using LiveStatsManager.Services.DataStore;
using Shared.Enums;
using Shared.Extensions;
using Shared.GameState;
using Shared.Objects;

namespace LiveStatsManager.Services.AllSport;

public class AllSportListener(
    SettingsProvider settingsProvider,
    IDataStore store,
    AppState appState) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var settings = settingsProvider.AllSportSettings;
        if (settings.Enabled == false) return;
        using var reader = new SerialPortReader(settings.ComPort, settings.BaudRate, lineTerminator: "\x04");
        reader.Open();
        while (!stoppingToken.IsCancellationRequested)
        {
            await reader.ReadAsync(HandleLine);
            await Task.Delay(100, stoppingToken);
        }
    }

    private Task HandleLine(string line)
    {
        var sport = appState.Sport;
        var data = new AllSportData(line, sport);
        store.Add(data.DataPairs().ToList());
        
        if(store.Add("fade:Home-Score", data.HomeScore))
        {
            store.Add("Home-Last-Score", data.Clock);
        }
        if(store.Add("fade:Away-Score", data.AwayScore))
        {
            store.Add("Away-Last-Score", data.Clock);
        }
        
        return Task.CompletedTask;
    }
}