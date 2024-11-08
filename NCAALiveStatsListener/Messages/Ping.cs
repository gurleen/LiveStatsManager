using System.Text.Json.Serialization;

namespace NCAALiveStatsListener.Messages;

public class Ping
{
    [JsonPropertyName("timestamp")]
    public string Timestamp { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff");
}