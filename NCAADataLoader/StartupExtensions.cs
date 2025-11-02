using Microsoft.Extensions.DependencyInjection;
using NCAADataLoader.Loaders;
using NCAALiveStats.ExternalData.StatCrew;

namespace NCAADataLoader;

public static class StartupExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddLogging();
        services.AddDbContext<StatsContext>();
        services.AddScoped<NCAALoader>();
    }
}