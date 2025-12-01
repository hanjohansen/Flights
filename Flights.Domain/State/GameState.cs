using Flights.Domain.Entities.Game;

namespace Flights.Domain.State;
public record GameState(
    Guid Id,
    Guid? TournamentId,
    GameType Type,
    bool FinishAfterFirstRank,
    InOutModifier InModifier,
    InOutModifier OutModifier,
    DateTimeOffset Started,
    int Round,
    bool Finished,
    Guid? CurrentPlayerId,
    List<PlayerState> PlayerStates,
    CricketState? CricketState = null,
    AroundTheClockGameState? AroundTheClockState = null
);

public record PlayerState(
    Guid PlayerId,
    string PlayerName,
    bool IsIn,
    bool IsBust,
    int? Rank,
    int Points,
    DartsState? Darts,
    X01State? X01State,
    List<DartsState>? Checkouts = null,
    CricketState? CricketState = null,
    AroundTheClockState? AroundTheClockState = null);

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

    public static DartsState Create(DartModifier modifier, int value){
        return new DartsState(DartState.Create(modifier, value), null, null);
    }

    public static DartsState Create(DartModifier mod1, int val1, DartModifier mod2, int val2){
        return new DartsState(DartState.Create(mod1, val1), DartState.Create(mod2, val2), null);
    }

    public static DartsState Create(DartModifier mod1, int val1, DartModifier mod2, int val2, DartModifier mod3, int val3){
        return new DartsState(DartState.Create(mod1, val1), DartState.Create(mod2, val2), DartState.Create(mod3, val3));
    }
    
    public int Sum(){
        return GetDartsList().Sum(x => x.Calculated);
    }
}

public record DartState(
    DartModifier Modifier,
    int Value,
    int Calculated
){
    public static DartState FromEntity(DartStatEntity entity){
        return new DartState(entity.Modifier, entity.Value, entity.GetCalculatedValue());
    }

    public static DartState Create(DartModifier mod, int value){
        return new DartState(mod, value, GetCalculatedValue(mod, value));
    }

    public static int GetCalculatedValue(DartModifier mod, int value){
        switch(mod){
            case DartModifier.Double:
                return value * 2;
            case DartModifier.Triple:
                return value * 3;
            case DartModifier.None:
            default:
                return value;
        }
    }
}

public record X01State(
    decimal PointAvg,
    int PointMax,
    bool Has60,
    bool Has120,
    bool Has180,
    int WashmachineCount,
    int Misses
);

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

public record AroundTheClockGameState(int CurrentTarget);

public record AroundTheClockState(
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