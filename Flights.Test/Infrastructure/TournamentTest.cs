using Flights.Domain.Entities.Game;

namespace Flights.Test.Infrastructure;

public class TournamentTest : DbTestBase
{
    [Fact]
    public async Task TournamentDbTest()
    {
        var conn = "DataSource=/home/hannes/Documents/Code/Flights/Flights.Client/app.db;Cache=Shared;foreign keys=true";
        var tRepo = CreateTournamentRepo(conn);
        var pRepo = CreatePlayerRepo(conn);

        var players = await pRepo.GetPlayers(DefaultTenantId);
        players = players.Take(5).ToList();

        var state = await tRepo.CreateTournament(DefaultTenantId, players.Select(x => x.Id).ToList(), 2,
            GameType.X01, false,301, InOutModifier.None, InOutModifier.Double);

        var model = await tRepo.GetTournament(state.Id);
        state = model.ResolveTournamentState();
        
        Assert.True(state.Id == model.Entity.Id);

        await tRepo.DeleteTournament(model.Entity.Id);
    }

    [Fact]
    public async Task TournamentLoadTest()
    {
        var conn = "DataSource=/home/hannes/Documents/Code/Flights/Flights.Client/app.db;Cache=Shared;foreign keys=true";
        var tRepo = CreateTournamentRepo(conn);

        var model = await tRepo.GetTournament(Guid.Parse("da0ab07e-1f3a-4064-b27f-3e8b424a771c"));
        model.SkipLosersCup();
        
        var state = model.ResolveTournamentState();

        Assert.True(true);
    }
}