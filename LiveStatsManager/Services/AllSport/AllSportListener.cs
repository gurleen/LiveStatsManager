using LiveStatsManager.Services.DataStore;

namespace LiveStatsManager.Services.AllSport;

public class AllSportListener(
    SettingsProvider settingsProvider,
    IDataStore store,
    AppState appState,
    TypedDataStore typedDataStore,
    ILogger<AllSportListener> logger) : BackgroundService
{
    private int Retries = 5;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var settings = settingsProvider.AllSportSettings;
        if (settings.Enabled == false) return;
        while (true)
        {
            try
            {
                using var reader = new SerialPortReader(settings.ComPort, settings.BaudRate, lineTerminator: "\x04");
                reader.Open();
                logger.LogInformation("Connected to AllSport CG.");
                while (!stoppingToken.IsCancellationRequested)
                {
                    await reader.ReadAsync(HandleLine);
                    await Task.Delay(100, stoppingToken);
                }
            }
            catch (Exception exc)
            {
                logger.LogError(exc, "Exception in AllSportListener");
                if(Retries > 0)
                {
                    await Task.Delay(1000, stoppingToken);
                    logger.LogCritical("Attempting to re-connect to AllSport CG...");
                    Retries -= 1;
                }
                else
                {
                    logger.LogError("AllSportListener will not attempt to re-connect.");
                }
            }
        }
    }

    private Task HandleLine(string line)
    {
        var sport = appState.Sport;
        var data = new AllSportData(line, sport);
        store.Add(data.DataPairs().ToList());
        UpdateTypedStore(data);

        if (store.Add("fade:Home-Score", data.HomeScore))
        {
            store.Add("Home-Last-Score", data.Clock);
        }
        if (store.Add("fade:Away-Score", data.AwayScore))
        {
            store.Add("Away-Last-Score", data.Clock);
        }

        return Task.CompletedTask;
    }

    private void UpdateTypedStore(AllSportData data)
    {
        var homeScoreDiff = typedDataStore.GameState.HomeTeam.Score - data.HomeScoreInt;
        var awayScoreDiff = typedDataStore.GameState.AwayTeam.Score - data.AwayScoreInt;
        typedDataStore.GameState = typedDataStore.GameState with
        {
            Clock = data.ClockSeconds,
            ClockDisplay = data.ClockDisplay,
            ShotClock = data.ShotClockSeconds,
            Period = data.PeriodInt,
            HomeTeam = typedDataStore.GameState.HomeTeam with
            {
                Score = data.HomeScoreInt,
                LastScoreTime = (homeScoreDiff > 0) ? data.ClockSeconds : typedDataStore.GameState.HomeTeam.LastScoreTime,
                LastFieldGoalTime = (homeScoreDiff > 1) ? data.ClockSeconds : typedDataStore.GameState.HomeTeam.LastFieldGoalTime
            },
            AwayTeam = typedDataStore.GameState.AwayTeam with
            {
                Score = data.AwayScoreInt,
                LastScoreTime = (awayScoreDiff > 0) ? data.ClockSeconds : typedDataStore.GameState.AwayTeam.LastScoreTime,
                LastFieldGoalTime = (awayScoreDiff > 1) ? data.ClockSeconds : typedDataStore.GameState.AwayTeam.LastFieldGoalTime
            }
        };
    }
}