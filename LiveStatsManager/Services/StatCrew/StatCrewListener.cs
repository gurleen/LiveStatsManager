using System;
using NCAALiveStats.ExternalData.StatCrew;

namespace LiveStatsManager.Services.StatCrew;

public class StatCrewListener : BackgroundService
{
    private readonly string StatsFileDir = @"/Users/gurleen/Downloads/";
    private StatCrewDataService statCrewDataService;
    private FileSystemWatcher fileSystemWatcher;

    public StatCrewListener(StatCrewDataService dataService, SettingsProvider settings)
    {
        statCrewDataService = dataService;
        fileSystemWatcher = new FileSystemWatcher(StatsFileDir, "bbgame.xml");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        fileSystemWatcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;

        fileSystemWatcher.Changed += OnChanged;
        fileSystemWatcher.EnableRaisingEvents = true;
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        var parser = new StatCrewBasketballParser(e.FullPath);
        Task.Run(async () =>
        {
            var data = await parser.Load();
            statCrewDataService.UpdateGameState(data);
        });
    }
}
