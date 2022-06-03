using System.Reflection;

namespace SearchBot.Lib.DataMiddleware;

public class FilePathReplacerMiddleware : IDataMiddleware<string, string>
{
    private static readonly Dictionary<string, string> replacements = new()
    {
        {"WorkingDirectory", Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}
    };
        
    public string Convert(string input)
    {
        var res = input;
        foreach (var replacement in replacements)
            res = input.Replace('{' + replacement.Key + '}', replacement.Value);
        

        return res;
    }
}