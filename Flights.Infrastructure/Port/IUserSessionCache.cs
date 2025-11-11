namespace Flights.Infrastructure.Port;

public interface IUserSessionCache
{
    Task CacheUserSession(Guid userId);

    bool HasUserSession(Guid userId);

    void RemoveUserSession(Guid userId);
}