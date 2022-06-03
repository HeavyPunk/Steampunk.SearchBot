using System.Text.Json;
using SearchBot.Lib.Config;
using SearchBot.Lib.DataMiddleware;
using SearchBot.Worker.Jobs;

namespace SearchBot.Configuration.Args;

public class SiteParserConfiguration
{
    private readonly IAppConfigurator _configurator;
    private readonly IDataMiddleware<string, string> _filePathResolver = new FilePathReplacerMiddleware();
    private ConfigFile _file;
    
    public SiteParserConfiguration(IAppConfigurator configurator)
    {
        _configurator = configurator;
    }

    private async Task<ConfigFile> ParseFile(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"The config file {path} cannot be found");
        using (var stream = File.Open(path, FileMode.Open))
        {
            var config = await JsonSerializer.DeserializeAsync<ConfigFile>(stream, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });
            return config;
        }
    }

    public async Task<ArgsContainer<ConfigFile>> GetArgs()
    {
        var appSettings = await _configurator.GetConfig();
        var config = await ParseFile(_filePathResolver.Convert(appSettings.ResourceFilePath));
        var result = new ArgsContainer<ConfigFile>
        {
            Arguments = new List<Argument<ConfigFile>>{new(){Value = config}}
        };
        return result;
    }
}