using System.Globalization;
using CsvHelper;
using Shared.Services;

namespace LiveStatsManager.Services;

public class FileWriterService(SettingsProvider settingsProvider,
    ServiceStatusTracker status, TeamDataRepository teamRepo, AppState appState) : BackgroundService
{
    private readonly FileWriterSettings Settings = settingsProvider.FileWriterSettings;
    private string OutputDir => Settings.OutputDirectory;
    private string StandingsFile => Path.Join(OutputDir, "standings.csv");
    private string ScoresFile => Path.Join(OutputDir, "scores.csv");
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!Settings.Enabled)
        {
            status.SetStatus<FileWriterService>(ServiceStatus.Disabled);
            return;    
        }
        
        Directory.CreateDirectory(OutputDir);
        status.SetStatus<FileWriterService>(ServiceStatus.Running);
        try
        {
            await Loop(stoppingToken);
        }
        catch
        {
            status.SetStatus<FileWriterService>(ServiceStatus.NotRunning);
        }
    }

    private async Task Loop(CancellationToken stoppingToken)
    {
        await WriteStandings();
        while (!stoppingToken.IsCancellationRequested)
        {
            await WriteScores();
            await Task.Delay(Settings.UpdateIntervalMinutes, stoppingToken);
        }
    }
    
    private static async Task WriteCsv(string filename, IEnumerable<object> records)
    {
        await using var writer = new StreamWriter(filename);
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        await csv.WriteRecordsAsync(records);
    }

    private async Task WriteStandings()
    {
        var standings = teamRepo.GetStandings(appState.Sport, 10);
        await WriteCsv(StandingsFile, standings);
    }

    private async Task WriteScores()
    {
        var scores = await teamRepo.GetScoreboard(appState.Sport);
        await WriteCsv(ScoresFile, scores);
    }
}