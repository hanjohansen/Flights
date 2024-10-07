namespace Flights.Domain.State.Solvers.Cricket;

public class CricketStateDto
{
    public Guid PlayerId {get;set;}

    public int? Rank {get;set;}

    public int Points {get;set;}

    public DartsState? Darts {get;set;}

    public CricketValue V15 {get;set;} =  CricketValue.None;
    public CricketValue V16 {get;set;} =  CricketValue.None;
    public CricketValue V17 {get;set;} =  CricketValue.None;
    public CricketValue V18 {get;set;} =  CricketValue.None;
    public CricketValue V19 {get;set;} =  CricketValue.None;
    public CricketValue V20 {get;set;} =  CricketValue.None;
    public CricketValue Bulls {get;set;} =  CricketValue.None;

    public CricketState GetCricketState(){
        return new CricketState(
            V15,
            V16,
            V17,
            V18,
            V19,
            V20,
            Bulls
        );
    }

    public bool AllOpen(){
        return V15 >= CricketValue.Open
            && V16 >= CricketValue.Open
            && V17 >= CricketValue.Open
            && V18 >= CricketValue.Open
            && V19 >= CricketValue.Open
            && V20 >= CricketValue.Open
            && Bulls >= CricketValue.Open;
    }
}