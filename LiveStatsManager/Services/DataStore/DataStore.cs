using System.Collections.Concurrent;
using LiveStatsManager.FileWatcher;
using LiveStatsManager.Hubs;
using Microsoft.AspNetCore.SignalR;
using Shared.Interfaces;

namespace LiveStatsManager.Services.DataStore;

public interface IDataStore
{
    public event Func<Task>? OnUpdate;
    public bool Add(string key, string value);
    public bool Add(DataPair pair);
    public void Add(List<DataPair> pairs);
    public void Toggle(string key);
    public Dictionary<string, string> GetAll();
    public string Get(string key, string defaultValue = "");
}

public class DataStore(IHubContext<LiveDataHub, ILiveDataHub> hubContext) : IDataStore
{
    private readonly SynchronizationContext _syncContext = SynchronizationContext.Current 
                                                           ?? new SynchronizationContext();
    private readonly ConcurrentDictionary<string, string> _dataStore = new();
    public event Func<Task>? OnUpdate;

    public bool Add(string key, string value)
    {
        if(key.StartsWith('#')) return false;
        if (_dataStore.TryGetValue(key, out var existingValue) && existingValue == value) return false;
        _dataStore[key] = value;
        hubContext.Clients.All.DataUpdate(key, value);
        try
        {
            _syncContext.Post(_ => OnUpdate?.Invoke(), null);
        }
        catch
        {
            // ignored
        }
        return true;
    }

    public bool Add(DataPair pair)
    {
        return Add(pair.Key, pair.Value);
    }

    public void Add(List<DataPair> pairs)
    {
        pairs.ForEach(dp => Add(dp));
    }

    public void Toggle(string key)
    {
        Add(key, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());
    }

    public Dictionary<string, string> GetAll()
    {
        return _dataStore.ToDictionary(x => x.Key, x => x.Value);
    }

    public string Get(string key, string defaultValue = "")
    {
        return _dataStore.GetValueOrDefault(key) ?? defaultValue;
    }
}