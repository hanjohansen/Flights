using Flights.Domain.Entities;

namespace Flights.Domain.State;
public record GameState(
    Guid Id,
    GameType Type,
    InOutModifier InModifier,
    InOutModifier OutModifier,
    DateTimeOffset Started,
    int Round,
    bool Finished,
    Guid? CurrentPlayerId,
    List<PlayerState> PlayerStates,
    CricketState? CricketState = null,
    ShanghaiGameState? ShanghaiState = null
);

public record PlayerState(
    Guid PlayerId,
    string PlayerName,
    bool IsIn,
    bool IsBust,
    int? Rank,
    int Points,
    decimal PointAvg,
    DartsState? Darts,
    DartsState? Checkout = null,
    CricketState? CricketState = null,
    ShanghaiState? ShanghaiState = null);

public record DartsState(DartState? D1, DartState? D2, DartState? D3){
    public int RemainingDarts(){
        if(D1 is null)
            return 3;
        if(D2 is null)
            return 2;
        if(D3 is null)
            return 1;
        return 0;
    }

    public List<DartState> GetDartsList(){
        var result = new List<DartState>();
        if(D1 != null)
            result.Add(D1);
        if(D2 != null)
            result.Add(D2);
        if(D3 != null)
            result.Add(D3);

        return result;
    }
};

public record DartState(
    DartModifier Modifier,
    int Value,
    int Calculated
){
    public static DartState FromEntity(DartStatEntity entity){
        return new DartState(entity.Modifier, entity.Value, entity.GetCalculatedValue());
    }
};

public enum CricketValue {None, Single, Double, Open, Closed}

public record CricketState(
    CricketValue V15 = CricketValue.None,
    CricketValue V16 = CricketValue.None,
    CricketValue V17 = CricketValue.None,
    CricketValue V18 = CricketValue.None,
    CricketValue V19 = CricketValue.None,
    CricketValue V20 = CricketValue.None,
    CricketValue Bulls = CricketValue.None
){
    public bool AllClosed(){
        return V15 == CricketValue.Closed
            && V16 == CricketValue.Closed
            && V17 == CricketValue.Closed
            && V18 == CricketValue.Closed
            && V19 == CricketValue.Closed
            && V20 == CricketValue.Closed
            && Bulls == CricketValue.Closed;
    }
};

public record ShanghaiGameState(int CurrentTarget);

public record ShanghaiState(
    int CurrentTarget,
    int? V1,
    int? V2,
    int? V3,
    int? V4,
    int? V5,
    int? V6,
    int? V7,
    int? V8,
    int? V9,
    int? V10,
    int? V11,
    int? V12,
    int? V13,
    int? V14,
    int? V15,
    int? V16,
    int? V17,
    int? V18,
    int? V19,
    int? V20,
    int? Bulls);