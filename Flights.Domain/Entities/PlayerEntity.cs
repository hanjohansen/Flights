namespace Flights.Domain.Entities;
public class PlayerEntity : BaseEntity
{
    public string Name {get;set;} = null!;

    public List<GamePlayerEntity> Games {get;set;} = new();
}