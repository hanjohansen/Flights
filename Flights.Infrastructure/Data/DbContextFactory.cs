using Microsoft.EntityFrameworkCore;

namespace Flights.Infrastructure.Data;

public class DbContextFactory(string connStr) : IDbContextFactory<FlightsDbContext>
{
    public FlightsDbContext CreateDbContext()
    {
        var opts = new DbContextOptionsBuilder<FlightsDbContext>()
        .UseSqlite(connStr)
        .Options;

        var db = new FlightsDbContext(opts);

        return db;
    }

}