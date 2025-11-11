
namespace Flights.Test.Infrastructure;

public class StatRepoTest : DbTestBase
{
    [Fact]
    public async Task StatsTest()
    {
        // "DataSource=D:\\Code\\Flights\\Flights.Client\\app.db;Cache=Shared;foreign keys=true";
        var conn = "DataSource=/home/hannes/Documents/Code/Flights/Flights.Test/Infrastructure/TestDbs/app__15_5_25.db;Cache=Shared;foreign keys=true";

        var statsRepo = CreateStatsRepo(conn);

        var results = await statsRepo.GetTotalPlayerWins(DefaultTenantId);
    }
}