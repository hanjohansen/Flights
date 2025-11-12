namespace Flights.Client.Configuration;

public class AuthConfiguration
{
    public const string ConfigurationKey = "Auth";
    
    public bool AllowAutoLogin { get; set; }
}