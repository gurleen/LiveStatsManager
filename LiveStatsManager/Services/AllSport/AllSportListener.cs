namespace LiveStatsManager.Services;

public class AllSportListener(ILogger<LiveStatsListener> logger,
    SettingsProvider settingsProvider,
    ServiceStatus status) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }
}