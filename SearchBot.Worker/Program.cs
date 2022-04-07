using SearchBot.Configuration;
using SearchBot.Configuration.Args;
using SearchBot.Configuration.Settings;
using SearchBot.Lib.Appenders;
using SearchBot.Lib.Appenders.Database;
using SearchBot.Lib.Appenders.Logging;
using SearchBot.Lib.Logging;
using SearchBot.Worker;
using SearchBot.Worker.Jobs;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<ILog, ConsoleLog>();
        services.AddSingleton<IJobContainer, JobContainer>();
        services.AddSingleton<IAppConfigurator, Configurator>();
        services.AddSingleton<SiteParserConfiguration>();
        services.AddSingleton<ConfiguratorSettings>();
        services.AddSingleton<IAppender, DatabaseAppender>();
    })
    .Build();

await host.RunAsync();