namespace Flights.Client.Configuration;

public class StorageConfiguration
{
    public const string ConfigurationKey = "Storage";
    public string DbProvider { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
}