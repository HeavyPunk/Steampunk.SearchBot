namespace SearchBot.Lib.DataMiddleware;

public class StringErrorResolver : IDataMiddleware<string, string>
{
    public string Convert(string input)
    {
        return input
            .Replace('\"', '\'') //TODO: возможно лишнее
            .Replace(' ', ' ')
            .Replace("\n", "");
    }
}