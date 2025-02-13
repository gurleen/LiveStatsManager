using System.IO.Ports;
using System.Text;

namespace LiveStatsManager.Services.AllSport;

public class SerialPortReader : IDisposable
{
    private readonly SerialPort _serialPort;
    private readonly string _lineTerminator;
    private CancellationTokenSource _cts;

    public SerialPortReader(string portName, int baudRate, string lineTerminator = "\n")
    {
        if (string.IsNullOrEmpty(lineTerminator))
            throw new ArgumentException("Line terminator cannot be null or empty.", nameof(lineTerminator));

        _serialPort = new SerialPort(portName, baudRate)
        {
            Encoding = Encoding.UTF8,
            NewLine = lineTerminator,
            ReadTimeout = 5000,
            WriteTimeout = 5000
        };
        _lineTerminator = lineTerminator;
        _cts = new CancellationTokenSource();
    }

    public void Open()
    {
        if (!_serialPort.IsOpen)
            _serialPort.Open();
    }

    public void Close()
    {
        if (_serialPort.IsOpen)
            _serialPort.Close();
    }

    public async Task ReadAsync(Func<string, Task> onLineReceived)
    {
        if (!_serialPort.IsOpen)
            throw new InvalidOperationException("Serial port is not open.");

        _cts = new CancellationTokenSource();
        var buffer = new StringBuilder();

        await Task.Run(async () =>
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                try
                {
                    var incomingData = _serialPort.ReadExisting();
                    if (!string.IsNullOrEmpty(incomingData))
                    {
                        buffer.Append(incomingData);
                        var lineIndex = buffer.ToString().IndexOf(_lineTerminator, StringComparison.Ordinal);

                        while (lineIndex >= 0)
                        {
                            var line = buffer.ToString(0, lineIndex);
                            buffer.Remove(0, lineIndex + _lineTerminator.Length);
                            await onLineReceived(line);
                            lineIndex = buffer.ToString().IndexOf(_lineTerminator, StringComparison.Ordinal);
                        }
                    }
                }
                catch (TimeoutException)
                {
                    // Ignore timeout, continue reading
                }
            }
        }, _cts.Token);
    }

    public void StopReading()
    {
        _cts.Cancel();
    }

    public void Dispose()
    {
        _cts?.Cancel();
        _serialPort?.Dispose();
    }
}