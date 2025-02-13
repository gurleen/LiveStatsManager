namespace LiveStatsManager.Services.Graphics;

public class GraphicLayer(int layerNum, string source)
{
    private readonly SynchronizationContext _syncContext = 
        SynchronizationContext.Current ?? new SynchronizationContext();
    
    public event Func<Task>? OnPlay;
    public event Func<Task>? OnStop;
    public event Func<Task>? OnSetSource;
    
    public int LayerNum { get; init; } = layerNum;
    public string Source { get; set; } = source;
    public GraphicState State { get; private set; } = GraphicState.Stopped;
    
    public void SetSource(string source)
    {
        Source = source;
        _syncContext.Post(_ => OnSetSource?.Invoke(), null);
    }

    public void Play()
    {
        if (State != GraphicState.Stopped) return;
        _syncContext.Post(_ => OnPlay?.Invoke(), null);
    }

    public void Stop()
    {
        if (State != GraphicState.Playing) return;
        _syncContext.Post(_ => OnStop?.Invoke(), null);
    }
}