using SearchBot.Configuration.Models;

namespace SearchBot.Configuration;

public interface IAppConfigurator
{
    public Task<Config> GetConfig();
}