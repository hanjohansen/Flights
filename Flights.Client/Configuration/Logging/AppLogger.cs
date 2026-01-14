using Flights.Client.Authentication;
using Serilog.Context;

namespace Flights.Client.Configuration.Logging;

public class AppLogger<T>(ILogger<T> logger, IAuthenticationService authService)
{
    public void LogInformation(string message, params object[] args)
    {
        Log(LogLevel.Information, null, null, message, args);
    }
    
    public void LogInformation(Guid gameId, string message, params object[] args)
    {
        Log(LogLevel.Information, null, gameId, message, args);
    }

    public void LogError(string message, params object[] args)
    {
        Log(LogLevel.Error, null, null, message, args);
    }
    
    public void LogError(Exception ex, string message, params object[] args)
    {
        Log(LogLevel.Error, ex, null, message, args);
    }
    
    public void LogError(Guid gameId, string message, params object[] args)
    {
        Log(LogLevel.Error, null, gameId, message, args);
    }
    
    public void LogError(Guid gameId, Exception ex, string message, params object[] args)
    {
        Log(LogLevel.Error, ex, gameId, message, args);
    }

    private void Log(LogLevel level, Exception? ex, Guid? gameId, string message,  params object[] args)
    {
        var tenantId = authService.Tenant?.Id.ToString() ?? "";
        var gId = gameId?.ToString() ?? "";
        
        using(LogContext.PushProperty("TenantId", tenantId))
        using (LogContext.PushProperty("GameId", gId))
        {
            logger.Log(level, ex, message, args);
        }
    }
}