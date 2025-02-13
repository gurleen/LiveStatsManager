namespace NCAALiveStats.Messages.Helpers;

[AttributeUsage(AttributeTargets.Class)]
public class SocketMessage(string typeKey) : Attribute
{
    public string TypeKey { get; } = typeKey;
}