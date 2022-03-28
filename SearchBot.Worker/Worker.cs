using SearchBot.Lib.Logging;
using SearchBot.Worker.Jobs;
using SearchBot.Worker.Jobs.FromUrlUpdater;

namespace SearchBot.Worker;

public class Worker : BackgroundService
{
    private readonly ILog log;
    private readonly IJobContainer jobContainer;

    public Worker(ILog logger, IJobContainer jobContainer)
    {
        log = logger;
        this.jobContainer = jobContainer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var siteUpdater = new SiteUpdater();
        jobContainer.AddJob(siteUpdater, TimeSpan.FromMinutes(1));
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