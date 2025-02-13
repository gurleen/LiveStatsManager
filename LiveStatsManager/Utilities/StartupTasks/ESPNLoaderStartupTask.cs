using LiveStatsManager.Services;

namespace LiveStatsManager.Utilities.StartupTasks;

public class ESPNLoaderStartupTask(TeamDataRepository teamRepo, ILogger<ESPNLoaderStartupTask> logger) : IStartupTask
{
    public async Task Execute()
    {
        logger.LogInformation("Updating stats from ESPN...");
        await teamRepo.UpdateAll();
        logger.LogInformation("Stats updated from ESPN.");
    }
}