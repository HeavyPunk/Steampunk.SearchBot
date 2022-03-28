using SearchBot.Lib.Logging;
using SearchBot.Worker;
using SearchBot.Worker.Jobs;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<ILog, ConsoleLog>();
        services.AddSingleton<IJobContainer, JobContainer>();
    })
    .Build();

await host.RunAsync();