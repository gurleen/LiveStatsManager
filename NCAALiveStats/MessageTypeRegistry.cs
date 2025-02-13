using System.Reflection;
using NCAALiveStats.Messages.Helpers;
using Optional;

namespace NCAALiveStats;

public interface IMessageTypeRegistry
{
    void RegisterMessageTypes();
    Option<Type> GetMessageType(string typeKey);
}

public class MessageTypeRegistry : IMessageTypeRegistry
{
    private readonly Dictionary<string, Type> _typeRegistry = new();

    public MessageTypeRegistry()
    {
        RegisterMessageTypes();
    }

    public void RegisterMessageTypes()
    {
        // Get all classes in the current assembly with the MessageType attribute
        var types = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.GetCustomAttribute<SocketMessage>() != null);

        foreach (var type in types)
        {
            var attribute = type.GetCustomAttribute<SocketMessage>();
            if (attribute != null)
            {
                _typeRegistry[attribute.TypeKey] = type;
            }
        }
    }

    public Option<Type> GetMessageType(string typeKey)
    {
        return _typeRegistry.TryGetValue(typeKey, out var type) ? Option.Some(type) : Option.None<Type>();
    }
}