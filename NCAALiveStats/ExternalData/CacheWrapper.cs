using System.Text.Json;
using Optional;

namespace NCAALiveStats.ExternalData;

public class CacheWrapper<T>(T data)
{
    public DateTime CreatedAt { get; } = DateTime.Now;
    public T Data { get; init; } = data;
    public bool IsExpired(TimeSpan maxAge) => DateTime.Now - CreatedAt > maxAge;
}

public static class CacheWrapperExtensions
{
    public static string ToJson<T>(this CacheWrapper<T> cacheWrapper) => JsonSerializer.Serialize(cacheWrapper);

    public static CacheWrapper<T>? FromJson<T>(this string json)
    {
        return JsonSerializer.Deserialize<CacheWrapper<T>>(json);
    }
}