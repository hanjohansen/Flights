using Flights.Client.Service.Port;

namespace Flights.Client.Service;

public class ThemeInfoProvider : IThemeInfoProvider
{
    public bool IsDarkMode { get; set; }
}