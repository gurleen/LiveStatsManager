using System;
using LiveStatsManager.Models.TypedDataStore;
using Microsoft.AspNetCore.SignalR;

namespace LiveStatsManager.Hubs;

public interface ITypedLiveDataHub
{
    Task GameStateUpdated(GameState gameState);
    Task WrestlingStateUpdated(WrestlingScorebugState wrestlingBugState);
}

public class TypedLiveDataHub : Hub<ITypedLiveDataHub>
{
    private WrestlingScorebugState lastWrestlingState = WrestlingScorebugState.Default();

    public async Task UpdateGameState(GameState gameState) =>
        await Clients.All.GameStateUpdated(gameState);

    public async Task UpdateWrestlingState(WrestlingScorebugState state)
    {
        lastWrestlingState = state;
        await Clients.All.WrestlingStateUpdated(state);
    }

    public override async Task OnConnectedAsync()
    {
        await Clients.Caller.WrestlingStateUpdated(lastWrestlingState);
    }
}
