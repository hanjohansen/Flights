using Flights.Domain.State;

namespace Flights.Test.Cricket;

public class CricketSimBase
{
    protected TestHelpers Helpers = new();

    protected const int MaxGameDarts = 2000; 
    protected const int SimRounds = 1000;

    protected List<int> GetDartOptions(CricketState playerState, List<CricketState> otherPlayersState)
    {
        var result = playerState.AllOpen()
            ? GetNonClosedDartsOptions(otherPlayersState)
            : GetNonClosedDartsOptions(playerState);
        
        result.Add(0);

        return result;
    }

    private List<int> GetNonClosedDartsOptions(CricketState state)
    {
        var result = new List<int>();
        
        if(state.V15 < CricketValue.Closed)
            result.Add(15);

        if(state.V16 < CricketValue.Closed)
            result.Add(16);        

        if(state.V17 < CricketValue.Closed)
            result.Add(17);
        
        if(state.V18 < CricketValue.Closed)
            result.Add(18);
        
        if(state.V19 < CricketValue.Closed)
            result.Add(19);
        
        if(state.V20 < CricketValue.Closed)
            result.Add(20);
        
        if(state.Bulls < CricketValue.Closed)
            result.Add(25);

        return result;
    }
    
    private List<int> GetNonClosedDartsOptions(List<CricketState> states)
    {
        var result = new List<int>();
        
        if(states.Any(x => x.V15 < CricketValue.Open))
            result.Add(15);

        if(states.Any(x => x.V16 < CricketValue.Open))
            result.Add(16);        

        if(states.Any(x => x.V17 < CricketValue.Open))
            result.Add(17);
        
        if(states.Any(x => x.V18 < CricketValue.Open))
            result.Add(18);
        
        if(states.Any(x => x.V19 < CricketValue.Open))
            result.Add(19);
        
        if(states.Any(x => x.V20 < CricketValue.Open))
            result.Add(20);
        
        if(states.Any(x => x.Bulls < CricketValue.Open))
            result.Add(25);

        return result;
    }
    
    protected void DoFinishAsserts(GameState state)
    {
        Assert.True(state.Finished);
        Assert.DoesNotContain(state.PlayerStates, x => x.Rank == null);

        var maxRank = state.PlayerStates.Max(x => x.Rank!.Value);
        Assert.True(maxRank <= state.PlayerStates.Count);

        var openPlayers = state.PlayerStates.Where(x => x.CricketState!.AllOpen() == false).ToList();
        var openCount = openPlayers.Count;
        
        if(state.FinishAfterFirstRank == false)
            Assert.True(openCount <= 1);

        if (openCount == 0)
            return;
        
        if(state.FinishAfterFirstRank == false)
            Assert.True(openPlayers.Single().Rank == state.PlayerStates.Count);
    }
}