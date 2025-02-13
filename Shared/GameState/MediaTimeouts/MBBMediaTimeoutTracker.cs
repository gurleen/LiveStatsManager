namespace Shared.GameState.MediaTimeouts;

public class MBBMediaTimeoutTracker : IMediaTimeoutTracker
{
    private Queue<MediaTimeout> MediaMarkers = new();
    public bool FloaterTaken { get; private set; } = false;

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
    
    public void DeadBallAt(int period, int timeRemaining)
    {
        if (NextMedia.IsInWindow(period, timeRemaining))
        {
            
        }
    }

    public void ChargedTimeoutAt(int period, int timeRemaining)
    {
        throw new NotImplementedException();
    }

    public bool IsMediaTimeout { get; }
}