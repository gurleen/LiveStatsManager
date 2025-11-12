using System;
using NCAALiveStats.ExternalData.StatCrew;

namespace LiveStatsManager.Services.StatCrew;

public class StatCrewListener : BackgroundService
{
    private readonly bool Enabled;
    private readonly string StatsFileDir;
    private StatCrewDataService statCrewDataService;
    private readonly FileSystemWatcher? fileSystemWatcher;

    public StatCrewListener(StatCrewDataService dataService, SettingsProvider settings)
    {
        Enabled = settings.StatCrewSettings.Enabled;
        StatsFileDir = settings.StatCrewSettings.WatchDirectory;
        statCrewDataService = dataService;
        if (Enabled)
        {
            fileSystemWatcher = new FileSystemWatcher(StatsFileDir, settings.StatCrewSettings.XmlFileName);
            ParseFile(Path.Join(StatsFileDir, settings.StatCrewSettings.XmlFileName));
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (fileSystemWatcher is null) return;
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
        ParseFile(e.FullPath);
    }
    
    private void ParseFile(string path)
    {
        var parser = new StatCrewBasketballParser(path);
        Task.Run(async () =>
        {
            var data = await parser.Load();
            statCrewDataService.UpdateGameState(data);
        });
    }
}
