using SearchBot.Lib.Logging;

namespace SearchBot.Lib.Threading;

public class TaskHelper
{
    public static async Task<TReturn> WaitFor<TReturn>(Func<TReturn> awaitable, TimeSpan timeout, ILog log)
    {
        var cts = new CancellationTokenSource(timeout);
        var task = Task<TReturn>.Run(awaitable, cts.Token);
        TReturn res = default;
        try
        {
            res = await task;
        }
        catch (Exception e)
        {
            log.Warn(e.Message);
        }

        return res;
    }
}