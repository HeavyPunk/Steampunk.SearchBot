namespace SearchBot.Configuration.Settings;

public class ConfiguratorSettings
{
    public string ConfigPath { get; set; } =
        "/home/blackpoint/RiderProjects/Steampunk.SearchBot/SearchBot.Configuration/config.json";
    public TimeSpan UpdateTime { get; set; } = TimeSpan.FromSeconds(1);
}