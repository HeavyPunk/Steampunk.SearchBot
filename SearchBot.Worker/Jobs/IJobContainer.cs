namespace SearchBot.Worker.Jobs;

public interface IJobContainer : IDisposable
{
    public Guid AddJob(IJob job, TimeSpan respawn);
}