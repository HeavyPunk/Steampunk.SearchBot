using SearchBot.Lib.Logging;

namespace SearchBot.Worker.Jobs;

public interface IJob
{
    public JobStatus Status { get; set; }
    public bool Run(ILog log);
    public bool Stop();
}