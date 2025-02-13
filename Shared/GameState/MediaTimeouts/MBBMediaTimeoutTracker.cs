namespace Shared.GameState.MediaTimeouts;

public class MBBMediaTimeoutTracker : IMediaTimeoutTracker
{
    private readonly Queue<MediaTimeout> MediaMarkers = new();
    private readonly List<TakenMediaTimeout> TakenTimeouts = [];
    private (int, int)? FloaterTakenAt { get; set; } = null;

    private void AddMarkers()
    {
        MediaMarkers.Enqueue(new MediaTimeout(1, 16));
        MediaMarkers.Enqueue(new MediaTimeout(1, 12));
        MediaMarkers.Enqueue(new MediaTimeout(1, 8));
        MediaMarkers.Enqueue(new MediaTimeout(1, 4));
        MediaMarkers.Enqueue(new MediaTimeout(2, 16));
        MediaMarkers.Enqueue(new MediaTimeout(2, 12));
        MediaMarkers.Enqueue(new MediaTimeout(2, 8));
        MediaMarkers.Enqueue(new MediaTimeout(2, 4));
    }

    public MBBMediaTimeoutTracker()
    {
        AddMarkers();
    }
    
    private MediaTimeout NextMedia => MediaMarkers.Peek();

    private TakenMediaTimeout TakeTimeout(int period, int secondsRemaining)
    {
        var timeout = MediaMarkers.Dequeue().TakeAt(period, secondsRemaining);
        TakenTimeouts.Add(timeout);
        return timeout;
    }
    
    public void DeadBallAt(int period, int timeRemaining)
    {
        if (NextMedia.IsInWindow(period, timeRemaining))
        {
            TakeTimeout(period, timeRemaining);
        }
    }

    public void ChargedTimeoutAt(int period, int timeRemaining)
    {
        if (FloaterTakenAt.HasValue || period != 2) return;
        FloaterTakenAt = (period, timeRemaining);
    }
    
    private bool AtFloaterTimeout(int period, int timeRemaining) => 
        FloaterTakenAt.HasValue && FloaterTakenAt.Value == (period, timeRemaining);
    
    private bool AtMediaTimeout(int period, int timeRemaining) =>
        TakenTimeouts.Any(t => t.IsCurrent(period, timeRemaining));

    public bool IsMediaTimeout(int period, int timeRemaining) =>
        AtMediaTimeout(period, timeRemaining)
        || AtFloaterTimeout(period, timeRemaining);
}