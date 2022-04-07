using System.Text;

namespace SearchBot.Lib.Appenders.File;

public class CommonFileAppender : IAppender
{
    private string _filePath;
    
    public CommonFileAppender(string filePath)
    {
        _filePath = filePath;
    }
    
    public async Task Add(params string[] fields)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        using (var file = System.IO.File.Open(_filePath, FileMode.Append))
        {
            var value = $"{fields[0]} - {fields[1]}";
            var encoded = Encoding.Unicode.GetBytes(value);
            await file.WriteAsync(encoded, cts.Token);
        }
    }
}