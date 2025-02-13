using LiveStatsService.Components;
using LiveStatsService.Services;
using LiveStatsService.Utilities;
using MudBlazor.Services;
using NCAALiveStats;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.PropertyNamingPolicy = new ActualPropertyNamePolicy();
});
builder.Services.AddMudServices();

builder.Services.AddSingleton<GameState>();
builder.Services.AddSingleton<IMessageTypeRegistry, MessageTypeRegistry>();
builder.Services.AddSingleton<NCAAListener>();
builder.Services.AddHostedService<LiveStatsListener>();

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy  =>
        {
            policy.AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed(_ => true)
                .AllowCredentials();
        });
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapControllers();
app.UseCors(MyAllowSpecificOrigins);

app.Run();