using Flights.Infrastructure.Data;
using Flights.Infrastructure.Data.Repos;
using Flights.Infrastructure.Port;
using Microsoft.EntityFrameworkCore;

namespace Flights.Test.Infrastructure;

public class DbTestBase
{
    public IDbContextFactory<FlightsDbContext> CreateFactory(string connStr){
        return new DbContextFactory(connStr);
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
    
}