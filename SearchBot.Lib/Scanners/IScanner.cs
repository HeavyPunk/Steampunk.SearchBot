using SearchBot.Worker.Jobs;

namespace SearchBot.Lib.Scanners;

public interface IScanner<TSource, TResult>
{
    public TResult Scan(TSource source, ArgsContainer<TSource> args);
}