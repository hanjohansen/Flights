using Microsoft.EntityFrameworkCore;

namespace Flights.Infrastructure.Data;

public class TestingDbContextFactory : IDbContextFactory<FlightsDbContext>{

    public TestingDbContextFactory(string connStr){
        _connStr = connStr;
    }

    private readonly string _connStr;

    public FlightsDbContext CreateDbContext()
    {
        var opts = new DbContextOptionsBuilder<FlightsDbContext>()
        .UseSqlite(_connStr)
        .Options;

        var db = new FlightsDbContext(opts);

        return db;
    }

}