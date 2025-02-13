using System.Net.Sockets;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using NCAALiveStats.Messages;
using Newtonsoft.Json;
using Optional;
using Optional.Unsafe;
using Shared.Services;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace NCAALiveStats;

public class RawMessage(string objectType, string json)
{
    public string ObjectType { get; } = objectType;
    public string Json { get; } = json;
}

public class NCAAListener(ILogger<NCAAListener> logger, IMessageTypeRegistry messageTypeRegistry, RawGameState state,
    ServiceStatusTracker status)
{
    private bool ShouldLogData = false;
    
    private static JsonSerializerOptions jsonOptions = new()
    {
        Converters = { new BoolConverter(), new JsonStringEnumConverter() },
    };

    private string GetConnectionParameters()
    {
        var parameters = new ConnectionParameters { Types = "se,ac,mi,te,sc,box,st,pbp" };
        var jsonString = JsonSerializer.Serialize(parameters);
        logger.LogInformation("Connection parameters: {0}", jsonString);
        return jsonString;
    }

    public void Start(string address, int port, bool logData = false)
    {
        ShouldLogData = logData;
        if (ShouldLogData)
        {
            EnsureLogDirectoryExists();
            logger.LogInformation($"Writing incoming data to {LogLocation}");
        }
        
        try
        {
            using Socket socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            if (!socket.Connected)
            {
                socket.Connect(address, port);
            }
            
            status.SetStatus<NCAAListener>(ServiceStatus.Running);

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
        catch (Exception e)
        {
            logger.LogError(e, "Error in NCAAListener");
            status.SetStatus<NCAAListener>(ServiceStatus.NotRunning);
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
        System.Text.Json.JsonSerializer.Deserialize(json, T, jsonOptions) is { } obj
            ? Option.Some(obj)
            : Option.None<object>();

    private void MessageLoop(StreamReader reader)
    {
        var identifyResult = ReadLine(reader)
            .FlatMap(GetMessageType);
        if (!identifyResult.HasValue) return;
        var rawMessage = identifyResult.ValueOrFailure();
        
        if(ShouldLogData)
            LogMessage(rawMessage);

        logger.LogInformation($"Received message: {rawMessage.ObjectType}");

        var parseResult = identifyResult
            .FlatMap(message => messageTypeRegistry.GetMessageType(message.ObjectType))
            .FlatMap(T => ParseObject(T, rawMessage.Json));

        logger.LogInformation("Parsed message: {0}", parseResult.HasValue ? "Success" : "Failure");
        parseResult.MatchSome(HandleParsedObject);
    }

    private static string AppData => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    private const string AppName = "NCAALiveStats";
    private const string LogFolderName = "LoggedMessages";
    private static string LogLocation => Path.Combine(AppData, AppName, LogFolderName);
    private static void EnsureLogDirectoryExists() => Directory.CreateDirectory(LogLocation);

    private static void LogMessage(RawMessage message)
    {
        if(message.ObjectType == "ping") return;
        var filename = $"{message.ObjectType}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.json";
        var path = Path.Combine(LogLocation, filename);
        File.WriteAllText(path, message.Json);
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
            case MatchStatus matchStatus:
                state.UpdateStatus(matchStatus);
                break;
        }
    }
}