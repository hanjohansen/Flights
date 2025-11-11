using Flights.Infrastructure.Port;

namespace Flights.Infrastructure.Security;

public class BCryptor : IHashingService
{
    public string CreateHash(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);
    }

    public bool MatchHash(string password, string hash)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
    }
}