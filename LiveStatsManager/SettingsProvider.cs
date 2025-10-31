namespace LiveStatsManager;

public class LiveStatsSettings
{
    public bool Enabled { get; set; } = false;
    public string IpAddress { get; set; } = string.Empty;
    public int Port { get; set; } = 7677;
    public string ConnectionParams { get; set; } = "st,se,ac,mi,te,sc,box";
    public bool LogIncomingData { get; set; } = false;
}

public class DatabaseSettings
{
    public string DBFilename { get; set; } = string.Empty;
}

public class AllSportSettings
{
    public bool Enabled { get; set; } = false;
    public string ComPort { get; set; } = string.Empty;
    public int BaudRate { get; set; } = 9600;
    public bool MockEnabled { get; set; } = false;
}

public class FileWriterSettings
{
    public bool Enabled { get; set; } = false;
    public string OutputDirectory { get; set; } = string.Empty;
    public int UpdateIntervalMinutes { get; set; } = 5;
}

public class SettingsProvider
{
    public readonly LiveStatsSettings LiveStatsSettings = new();
    public readonly DatabaseSettings DatabaseSettings = new();
    public readonly AllSportSettings AllSportSettings = new();
    public readonly FileWriterSettings FileWriterSettings = new();
    public readonly string LiveDataDirectory;
    
    public SettingsProvider(IConfiguration config)
    {
        config.GetRequiredSection(nameof(LiveStatsSettings)).Bind(LiveStatsSettings);
        config.GetRequiredSection(nameof(DatabaseSettings)).Bind(DatabaseSettings);
        config.GetRequiredSection(nameof(AllSportSettings)).Bind(AllSportSettings);
        config.GetRequiredSection(nameof(FileWriterSettings)).Bind(FileWriterSettings);
        LiveDataDirectory = config.GetValue<string>("LiveDataDirectory") ?? string.Empty;
    }
}