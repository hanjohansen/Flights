using Flights.Domain.Entities.Game;
using Flights.Domain.Models;

namespace Flights.Test;

public class DebugReplay
{
    //[Fact]
    public async Task DoReplay()
    {
        string filePath = "/home/Debug_Data_05_11_24___21_16.csv";

        var data = await new DebugData().LoadData(filePath);
        
        var playerCount = data.Max(x => x.Player);
        var players = new TestHelpers().GetPlayers(playerCount + 1);
        var model = GameModel.Create(players, GameType.CtCricket, false, 0, InOutModifier.None, InOutModifier.None);

        var state = model.SolveGameState();

        
        for (var i = 0; i < data.Count; i++)
        {
            var id = state.CurrentPlayerId;
            if (id == null)
                break;

            var dart = data[i];
            state = model.AddPlayerStats(state.CurrentPlayerId!.Value,
                StatModel.Init(GetModifier(dart.Mod), dart.Value));
        }
    }

    private DartModifier GetModifier(string input)
    {
        switch (input)
        {
            case "N":
                return DartModifier.None;
            case "D":
                return DartModifier.Double;
            case "T":
                return DartModifier.Triple;
        }

        return DartModifier.None;
    }
}