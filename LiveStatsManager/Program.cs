using LiveStatsManager;

var app = await AppStartup.BuildApp(args);
app.Run();
Console.WriteLine("Press any key to exit...");
Console.ReadKey();