namespace Flights.Domain.Entities.Tournament;

public class TournamentEntity : BaseEntity
{
    public DateTimeOffset Started {get;set;}

    public DateTimeOffset? Finished {get;set;}

    public List<TournamentPlayerEntity> Players { get; set; } = new();

    public List<TournamentRoundEntity> Rounds { get; set; } = new();
}