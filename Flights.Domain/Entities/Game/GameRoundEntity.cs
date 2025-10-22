namespace Flights.Domain.Entities.Game;
public class GameRoundEntity : BaseEntity
{
    public Guid GameId {get;set;}

    public GameEntity Game {get;set;} = null!;

    public int Number {get;set;}

    public List<RoundStatEntity> RoundStats {get;set;} = new();

    public bool HasDarts()
    {
        return RoundStats.Any(x => x.AnyDartThrown());
    }
}