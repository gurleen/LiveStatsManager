using Havit.Blazor.Components.Web;
using LiveStatsManager.Components;
using LiveStatsManager.Components.Pages.Wrestling;
using LiveStatsManager.FileWatcher;
using LiveStatsManager.Hubs;
using LiveStatsManager.Services;
using LiveStatsManager.Services.AllSport;
using LiveStatsManager.Services.DataStore;
using LiveStatsManager.Services.FileWriter;
using LiveStatsManager.Services.Graphics;
using LiveStatsManager.Utilities.StartupTasks;
using NCAALiveStats;
using Shared.GameState;
using Shared.Objects;
using Shared.Services;

namespace LiveStatsManager;

public static class StartupExtensions
{
    private const string CorsPolicy = "_corsPolicy";
    
    public static void AddExternalServices(this IServiceCollection services)
    {
        services.AddRazorComponents()
            .AddInteractiveServerComponents();
        services.AddRazorPages();
        services.AddHxServices();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddHxMessenger();
        services.AddSignalR();
    }
    
    public static void AddBackgroundServices(this IServiceCollection services)
    {
        services.AddHostedService<FileTracker>();
        services.AddHostedService<LiveStatsListener>();
        services.AddHostedService<AllSportListener>();
        services.AddHostedService<FileWriterService>();
    }

    public static void AddUtilityServices(this IServiceCollection services)
    {
        services.AddSingleton<SettingsProvider>();
        services.AddSingleton<Database>();
        services.AddSingleton<DatabaseRepository>();
        services.AddSingleton<ServiceStatusTracker>();
    }

    public static void AddStatsServices(this IServiceCollection services)
    {
        services.AddSingleton<IMessageTypeRegistry, MessageTypeRegistry>();
        services.AddSingleton<RawGameState>();
        services.AddSingleton<LocalTeamData>();
        services.AddSingleton<TeamDataRepository>();
        services.AddSingleton<NCAAListener>();
        services.AddSingleton<AppState>();
        services.AddSingleton<CurrentGameState>();
    }

    public static void AddGraphicsServices(this IServiceCollection services)
    {
        services.AddSingleton<IDataStore, DataStore>();
        services.AddSingleton<GraphicsManager>();
    }

    public static void AddViewModels(this IServiceCollection services)
    {
        services.AddSingleton<WrestlingViewModel>();
    }

    public static void AddStartupTasks(this IServiceCollection services)
    {
        services.AddTransient<IStartupTask, ESPNLoaderStartupTask>();
    }
    
    public static void ApplyCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(name: CorsPolicy,
                policy  =>
                {
                    policy.AllowAnyHeader()
                        .AllowAnyMethod()
                        .SetIsOriginAllowed(_ => true)
                        .AllowCredentials();
                });
        });
    }

    public static void AddMiddlewares(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseAntiforgery();
        app.UseCors(CorsPolicy);
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    public static void AddMappings(this WebApplication app)
    {
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();
        app.MapRazorPages();
        app.MapControllers();
        app.MapHub<LiveDataHub>("/LiveData");
    }

    public static async Task ExecuteStartupTasks(this IServiceProvider services)
    {
        var startupTasks = services.GetServices<IStartupTask>();
        foreach(var startupTask in startupTasks) await startupTask.Execute();
    }
}