using Flights.Domain.Entities.Game;
using Flights.Domain.Entities.Tournament;

namespace Flights.Domain.Entities;
public class PlayerEntity : BaseEntity
{
    public Guid TenantId { get; set; }
    public TenantEntity Tenant { get; set; } = null!;
    
    public string Name {get;set;} = null!;

    public bool Deleted {get;set;}

    public List<GamePlayerEntity> Games {get;set;} = new();
    
    public List<TournamentPlayerEntity> Tournaments {get;set;} = new();

    public List<PlayerFileEntity> Files {get;set;} = new();
}