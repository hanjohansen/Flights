using Flights.Infrastructure.Port;
using Flights.Infrastructure.Security;

namespace Flights.Test.Infrastructure.Security;

public class PasswordHashTests
{
    [Fact]
    public void TestHashing()
    {
        string myPw = "somePasswordOfMine";
        IHashingService hashService = new BCryptor();
            
        string hash = hashService.CreateHash(myPw);
        
        Assert.True(hashService.MatchHash(myPw, hash));
    }
    
    [Fact]
    public void CreateHash()
    {
        string myPw = "garage";
        IHashingService hashService = new BCryptor();
        string hash = hashService.CreateHash(myPw);
    }
}