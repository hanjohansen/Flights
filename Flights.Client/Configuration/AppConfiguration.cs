namespace Flights.Client.Configuration;

public class AppConfiguration
{
    public const string ConfigurationKey = "App";
    
    public bool UseLogging { get; set; }

    public bool UseRequestLogging { get; set; }
}