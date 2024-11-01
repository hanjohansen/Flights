using Flights.Domain.State;

namespace Flights.Test.Cricket;

public class CricketSimBase
{
    protected TestHelpers Helpers = new();

    protected const int MaxGameDarts = 2000; 
    protected const int SimRounds = 500; 

    protected List<int> GetDartOptions(CricketState state){
        var result = new List<int>();
        result.Add(0);

        if(state.AllClosed()){
            result.Add(15);
            result.Add(16);
            result.Add(17);
            result.Add(18);
            result.Add(19);
            result.Add(20);
            result.Add(25);

            return result;
        }


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

    protected void DoFinishAsserts(GameState state)
    {
        Assert.True(state.Finished);
        Assert.DoesNotContain(state.PlayerStates, x => x.Rank == null);

        var openPlayers = state.PlayerStates.Where(x => x.CricketState!.AllOpen() == false).ToList();
        var openCount = openPlayers.Count;
        Assert.True(openCount <= 1);

        if (openCount == 0)
            return;
        
        Assert.True(openPlayers.Single().Rank == state.PlayerStates.Count);
    }
}