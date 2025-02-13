using NCAALiveStats;
using Shared.Services;

namespace LiveStatsManager.Services;

public class LiveStatsListener(
    NCAAListener ncaaListener,
    ILogger<LiveStatsListener> logger,
    SettingsProvider settingsProvider,
    ServiceStatusTracker status) : BackgroundService
{
    private Thread? _listenerThread;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        status.SetStatus<LiveStatsListener>(ServiceStatus.NotRunning);
        var settings = settingsProvider.LiveStatsSettings;
        if (!settings.Enabled)
        {
            status.SetStatus<LiveStatsListener>(ServiceStatus.Disabled);
            return Task.CompletedTask;
        }
        _listenerThread = new Thread(() => ncaaListener.Start(settings.IpAddress, settings.Port, settings.Enabled));
        _listenerThread.Start();
        return Task.CompletedTask;
    }
}