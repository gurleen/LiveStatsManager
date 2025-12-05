using System;
using LiveStatsManager.Hubs;
using LiveStatsManager.Models.TypedDataStore;
using Microsoft.AspNetCore.SignalR;
using Shared.Interfaces;

namespace LiveStatsManager.Services.DataStore;

public class TypedDataStore(IHubContext<TypedLiveDataHub, ITypedLiveDataHub> hubContext)
{
    private GameState _gameState;
    private WrestlingScorebugState _wrestlingScorebugState;
    public GameState GameState
    {
        get => _gameState;
        set
        {
            _gameState = value;
            hubContext.Clients.All.GameStateUpdated(_gameState);
        }
    }

    public WrestlingScorebugState WrestlingScorebugState
    {
        get => _wrestlingScorebugState;
        set
        {
            _wrestlingScorebugState = value;
            hubContext.Clients.All.WrestlingStateUpdated(_wrestlingScorebugState);
        }
    }
}
