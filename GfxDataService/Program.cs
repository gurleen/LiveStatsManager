using GfxDataService.DataStore;
using GfxDataService.FileWatcher;
using GfxDataService.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<FileTracker>();
builder.Services.AddSingleton<IDataStore, DataStore>();
builder.Services.AddControllers();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(corsBuilder =>
{
    corsBuilder.WithOrigins("https://gfx.dragonstv.io")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
});
app.MapControllers();
app.MapHub<LiveDataHub>("/LiveData");
app.UseHttpsRedirection();


app.Run("http://localhost:5069");