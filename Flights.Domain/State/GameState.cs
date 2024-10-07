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
    CricketState? CricketState = null
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
    CricketState? CricketState = null);

public record DartsState(DartState? D1, DartState? D2, DartState? D3);

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
);