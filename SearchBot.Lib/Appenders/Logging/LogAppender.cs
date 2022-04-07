using SearchBot.Lib.Logging;

namespace SearchBot.Lib.Appenders.Logging;

public class LogAppender : IAppender
{
    private readonly ILog _log;

    public LogAppender(ILog log)
    {
        _log = log;
    }
    public async Task Add(params string[] fields)
    {
        _log.Info(string.Join(" - ", fields));
    }
}