using Flights.Domain.Entities;

namespace Flights.Domain.State.Solvers.Shanghai;

public class ShanghaiSolver : IGameSolver
{
    private readonly GameEntity _game;

    public ShanghaiSolver(GameEntity game)
    {
        _game = game;
    }

    public GameState Solve()
    {
        var playerStates = GetPlayerStates();
        var rounds = _game.Rounds.Count;
        var finished = _game.Rounds.Count == 7
            && _game.Rounds.Last()
                .RoundStats.All(x => x.GetDartsList().Count == 3);

        if(finished){
            _game.Finished = DateTimeOffset.UtcNow;
            playerStates = CalculateRanks(playerStates);
            var roundStats = _game.Rounds.Last().RoundStats;
            foreach(var roundStat in roundStats){
                var playerState = playerStates.First(x => x.PlayerId == roundStat.Player.Id);
                roundStat.Rank = playerState.Rank;
            }
        }

        Guid? currentPlayerId = null;
        var currentTarget = 0;

        if(!finished){
            currentTarget = (_game.Rounds.Count - 1) * 3;
            var currentPlayer = _game.Rounds.Last()
                .RoundStats.FirstOrDefault(x => x.GetDartsList().Count < 3);

            if(currentPlayer == null){
                currentPlayer = _game.Rounds.Last().RoundStats.First();
                currentTarget += 4;
            }else{
                currentTarget += currentPlayer.GetDartsList().Count + 1;
            }

            if(currentTarget == 21)
                currentTarget = 25;

            currentPlayerId = currentPlayer.Player.Id;

            playerStates = playerStates.Select(x => {
                if(x.PlayerId == currentPlayerId){
                    var newTarget = x.ShanghaiState!.CurrentTarget == 20
                        ? x.ShanghaiState!.CurrentTarget + 5
                        : x.ShanghaiState!.CurrentTarget +1;
                    return x with {ShanghaiState = x.ShanghaiState! with {CurrentTarget = newTarget}};
                }
                return x;
            }).ToList();
        }

        return new GameState(
            Id: _game.Id,
            Type: _game.Type,
            InModifier: InOutModifier.None,
            OutModifier: InOutModifier.None,
            Started: _game.Started,
            Round: rounds,
            Finished: finished,
            CurrentPlayerId: currentPlayerId,
            PlayerStates: playerStates,
            CricketState: null,
            ShanghaiState: new ShanghaiGameState(currentTarget));
    }

    private List<PlayerState> GetPlayerStates(){
        var playerDtos = _game.Players
            .Select(x => new ShanghaiStateDto(){PlayerId = x.Player.Id, CurrentTarget = 1})
            .ToList();

        foreach(var round in _game.Rounds){
            var intTargetNumber = (_game.Rounds.IndexOf(round) * 3) + 1;

            foreach(var playerRound in round.RoundStats){
                var playerDto = playerDtos.First(x => x.PlayerId == playerRound.Player.Id);
                ProcessPlayerRound(playerDto, playerRound, intTargetNumber);

                playerRound.EndPoints = playerDto.PointsSum();
            }
        }

        var states = new List<PlayerState>();
        foreach(var dto in playerDtos){
            var player = _game.Players.First(x => x.Player.Id == dto.PlayerId);
            var state = new PlayerState(
                PlayerId: player.Player.Id,
                PlayerName: player.Player.Name,
                IsIn: true,
                IsBust: false,
                Rank: null,
                Points: dto.Points,
                PointAvg: 0,
                Darts: dto.Darts,
                null,
                null,
                dto.ToState());
            states.Add(state);
        }

        return states;
    }

    private void ProcessPlayerRound(ShanghaiStateDto playerState, RoundStatEntity playerRound, int targetNumber){
        if(targetNumber > 25)
            return;

        DartState? first = null;
        DartState? second = null;
        DartState? third = null;

        if(playerRound.FirstDart != null)
            first = DartState.FromEntity(playerRound.FirstDart);

        if(playerRound.SecondDart != null)
            second = DartState.FromEntity(playerRound.SecondDart);

        if(playerRound.ThirdDart != null)
            third = DartState.FromEntity(playerRound.ThirdDart);

        playerState.Darts = new DartsState(first, second, third);   
                      
        var darts = playerRound.GetDartsList();
        var intTargetNumber = targetNumber;

        foreach(var dart in darts){
            if(intTargetNumber > 25)
                return;
            var setProp = typeof(ShanghaiStateDto).GetProperties().First(x => x.Name == "V" + intTargetNumber); 
            setProp.SetValue(playerState, 0);

            if(dart.Value == intTargetNumber){
                var points = dart.GetCalculatedValue();           
                
                setProp.SetValue(playerState, points);
                playerState.Points += points;
            }

            playerState.CurrentTarget = intTargetNumber;

            intTargetNumber = intTargetNumber == 20
                ? intTargetNumber + 5
                : intTargetNumber + 1;
        }
    }

    private List<PlayerState> CalculateRanks(List<PlayerState> playerStates){
        var result = new List<PlayerState>();
        var playerPoints = playerStates.Select(x => x.Points)
            .Distinct()
            .OrderByDescending(x => x)
            .ToList();
        
        foreach(var player in playerStates){
            var rank = playerPoints.IndexOf(player.Points) + 1;
            result.Add(player with {Rank = rank});
        }

        return result;
    }
}