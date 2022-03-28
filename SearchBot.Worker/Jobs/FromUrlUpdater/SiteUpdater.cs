using SearchBot.Lib.Logging;
using SearchBot.Lib.Scanners.SiteScanner;
using SearchBot.Lib.Threading;

namespace SearchBot.Worker.Jobs.FromUrlUpdater;

public class SiteUpdater : IJob
{
    private JobStatus _status;

    public JobStatus Status
    {
        get => workingTask?.IsCompleted ?? true ? JobStatus.Stopped : _status;
        set => _status = value;
    }

    private Task workingTask;
    private CancellationTokenSource cts = new();
    private SiteScanner siteScanner = new();
    private StringArgsProvider _stringArgsProvider = new();

    private async Task Work(ArgsContainer<string> urlsContainer, ILog log)
    {
        var urls = urlsContainer.Arguments.Select(arg => arg.Value);
        var isGood = await TaskHelper.WaitFor(
            () => ScanUrl(urls.First(), log),
            TimeSpan.FromSeconds(10),
            log
        );
        Task.WaitAll(
            urls.Select(url => TaskHelper.WaitFor(() => ScanUrl(url, log), TimeSpan.FromSeconds(10), log)).ToArray()
            );
        log.Info($"Is scan success: {isGood}");
    }

    private bool ScanUrl(string url, ILog log)
    {
        var id = Guid.NewGuid();
        log.Info($"Scanning for {url}, scan id = {id}");
        var res = siteScanner.Scan(url, new ArgsContainer<string>
        {
            Arguments = new List<Argument<string>>
            {
                new() {Value = "Please read"}
            }
        });

        foreach (var pair in res)
        {
            log.Info($"{pair.Key} - {pair.Value}");
        }
        
        log.Info($"Scanning {id} is stopped");
        return true;
    }
    
    public bool Run(ILog log)
    {
        if (workingTask is { IsCompleted: false })
            throw new Exception("Job is already running");
        
        workingTask = Task.Run(() => Work(_stringArgsProvider.GetArgs(), log), cts.Token);
        Status = JobStatus.Running;
        return true;
    }

    public bool Stop()
    {
        if (workingTask is { IsCompleted: true })
            throw new Exception("Job is already stopped");
        
        cts.Cancel();
        Status = JobStatus.Stopped;
        cts = new CancellationTokenSource();
        return true;
    }
}