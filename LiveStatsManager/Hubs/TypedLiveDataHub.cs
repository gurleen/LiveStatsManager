using System;
using LiveStatsManager.Models.TypedDataStore;
using Microsoft.AspNetCore.SignalR;

namespace LiveStatsManager.Hubs;

public interface ITypedLiveDataHub
{
    Task GameStateUpdated(GameState gameState);
}

public class TypedLiveDataHub : Hub<ITypedLiveDataHub>
{
    public async Task UpdateGameState(GameState gameState) =>
        await Clients.All.GameStateUpdated(gameState);
}
