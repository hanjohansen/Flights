using System.Diagnostics;
using Flights.Domain.Entities;
using Flights.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Flights.Test.Infrastructure;
public class CricketFinishTest : DbTestBase
{
    const string TestDb = "DataSource=/home/hanjo/Documents/Code/Flights/Flights.Test/Infrastructure/TestDbs/flights-app-17-10-24.db;Cache=Shared;foreign keys=true";

    [Fact]
    public async Task LoadRaw(){
        var gId = Guid.Parse("7CF7717C-6120-4136-B193-C16374DCB50F");

        var context = await CreateFactory(TestDb).CreateDbContextAsync();
        await context.Database.MigrateAsync();

        var game = await context.Games
            .AsNoTrackingWithIdentityResolution()
            .AsSplitQuery()
            .Include(x => x.Players)
            .ThenInclude(x => x.Player)
            .Include(x => x.Rounds.OrderBy(y => y.Number))
            .ThenInclude(x => x.RoundStats.OrderBy(y => y.OrderNumber))
            .ThenInclude(x => x.Player)
            .FirstAsync(x => x.Id == gId);
        
        var players = game.Rounds.First().RoundStats.Select(x => x.Player).ToList();

        var model = GameModel.Create(
            players, 
            GameType.Cricket,
            0,
            InOutModifier.None,
            InOutModifier.None);

        var state = model.SolveGameState();

        foreach(var round in game.Rounds){
            Debug.WriteLine("Round: " + round.Number);
            foreach(var roundStat in round.RoundStats){
                Debug.WriteLine("Player: " + roundStat.OrderNumber);
                foreach(var dart in roundStat.GetDartsList())
                    state = model.AddPlayerStats(roundStat.PlayerId, StatModel.Init(dart.Modifier, dart.Value));
            }
               
        }

        var player = game.Players.First(x => x.Player.Id == state.CurrentPlayerId);
        Assert.True(player.Player.Name == "Björn");

        model.AddPlayerStats(state.CurrentPlayerId!.Value, StatModel.Init(25));
    }
}