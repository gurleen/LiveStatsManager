using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NCAADataLoader;
using NCAADataLoader.Loaders;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddServices();
var app = builder.Build();

var loader = app.Services.GetRequiredService<NCAALoader>();
await loader.LoadAll();