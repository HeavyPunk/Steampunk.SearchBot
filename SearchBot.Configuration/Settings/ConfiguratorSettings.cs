namespace SearchBot.Configuration.Settings;

public class ConfiguratorSettings
{
    public string ConfigPath { get; set; } =
        $"{Environment.CurrentDirectory}/config.json";
    public TimeSpan UpdateTime { get; set; } = TimeSpan.FromSeconds(1);
}