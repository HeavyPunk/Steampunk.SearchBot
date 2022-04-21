namespace SearchBot.Lib.DataMiddleware;

public interface IDataMiddleware<TInput, TOutput>
{
    public TOutput Convert(TInput input);
}