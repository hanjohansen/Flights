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
    List<PlayerState> PlayerStates
);

public record PlayerState(
    Guid PlayerId,
    string PlayerName,
    bool IsIn,
    bool IsBust,
    int? Rank,
    int Points,
    decimal PointAvg,
    DartsState? Darts);

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