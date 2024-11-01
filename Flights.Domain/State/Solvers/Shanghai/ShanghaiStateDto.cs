namespace Flights.Domain.State.Solvers.Shanghai;

public class ShanghaiStateDto
{
    public Guid PlayerId {get;set;}

    public DartsState Darts {get;set;} = new(null, null, null);

    public int Points {get;set;}

    public int CurrentTarget {get;set;}

    public int? V1 {get;set;}
    public int? V2 {get;set;}
    public int? V3 {get;set;}
    public int? V4 {get;set;}
    public int? V5 {get;set;}
    public int? V6 {get;set;}
    public int? V7 {get;set;}
    public int? V8 {get;set;}
    public int? V9 {get;set;}
    public int? V10 {get;set;}
    public int? V11 {get;set;}
    public int? V12 {get;set;}
    public int? V13 {get;set;}
    public int? V14 {get;set;}
    public int? V15 {get;set;}
    public int? V16 {get;set;}
    public int? V17 {get;set;}
    public int? V18 {get;set;}
    public int? V19 {get;set;}
    public int? V20 {get;set;}
    public int? V25 {get;set;}

    public ShanghaiState ToState(){
        return new ShanghaiState(
            CurrentTarget,
            V1,
            V2,
            V3,
            V4,
            V5,
            V6,
            V7,
            V8,
            V9,
            V10,
            V11,
            V12,
            V13,
            V14,
            V15,
            V16,
            V17,
            V18,
            V19,
            V20,
            V25
        );
    }

    public int PointsSum(){
        var sum = V1 +  V2 +  V3 + V4 + V5 + V6 + V7 + V8 + V9 + V10 + 
                V11 + V12 + V13 + V14 + V15 + V16 + V17 + V18 + V19 + V20 + 
                V25;

        return sum ?? 0;
    }
}