using Flights.Domain.Entities;
using Flights.Util;

namespace Flights.Domain.Models;

public class TournamentModel
{
    private TournamentModel(List<PlayerEntity> players)
    {
        _players = players;
        _tournament = new Tournament();
    }
    
    private readonly List<PlayerEntity> _players;
    private readonly Tournament _tournament;
    
    public static TournamentModel Create(List<PlayerEntity> players)
    {
        var model = new TournamentModel(players);

        return model;
    }

    public void Init()
    {
        _players.Shuffle();
        var playerModulo = _players.Count % 2;
    }
}

public class Tournament
{
    public List<TournamentRound> Rounds { get; set; } = new();
}

public class TournamentRound
{
    public PlayerEntity? WildCard { get; set; } = new();
    public List<TournamentGame> Games { get; set; } = new();
}

public class TournamentGame
{
    public List<TournamentGamePlayer> Players { get; set; } = new();
}

public class TournamentGamePlayer
{
    public TournamentGamePlayer(PlayerEntity player)
    {
        Player = player;
    }
    
    public PlayerEntity Player { get; }
    
    public bool IsWinner { get; set; }
}