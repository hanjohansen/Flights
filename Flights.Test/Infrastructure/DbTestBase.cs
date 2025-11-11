using Flights.Infrastructure.Data;
using Flights.Infrastructure.Data.Repos;
using Flights.Infrastructure.Port;
using Microsoft.EntityFrameworkCore;

namespace Flights.Test.Infrastructure;

public class DbTestBase
{
    public Guid DefaultTenantId = Guid.Parse("13c4ccc6-51fb-45f8-b06b-9ce9d2bbb254");
    
    public IDbContextFactory<FlightsDbContext> CreateFactory(string connStr){
        
        var contextFactory = new DbContextFactory(connStr);
        contextFactory.CreateDbContext().Database.Migrate();
        return contextFactory;
    }

    public IGameRepository CreateGameRepo(string connStr){
        var factory = CreateFactory(connStr);

        return new GameRepository(factory);
    }

    public IPlayerRepository CreatePlayerRepo(string connStr){
        var factory = CreateFactory(connStr);

        return new PlayerRepository(factory);
    }
    
    public ITournamentRepository CreateTournamentRepo(string connStr){
        var factory = CreateFactory(connStr);

        return new TournamentRepository(factory);
    }

    public IStatRepository CreateStatsRepo(string constr)
    {
        var factory = CreateFactory(constr);
        return new StatRepository(factory);
    }
    
}