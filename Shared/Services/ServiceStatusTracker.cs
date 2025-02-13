namespace LiveStatsManager.Services;

public enum ServiceStatus
{
    Unknown,
    Disabled,
    Running,
    NotRunning
}

public class ServiceStatusTracker
{
    private readonly Dictionary<Type, ServiceStatus> _status = new();
    
    public void SetStatus<T>(ServiceStatus status)
    {
        _status[typeof(T)] = status;
    }
    
    public ServiceStatus GetStatus<T>()
    {
        return _status.TryGetValue(typeof(T), out var status)? status : ServiceStatus.Unknown;
    }

    public ServiceStatus GetStatus(Type service)
    {
        return _status.TryGetValue(service, out var status) ? status : ServiceStatus.Unknown;
    }
}