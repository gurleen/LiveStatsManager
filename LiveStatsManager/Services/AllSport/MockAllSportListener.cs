using System;
using LiveStatsManager.Models.TypedDataStore;
using LiveStatsManager.Services.DataStore;

namespace LiveStatsManager.Services.AllSport;

public class MockAllSportListener(TypedDataStore typedDataStore) : BackgroundService
{
    private int Clock = 60 * 20;
    private int ShotClock = 30;
    private int Period = 1;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (true)
        {
            Tick();
            // Update();
            await Task.Delay(1000, stoppingToken);
        }
    }

    private void Tick()
    {
        Clock = Clock == 0 ? (60 * 20) : (Clock - 1);
        ShotClock = ShotClock == 0 ? 30 : (ShotClock - 1);
    }

    private void Update()
    {
        typedDataStore.GameState = typedDataStore.GameState with
        {
            Clock = Clock,
            ShotClock = ShotClock
        };
    }
}
