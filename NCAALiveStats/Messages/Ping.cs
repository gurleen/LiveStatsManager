using System.Text.Json.Serialization;
using NCAALiveStats.Messages.Helpers;

namespace NCAALiveStats.Messages;

[SocketMessage("ping")]
public class Ping
{
    [JsonPropertyName("timestamp")]
    public string Timestamp { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff");
}