namespace Flights.Domain.State;

public record GameCountState(
    int? Total,
    int? X01,
    int? Cricket,
    int? CtCricket,
    int? AroundTheClock,
    List<PlayerGameCount> PlayerGames);

public record PlayerGameCount(
    string PlayerName,
    GameCountState GameCount
);

public class PlayerWins
{
    public PlayerWins(string playerName)
    {
        Name = playerName;
    }
    
    public string Name { get; set; }
    public GameWins X01 { get; set; } = new();
    public GameWins Cricket { get; set; } = new();
    public GameWins CtCricket { get; set; } = new();
    public GameWins AroundTheClock { get; set; } = new();
    
    public GameWins Total  { get; set; } = new();
}

public class GameWins
{
    public int First { get; set; }
    public int Second { get; set; }
    public int Third { get; set; }
    public decimal Points { get; set; }
    public int Games { get; set; }
    
    public decimal PointsToGamesRatio { get; set; }
    public decimal FirstToGamesRatio { get; set; }
};