using LiveStatsManager.Services.DataStore;

namespace LiveStatsManager.FileWatcher;

public class FileTracker : IHostedService
{
    private readonly string Path;
    private readonly FileSystemWatcher _watcher;
    private readonly IDataStore dataStore;

    public FileTracker(IConfiguration config, IDataStore dataStore)
    {
        Path = config.GetValue<string>("LiveDataDirectory") ?? "/LiveData";
        Directory.CreateDirectory(Path);
        _watcher = new FileSystemWatcher(Path);
        this.dataStore = dataStore;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Initialize();
        
        _watcher.Changed += OnChanged;
        _watcher.NotifyFilter = NotifyFilters.LastWrite;
        _watcher.EnableRaisingEvents = true;
        
        return Task.CompletedTask;
    }
    
    private void Initialize()
    {
        var files = Directory.GetFiles(Path).Where(FileValid);
        foreach (var file in files)
        {
            ParseFile(file);
        }
    }

    private void ParseFile(string filename)
    {
        try
        {
            var records = CsvParser.Parse(filename);
            dataStore.Add(records);
        }
        catch(Exception e)
        {
            // Log error
        }
    }
    
    private static bool FileValid(string filename) => filename.EndsWith(".csv");
    
    private void OnChanged(object source, FileSystemEventArgs args)
    {
        Console.WriteLine($"File: {args.FullPath} {args.ChangeType}");
        var filename = args.FullPath;
        if (!FileValid(filename)) return;
        ParseFile(filename);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}