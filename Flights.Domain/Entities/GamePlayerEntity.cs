namespace Flights.Domain.Entities;

public class GamePlayerEntity : BaseEntity
{
    public Guid GameId {get;set;}

    public GameEntity Game {get;set;} = null!;

    public Guid PlayerId {get;set;}

    public PlayerEntity Player {get;set;} = null!;

    public int OrderNumber {get;set;}
}