using System;
using NCAALiveStats.ExternalData.StatCrew.Objects;

namespace LiveStatsManager.Services.StatCrew;

public class StatCrewDataService()
{
    private StatCrewBasketballState _gameState;
    public StatCrewBasketballState GameState => _gameState;

    public void UpdateGameState(StatCrewBasketballState newState)
    {
        _gameState = newState;
        Console.WriteLine("Updated StatCrew game state.");
    }
}
