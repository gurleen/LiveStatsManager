using System.Text;
using Microsoft.Extensions.Logging;

namespace AllSportListener;

public class AllSportParser(ILogger<AllSportParser> logger)
{
    private readonly SerialPortInput SerialPort = new();
    private string? OutputFile;

    public void Start(string comPort, string? outputFile)
    {
        OutputFile = outputFile;
        logger.LogInformation("Starting AllSportParser");
        SerialPort.MessageReceived += MessageReceived;
        SerialPort.ConnectionStatusChanged += ConnectionStatusChanged;
        while (true)
        {
            Console.ReadLine();
        }
        // ReSharper disable once FunctionNeverReturns
    }

    private void ConnectionStatusChanged(object sender, ConnectionStatusChangedEventArgs args)
    {
        logger.LogInformation($"Connection status changed to Connected = {args.Connected}");
    }

    private void MessageReceived(object sender, MessageReceivedEventArgs args)
    {
        var message = Encoding.ASCII.GetString(args.Data);
        logger.LogInformation($"Message received: {message}");
    }
}