namespace Flights.Domain.State;

public record GameCountState(
    int? Total,
    int? X01,
    int? Cricket,
    int? CtCricket,
    int? Shanghai,
    List<PlayerGameCount> PlayerGames);

public record PlayerGameCount(
    string PlayerName,
    GameCountState GameCount
);