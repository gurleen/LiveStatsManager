using AllSportListener;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var services = new ServiceCollection();
services.AddLogging(builder => builder.AddConsole());
services.AddSingleton<AllSportParser>();
IServiceProvider serviceProvider = services.BuildServiceProvider();

Parser.Default.ParseArguments<CommandLineArgs>(args).WithParsed(options =>
{
    var parser = serviceProvider.GetRequiredService<AllSportParser>();
    parser.Start(options.ComPort, options.OutputFile);
});