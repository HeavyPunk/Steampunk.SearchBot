using System.Collections.Concurrent;
using SearchBot.Lib.Logging;

namespace SearchBot.Worker.Jobs;

public class JobContainer : IJobContainer
{
    private ConcurrentDictionary<Guid, IJob> container;
    private ConcurrentDictionary<Guid, (DateTime, TimeSpan)> respawnContainer;
    private Task updater;
    private CancellationTokenSource cts = new();
    private ILog log;

    public JobContainer(ILog log)
    {
        this.container = new ConcurrentDictionary<Guid, IJob>();
        this.respawnContainer = new ConcurrentDictionary<Guid, (DateTime, TimeSpan)>();
        updater = Task.Run(Update, cts.Token);
        this.log = log;
    }

    private async Task Update()
    {
        while (true)
        {
            foreach (var pair in respawnContainer)
            {
                var id = pair.Key;
                var (lastUpdate, respawn) = pair.Value;
                var canGetJob = container.TryGetValue(id, out var job);
                if (DateTime.Now - lastUpdate > respawn && canGetJob && job.Status == JobStatus.Stopped)
                {
                    await job.Run(log);
                    respawnContainer.TryUpdate(id, (DateTime.Now, respawn), (lastUpdate, respawn));
                }
            }

            await Task.Delay(100);
        }
    }
    
    public Guid AddJob(IJob job, TimeSpan respawn)
    {
        var id = Guid.NewGuid();
        container.TryAdd(id, job);
        respawnContainer.TryAdd(id, (DateTime.MinValue, respawn));
        return id;
    }

    public void Dispose()
    {
        cts.Cancel();
        updater.Dispose();
    }
}