using Flights.Domain.Entities.Game;
using Flights.Domain.Entities.Tournament;

namespace Flights.Domain.Entities;

public class TenantEntity : BaseEntity
{
    public string Name { get; set; } = null!;

    public string Password { get; set; } = null!;

    public List<PlayerEntity> Players { get; set; } = [];
    
    public List<GameEntity> Games { get; set; } = [];
    public List<TournamentEntity> Tournaments { get; set; } = [];
}