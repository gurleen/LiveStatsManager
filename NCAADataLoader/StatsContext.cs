using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NCAADataLoader.Models;

namespace NCAADataLoader;

public class StatsContext : DbContext
{
    public DbSet<Team> Teams { get; set; }
    private string DbPath { get; }
    private ILogger<StatsContext> _logger { get; }

    public StatsContext(ILogger<StatsContext> logger)
    {
        _logger = logger;
        const Environment.SpecialFolder folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        _logger.LogInformation($"Database path: {path}");
        DbPath = Path.Join(path, "ncaa_stats.db");
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}