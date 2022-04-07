using SearchBot.Configuration.Args;
using SearchBot.Lib.Appenders;
using SearchBot.Lib.Logging;
using SearchBot.Worker.Jobs;
using SearchBot.Worker.Jobs.FromUrlUpdater;

namespace SearchBot.Worker;

public class Worker : BackgroundService
{
    private readonly ILog log;
    private SiteParserConfiguration _siteParserConfiguration;
    private readonly IJobContainer jobContainer;
    private readonly IAppender _appender;

    public Worker(
        ILog logger,
        IJobContainer jobContainer,
        SiteParserConfiguration siteParserConfiguration,
        IAppender appender
        )
    {
        log = logger;
        this.jobContainer = jobContainer;
        _siteParserConfiguration = siteParserConfiguration;
        _appender = appender;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var siteUpdater = new SiteUpdater(_siteParserConfiguration, _appender);
        jobContainer.AddJob(siteUpdater, TimeSpan.FromMinutes(10));
        var jobs = new IJob[]
        {
            siteUpdater
        };
        
        while (!stoppingToken.IsCancellationRequested)
        {
            // foreach (var job in jobs)
            //     log.Info($"{job.GetType().Name} status is {job.Status}");
                
            
            await Task.Delay(1000, stoppingToken);
        }
    }
}