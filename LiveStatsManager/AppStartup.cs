namespace LiveStatsManager;

public static class AppStartup
{
    public static async Task<WebApplication> BuildApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddExternalServices();
        builder.Services.AddBackgroundServices();
        builder.Services.AddUtilityServices();
        builder.Services.AddStatsServices();
        builder.Services.AddGraphicsServices();
        builder.Services.AddViewModels();
        builder.Services.AddStartupTasks();
        builder.Services.ApplyCorsPolicy();

        var app = builder.Build();
        app.AddMiddlewares();
        app.AddMappings();

        await app.Services.ExecuteStartupTasks();

        app.Urls.Add("http://localhost:5000");
        app.Urls.Add("http://localhost:5069");
        
        return app;
    }

    public static async Task<WebApplication> BuildApp()
    {
        return await BuildApp([]);
    }
}