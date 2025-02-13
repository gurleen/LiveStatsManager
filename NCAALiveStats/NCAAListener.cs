using System.Net.Sockets;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using NCAALiveStats.Messages;
using Newtonsoft.Json;
using Optional;
using Optional.Unsafe;

namespace NCAALiveStats;

public class RawMessage(string objectType, string json)
{
    public string ObjectType { get; } = objectType;
    public string Json { get; } = json;
}

public class NCAAListener(ILogger<NCAAListener> logger, IMessageTypeRegistry messageTypeRegistry, GameState state)
{
    private static JsonSerializerOptions jsonOptions = new()
    {
        Converters = { new BoolConverter() },
    };
    
    private string GetConnectionParameters()
    {
        var parameters = new ConnectionParameters{ types = "se,ac,mi,te,sc,box" };
        var jsonString = JsonConvert.SerializeObject(parameters);
        logger.LogInformation("Connection parameters: {0}", jsonString);
        return jsonString;
    }
    
    public void Start(string address, int port)
    {
        using Socket socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        if (!socket.Connected)
        {
            socket.Connect(address, port);
        }
        using NetworkStream networkStream = new(socket);
        using StreamReader reader = new(networkStream);
        using StreamWriter writer = new(networkStream);
        writer.WriteLine(GetConnectionParameters());
        writer.Flush();

        while (true)
        {
            MessageLoop(reader);
        }
    }

    private static Option<RawMessage> GetMessageType(string jsonMessage)
    {
        using var doc = JsonDocument.Parse(jsonMessage);
        if (!doc.RootElement.TryGetProperty("type", out var typeProperty)) return Option.None<RawMessage>();
        var value = typeProperty.GetString();
        return value != null ? Option.Some(new RawMessage(value, jsonMessage)) : Option.None<RawMessage>();
    }

    private static Option<string> ReadLine(StreamReader reader)
    {
        var line = reader.ReadLine();
        return line == null ? Option.None<string>() : Option.Some(line);
    }

    private static Option<object> ParseObject(Type T, string json) => 
        System.Text.Json.JsonSerializer.Deserialize(json, T, jsonOptions) is { } obj ? 
            Option.Some(obj) : Option.None<object>();
    
    private void MessageLoop(StreamReader reader)
    {
        var identifyResult = ReadLine(reader)
            .FlatMap(GetMessageType);
        if(!identifyResult.HasValue) return;
        var rawMessage = identifyResult.ValueOrFailure();

        logger.LogInformation($"Received message: {rawMessage.ObjectType}");
        
        var parseResult = identifyResult
            .FlatMap(message => messageTypeRegistry.GetMessageType(message.ObjectType))
            .FlatMap(T => ParseObject(T, rawMessage.Json));
        
        logger.LogInformation("Parsed message: {0}", parseResult.HasValue ? "Success" : "Failure");
        parseResult.MatchSome(HandleParsedObject);
    }

    private void HandleParsedObject(object message)
    {
        switch (message)
        {   
            case TeamMessage teamMessage:
                state.UpdateTeams(teamMessage);
                break;
            case Boxscore boxscore:
                state.UpdateBoxscore(boxscore);
                break;
        }
    }
}