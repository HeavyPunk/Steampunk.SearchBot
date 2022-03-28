using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using SearchBot.Configuration.Models;
using SearchBot.Configuration.Settings;

namespace SearchBot.Configuration;

public class Configurator
{
    private readonly ConfiguratorSettings _settings;
    private DateTime lastUpdate = DateTime.MinValue;
    private Config cache = new();
    private ILogger<Configurator> logger;

    public Configurator(ConfiguratorSettings settings, ILogger<Configurator> logger)
    {
        _settings = settings;
        this.logger = logger;
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
            logger.LogError($"Configurator cannot find file {_settings.ConfigPath}");
            return cache;
        }
        using (var stream = File.Open(_settings.ConfigPath, FileMode.Open))
        {
            var config = JsonSerializer.Deserialize<Config>(stream, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });
            cache = config;
        }

        return cache;
    }
}