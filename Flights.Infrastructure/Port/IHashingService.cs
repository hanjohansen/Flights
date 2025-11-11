namespace Flights.Infrastructure.Port;

public interface IHashingService
{
    string CreateHash(string input);
    
    bool MatchHash(string input, string hash);
}