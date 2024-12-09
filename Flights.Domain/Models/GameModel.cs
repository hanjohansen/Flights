using Flights.Domain.Entities;
using Flights.Domain.Entities.Game;
using Flights.Domain.Exception;
using Flights.Domain.State;
using Flights.Domain.State.Solvers;

namespace Flights.Domain.Models;
public class GameModel
{
    public static GameModel Create(List<PlayerEntity> players, GameType type, bool finishAfterFirstRank, int x01Target, InOutModifier inMod, InOutModifier outMod){

        var gamePlayers = players.Select(p => new GamePlayerEntity
        {
            Player = p,
            OrderNumber = players.IndexOf(p)
            }).ToList();

        var model = new GameModel(new GameEntity
        {
            Type = type,
            X01Target = x01Target,
            InModifier = inMod,
            OutModifier = outMod,
            FinishAfterFirstRank = finishAfterFirstRank,
            Players = gamePlayers,
            Started = DateTimeOffset.UtcNow
        });

        model.CreateNewRound();
        model.SolveGameState();
        return model;
    }

    public static GameModel FromEntity(GameEntity entity) => new(entity);
    private GameModel(GameEntity entity){
        _gameEntity = entity;
        _gameSolver = GameSolverFactory.GetGameSolver(entity);
    }

    private readonly GameEntity _gameEntity;
    private readonly IGameSolver _gameSolver;

    public GameEntity Entity => _gameEntity;

    public GameState SolveGameState(){
        return _gameSolver.Solve();
    }

    public GameState AddPlayerStats(Guid playerId, StatModel dart1, StatModel dart2, StatModel dart3){
        AddPlayerStats(playerId, dart1);
        AddPlayerStats(playerId, dart2);
        return AddPlayerStats(playerId, dart3);
    }

    public GameState AddPlayerStats(Guid playerId, StatModel dart){
        var playerStat = GetCurrentPlayer();

        if(playerStat.Player.Id != playerId){
            throw new FlightsGameException("Its " + playerStat.Player.Name + "'s turn!");
        }

        dart.Validate();
        var dartStat = new DartStatEntity {Modifier=dart.Modifier, Value = dart.Value};

        if(playerStat.FirstDart == null){
            playerStat.FirstDart = dartStat;
        }else if(playerStat.SecondDart == null){
            playerStat.SecondDart = dartStat;
        }else if(playerStat.ThirdDart == null){
            playerStat.ThirdDart = dartStat;
        }

        return _gameSolver.Solve();
    }

    private GameRoundEntity EnsureCurrentRound(){
        if(!_gameEntity.Rounds.Any()){
            return CreateNewRound();
        }

        var last = _gameEntity.Rounds.Last();
        if(last.RoundStats
            .All(x => (x.FirstDart != null && x.SecondDart != null && x.ThirdDart != null) || 
                x.IsBust ||
                x.Rank != null)){
            return CreateNewRound();
        }

        return last;
    }

    private GameRoundEntity CreateNewRound(){
        var newRound = new GameRoundEntity();
        newRound.Number = _gameEntity.Rounds.Count + 1;

        var lastRound = _gameEntity.Rounds.LastOrDefault();
        if(lastRound == null){
            foreach(var player in _gameEntity.Players){
                newRound.RoundStats.Add(new RoundStatEntity
                {
                    Player = player.Player,
                    OrderNumber = _gameEntity.Players.IndexOf(player),
                });
            }
        }
        else{
            if(lastRound.RoundStats.All(x => x.Rank != null))
                throw new FlightsGameException("Game is finished!");

            foreach(var stat in lastRound.RoundStats){
                newRound.RoundStats.Add(new RoundStatEntity
                {
                    OrderNumber = stat.OrderNumber,
                    Player = stat.Player,
                    StartPoints = stat.EndPoints,
                    Rank = stat.Rank,
                    IsIn = stat.IsIn
                });
            }
        }

        _gameEntity.Rounds.Add(newRound);
        return newRound;
    }
    
    private RoundStatEntity GetCurrentPlayer(){
        var round = EnsureCurrentRound();
        var remainingPlayers = round.RoundStats
            .Where(x => (x.FirstDart == null || x.SecondDart == null || x.ThirdDart == null) &&
                x.IsBust == false &&
                x.Rank == null).ToList();

        if(remainingPlayers.Count == 0){
            throw new FlightsGameException("Game is finished!");
        }

        return remainingPlayers.First(x => 
            x.ThirdDart == null || x.SecondDart == null || x.FirstDart == null);
    }

    public GameState RevertLastDart(){
        var lastRound = Entity.Rounds.Last();

        var removed = false;

        for(var i = lastRound.RoundStats.Count - 1; i >= 0; i--){
            var stat = lastRound.RoundStats[i];

            var darts = stat.GetDartsList();
            
            if(darts.Count == 0){continue;}

            if(stat.ThirdDart != null)
            {
                stat.ThirdDart = null;
                removed = true;
                break;
            }

            if(stat.SecondDart != null)
            {
                stat.SecondDart = null;
                removed = true;
                break;
            }

            if(stat.FirstDart != null)
            {
                stat.FirstDart = null;
                removed = true;
                if(i == 0)
                    if(Entity.Rounds.Count > 1)
                        Entity.Rounds.Remove(lastRound);
                break;
            }
        }

        if(!removed)
            throw new FlightsGameException("No more Darts to remove!");

        return SolveGameState();
    }
}