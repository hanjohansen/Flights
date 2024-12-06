using Flights.Domain.Entities.Game;

namespace Flights.Domain.Entities;
public class PlayerEntity : BaseEntity
{
    public string Name {get;set;} = null!;

    public bool Deleted {get;set;}

    public List<GamePlayerEntity> Games {get;set;} = new();

    public List<PlayerFileEntity> Files {get;set;} = new();
}