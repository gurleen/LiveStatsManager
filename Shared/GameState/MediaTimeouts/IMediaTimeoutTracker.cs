namespace Shared.GameState.MediaTimeouts;

public interface IMediaTimeoutTracker
{
    public void DeadBallAt(int period, int timeRemaining);
    public void ChargedTimeoutAt(int period, int timeRemaining);
    public bool IsMediaTimeout(int period, int timeRemaining);
}