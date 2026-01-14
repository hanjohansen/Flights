namespace Flights.Client.Configuration.Logging;

public class RequestExceptionMiddleware(RequestDelegate next, ILogger<RequestExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occured in request to '{0}'", context.Request.Path);
        }
    }
}