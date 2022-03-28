using Microsoft.Extensions.Logging;

namespace SearchBot.Lib.Logging;

public class ConsoleLog : ILog
{
    private readonly ILogger<ConsoleLog> _baseLogger;

    public ConsoleLog(ILogger<ConsoleLog> baseLogger)
    {
        _baseLogger = baseLogger;
    }

    public void Info(string message)
    {
        if (message != null) _baseLogger.LogInformation(message);
    }

    public void Warn(string message)
    {
        if (message != null) _baseLogger.LogInformation(message);
    }

    public void Debug(string message)
    {
        if (message != null) _baseLogger.LogInformation(message);
    }

    public void Error(string message)
    {
        if (message != null) _baseLogger.LogInformation(message);
    }
}