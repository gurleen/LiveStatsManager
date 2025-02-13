using GfxDataService.DataStore;
using GfxDataService.FileWatcher;
using Microsoft.AspNetCore.SignalR;
using Shared.Interfaces;

namespace GfxDataService.Hubs;

public class LiveDataHub : Hub<ILiveDataHub>
{
    public async Task EmitUpdate(DataPair pair) => 
        await Clients.All.DataUpdate(pair.Key, pair.Value);
}