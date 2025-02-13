using LiteDB;

namespace LiveStatsManager;

public class Database
{
    private readonly DatabaseSettings _settings;
    private string ConnectionString => _settings.DBFilename;

    public Database(IServiceProvider services)
    {
        var settingsProvider = services.GetRequiredService<SettingsProvider>();
        _settings = settingsProvider.DatabaseSettings;
    }

    public ILiteDatabase Session() => new LiteDatabase(ConnectionString);
}