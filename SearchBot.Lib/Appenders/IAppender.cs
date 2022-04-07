namespace SearchBot.Lib.Appenders;

public interface IAppender
{
    Task Add(params string[] fields);
}