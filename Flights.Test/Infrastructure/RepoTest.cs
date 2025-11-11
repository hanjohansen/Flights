using Flights.Infrastructure.Data;
using Flights.Infrastructure.Data.Repos;
using Flights.Infrastructure.Port;

namespace Flights.Test.Infrastructure;

public class RepoTest
{
    private IGameRepository GetGameRepo(){
        var conn = "DataSource=/home/hanjo/Documents/Code/Flights/Flights.Client/app.db;Cache=Shared;foreign keys=true";
        var factory = new DbContextFactory(conn);

        return new GameRepository(factory);
    }

    //[Fact]
    public async Task RunOrderTest(){
        var repo = GetGameRepo();

        var games = await repo.GetGames(Guid.Empty);

        var model = await repo.GetGame(Guid.Parse("C9A63575-7F40-4990-B5E8-1A48820B398C"));
        model.SolveGameState();

        Assert.True(games.Count > 0);
    }    
}