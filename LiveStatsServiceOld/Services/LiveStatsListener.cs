using NCAALiveStats;

namespace LiveStatsServiceOld.Services;

public class LiveStatsListener(NCAAListener ncaaListener, ILogger<LiveStatsListener> logger)
    : BackgroundService
{
    private Thread? _listenerThread;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _listenerThread = new Thread(() => ncaaListener.Start("10.248.65.90", 7677));
        _listenerThread.Start();
        return Task.CompletedTask;
    }
}