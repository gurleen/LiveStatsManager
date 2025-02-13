using System.Collections.Concurrent;
using GfxDataService.FileWatcher;
using GfxDataService.Hubs;
using Microsoft.AspNetCore.SignalR;
using Shared.Interfaces;

namespace GfxDataService.DataStore;

public interface IDataStore
{
    public void Add(DataPair pair);
    public void Add(List<DataPair> pairs);
    public Dictionary<string, string> GetAll();
    public string Get(string key);
}

public class DataStore(IHubContext<LiveDataHub, ILiveDataHub> hubContext) : IDataStore
{
    private readonly ConcurrentDictionary<string, string> _dataStore = new();

    private void Add(string key, string value)
    {
        if(key.StartsWith('#')) return;
        if(_dataStore.TryGetValue(key, out var existingValue) && existingValue == value) return;
        _dataStore[key] = value;
        Console.WriteLine($"{key} = {value}");
        hubContext.Clients.All.DataUpdate(key, value);
    }

    public void Add(DataPair pair)
    {
        Add(pair.Key, pair.Value);
    }

    public void Add(List<DataPair> pairs)
    {
        pairs.ForEach(Add);
    }

    public Dictionary<string, string> GetAll()
    {
        return _dataStore.ToDictionary(x => x.Key, x => x.Value);
    }

    public string Get(string key)
    {
        return _dataStore.GetValueOrDefault(key) ?? string.Empty;
    }
}