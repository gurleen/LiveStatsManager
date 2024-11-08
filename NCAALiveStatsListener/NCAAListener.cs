using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.Logging;
using NCAALiveStatsListener.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SocketMeister;

namespace NCAALiveStatsListener;

public class NCAAListener(ILogger<NCAAListener> logger)
{
    public Team? HomeTeam { get; set; }
    public Team? AwayTeam { get; set; }
    
    public BoxScore BoxScore { get; set; }
    public string RawBox { get; set; }
    
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
        
        MessageLoop(reader);
    }

    private Type? MessageType(string jsonString)
    {
        JObject json = JObject.Parse(jsonString);
        string type = json["type"]?.ToString() ?? string.Empty;
        logger.LogInformation($"Got message type: {type}");
        return type switch
        {
            "teams" => typeof(TeamMessage),
            "boxscore" => typeof(BoxScore),
            _ => null
        };
    }

    private void MessageLoop(StreamReader reader)
    {
        while (true)
        {
            var message = reader.ReadLine();
            if (message == null)
            {
                break;
            }
            var messageType = MessageType(message);
            if (messageType == null) continue;
            var messageObject = JsonConvert.DeserializeObject(message, messageType);
            switch (messageObject)
            {
                case TeamMessage teamMessage:
                    HandleTeamMessage(teamMessage);
                    break;
                case BoxScore boxScore:
                    BoxScore = boxScore;
                    RawBox = message;
                    break;
            }
            logger.LogInformation("Message object: {0}", messageObject);
        }
    }
    
    private void HandleTeamMessage(TeamMessage teamMessage)
    {
        HomeTeam = teamMessage.Teams.Find(t => t.Detail.IsHomeCompetitor);
        AwayTeam = teamMessage.Teams.Find(t => !t.Detail.IsHomeCompetitor);
    }

    private string GetConnectionParameters()
    {
        var parameters = new ConnectionParameters{ types = "se,ac,mi,te,sc,box" };
        var jsonString = JsonConvert.SerializeObject(parameters);
        logger.LogInformation("Connection parameters: {0}", jsonString);
        return jsonString;
    }
}