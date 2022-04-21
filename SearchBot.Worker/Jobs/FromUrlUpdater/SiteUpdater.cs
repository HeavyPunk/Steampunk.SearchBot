using SearchBot.Configuration.Args;
using SearchBot.Lib.Appenders;
using SearchBot.Lib.Config;
using SearchBot.Lib.DataMiddleware;
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
    private SiteParserConfiguration _siteParserConfiguration;
    private IAppender _appender;

    private IList<IDataMiddleware<string, string>> middlewares = new List<IDataMiddleware<string, string>>
    {
        new StringErrorResolver(),
        new StringNullResolver(),
    };

    public SiteUpdater(SiteParserConfiguration siteParserConfiguration, IAppender appender)
    {
        _siteParserConfiguration = siteParserConfiguration;
        _appender = appender;
    }

    private async Task Work(ArgsContainer<ConfigFile> urlsContainer, ILog log)
    {
        var configFiles = urlsContainer.Arguments.Select(arg => arg.Value).First();
        Task.WaitAll(
            configFiles.Nodes.Select(node => TaskHelper.WaitFor(() => ScanUrl(node, log), TimeSpan.FromSeconds(10), log)).ToArray()
            );
        log.Info($"Is scan success: {true}");
    }

    private bool ScanUrl(Node node, ILog log)
    {
        var id = Guid.NewGuid();
        log.Info($"Scanning for {node.Href}, scan id = {id}");
        var res = siteScanner.Scan(node, null);

        try
        {
            foreach (var (question, answer) in res)
            {
                var resQuestion = GoToMiddleware(question);
                var resAnswer = GoToMiddleware(answer);
                _appender.Add(resQuestion, resAnswer);
            }
        }
        catch (Exception e)
        {
            log.Error(e.ToString());
        }


        log.Info($"Scanning {id} is stopped");
        return true;
    }

    private string GoToMiddleware(string input)
    {
        var res = input;
        foreach (var middleware in middlewares)
            res = middleware.Convert(res);
        return res;
    }
    
    public async Task<bool> Run(ILog log)
    {
        if (workingTask is { IsCompleted: false })
            throw new Exception("Job is already running");
        
        workingTask = Task.Run(async () => Work(await _siteParserConfiguration.GetArgs(), log), cts.Token);
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