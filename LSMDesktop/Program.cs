using LiveStatsManager;

var app = await AppStartup.BuildApp(args);
var serverThread = new Thread(() => app.Run());

