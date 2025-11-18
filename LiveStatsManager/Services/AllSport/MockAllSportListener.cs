using System;
using LiveStatsManager.Components.Pages;
using LiveStatsManager.Models.TypedDataStore;
using LiveStatsManager.Services.DataStore;
using Shared.GameState;

namespace LiveStatsManager.Services.AllSport;

public class MockAllSportListener(TypedDataStore typedDataStore, CurrentGameState gameState, SettingsProvider settings) : BackgroundService
{
    private int Clock = 60 * 20;
    private string ClockDisplay
    {
        get
        {
            var span = TimeSpan.FromSeconds(Clock);
            return $"{span.Minutes}:{span.Seconds:D2}";
        }
    }
    private int ShotClock = 30;
    private int Period = 1;
    private int HomeScore = 0;
    private int AwayScore = 0;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!settings.AllSportSettings.MockEnabled) return;
        while (true)
        {
            Tick();
            Update();
            await Task.Delay(1000, stoppingToken);
        }
    }

    private void Tick()
    {
        Clock = Clock == 0 ? (60 * 20) : (Clock - 1);
        ShotClock = ShotClock == 0 ? 30 : (ShotClock - 1);
        var rand = new Random();
        if (rand.NextDouble() < 0.3)
        {
            HomeScore += 2;
            if(HomeScore > 100) { HomeScore = 0; }
        }
        else if (rand.NextDouble() < 0.3)
        {
            AwayScore += 2;
            if(AwayScore > 100) { AwayScore = 0; }
        }
    }

    private void Update()
    {
        var homeScoreDiff = typedDataStore.GameState.HomeTeam.Score - HomeScore;
        var awayScoreDiff = typedDataStore.GameState.AwayTeam.Score - AwayScore;
        typedDataStore.GameState = typedDataStore.GameState with
        {
            Period = Period,
            Clock = Clock,
            ClockDisplay = ClockDisplay,
            ShotClock = ShotClock,
            HomeTeam = typedDataStore.GameState.HomeTeam with
            {
                Score = HomeScore,
                LastScoreTime = (homeScoreDiff > 0) ? Clock : typedDataStore.GameState.HomeTeam.LastScoreTime,
                LastFieldGoalTime = (homeScoreDiff > 1) ? Clock : typedDataStore.GameState.HomeTeam.LastFieldGoalTime
            },
            AwayTeam = typedDataStore.GameState.AwayTeam with
            {
                Score = AwayScore,
                LastScoreTime = (awayScoreDiff > 0) ? Clock : typedDataStore.GameState.AwayTeam.LastScoreTime,
                LastFieldGoalTime = (awayScoreDiff > 1) ? Clock : typedDataStore.GameState.AwayTeam.LastFieldGoalTime
            }
        };
        gameState.Period = Period;
        gameState.TimeRemaining = Clock;
        gameState.ShotClock = ShotClock;
    }
}
