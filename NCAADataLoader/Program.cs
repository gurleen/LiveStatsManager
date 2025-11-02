using System.Text.Json;
using Microsoft.Extensions.Hosting;
using NCAADataLoader;
using NCAALiveStats.ExternalData.StatCrew;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddServices();
var app = builder.Build();

var filePath = @"/Users/gurleen/Downloads/bbgame.xml";
var loader = new StatCrewBasketballParser(filePath);
var gameState = await loader.Load();

var jsonString = JsonSerializer.Serialize(gameState);
File.WriteAllText("/Users/gurleen/Desktop/bbgame.json", jsonString);