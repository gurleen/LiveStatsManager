using GfxDataService.DataStore;
using GfxDataService.FileWatcher;
using Microsoft.AspNetCore.SignalR;

namespace GfxDataService.Hubs;

public interface ILiveDataHub
{
    Task DataUpdate(string key, string value);
}

public class LiveDataHub : Hub<ILiveDataHub>
{
    public async Task EmitUpdate(DataPair pair) => 
        await Clients.All.DataUpdate(pair.Key, pair.Value);
}