namespace Shared.GameState.MediaTimeouts;


public record MediaTimeout(int Period, int UnderMinutes)
{
    private int ToSeconds => UnderMinutes * 60;
    
    public bool IsInWindow(int period, int secondsRemaining) =>
        period == Period && secondsRemaining <= ToSeconds;
    
    public TakenMediaTimeout TakeAt(int period, int secondsRemaining) =>
        new(period, secondsRemaining, this);
}

public record TakenMediaTimeout(int Period, int SecondsRemaining, MediaTimeout Timeout)
{
    public bool IsCurrent(int period, int secondsRemaining) =>
        period == Period && secondsRemaining == SecondsRemaining;
}