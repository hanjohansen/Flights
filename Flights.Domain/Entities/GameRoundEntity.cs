namespace Flights.Domain.Entities;
public class GameRoundEntity : BaseEntity
{
    public Guid GameId {get;set;}

    public GameEntity Game {get;set;} = null!;

    public int Number {get;set;}

    public List<RoundStatEntity> RoundStats {get;set;} = new();
}