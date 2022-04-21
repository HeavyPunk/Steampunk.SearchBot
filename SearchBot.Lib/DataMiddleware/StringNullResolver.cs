namespace SearchBot.Lib.DataMiddleware;

public class StringNullResolver : IDataMiddleware<string, string>
{
    public string Convert(string input)
    {
        return string.IsNullOrEmpty(input)
            ? "Not known"
            : input;
    }
}