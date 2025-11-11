using Flights.Infrastructure.Port;
using Microsoft.Extensions.Caching.Memory;

namespace Flights.Infrastructure.Security;

public class UserSessionCache(IMemoryCache memCache) : IUserSessionCache
{
    public async Task CacheUserSession(Guid userId)
    {
        await memCache.GetOrCreateAsync(userId, entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromHours(12);
            return Task.FromResult(userId);
        });
    }

    public bool HasUserSession(Guid userId)
    {
        return memCache.TryGetValue(userId, out _);
    }

    public void RemoveUserSession(Guid userId)
    {
        if(HasUserSession(userId))
            memCache.Remove(userId);
    }
}