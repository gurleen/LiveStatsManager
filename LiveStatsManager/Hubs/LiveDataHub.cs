using LiveStatsManager.FileWatcher;
using Microsoft.AspNetCore.SignalR;
using Shared.Interfaces;

namespace LiveStatsManager.Hubs;

public class LiveDataHub : Hub<ILiveDataHub>
{
    public async Task EmitUpdate(DataPair pair) => 
        await Clients.All.DataUpdate(pair.Key, pair.Value);
}