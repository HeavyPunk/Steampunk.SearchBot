using System.Text.Json;
using SearchBot.Configuration.Models;
using SearchBot.Configuration.Settings;
using SearchBot.Lib.Logging;

namespace SearchBot.Configuration;

public class Configurator : IAppConfigurator
{
    private readonly ConfiguratorSettings _settings;
    private DateTime lastUpdate = DateTime.MinValue;
    private Config cache = new();
    private ILog log;

    public Configurator(ConfiguratorSettings settings, ILog logger)
    {
        _settings = settings;
        log = logger;
    }

    public async Task<Config> GetConfig()
    {
        if (DateTime.Now - lastUpdate <= _settings.UpdateTime)
            return cache;
        cache = await UpdateConfig().ConfigureAwait(false);
        lastUpdate = DateTime.Now;
        return cache;
    }

    private async Task<Config> UpdateConfig()
    {
        if (!File.Exists(_settings.ConfigPath))
        {
            log.Error($"Configurator cannot find file {_settings.ConfigPath}");
            return cache;
        }
        using (var stream = File.Open(_settings.ConfigPath, FileMode.Open))
        {
            var config = JsonSerializer.Deserialize<Config>(stream);
            cache = config;
        }

        return cache;
    }
}