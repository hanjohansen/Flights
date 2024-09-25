using Flights.Domain.Entities;

namespace Flights.Domain.State.Solvers;
public class X01Solver : IGameSolver
{
    public X01Solver(GameEntity entity){
        _game = entity;
    }
    private readonly GameEntity _game;
    public GameState Solve(){
        var isGameStart = IsGameStart();

        if(isGameStart)
            InitFirstRound();
        else
            InitLastFromPreviousRound();

        CalculateLastRound();

        var playerStates = GetPlayerState();
        var finished = playerStates.All(x => x.Rank != null);
        
        _game.Finished = finished
            ? DateTimeOffset.UtcNow
            : null;
            
        Guid? nextPlayerId = GetNextPlayerId();
        if(!finished && nextPlayerId == null){
            nextPlayerId = playerStates.First().PlayerId;
            playerStates = playerStates.Select(x => x with { Darts = new DartsState(null, null, null)})
                .ToList();
        }
            
        return new GameState(
            _game.Id,
            _game.Type,
            _game.InModifier,
            _game.OutModifier,
            _game.Started,
            _game.Rounds.Count,
            finished,
            nextPlayerId,
            playerStates);
    }

    private bool IsGameStart(){
        return _game.Rounds.Count == 1 ;
    }

    private void InitFirstRound(){
        _game.Rounds
            .First()
            .RoundStats.ForEach(x => {
                x.StartPoints = _game.X01Target;
                x.IsIn = false;});
    }

    private void InitLastFromPreviousRound(){
        var last = _game.Rounds.Last();
        var lastIndex = _game.Rounds.IndexOf(last);
        var previous = _game.Rounds[lastIndex - 1];

        for(int i = 0; i < previous.RoundStats.Count; i++){
            var currentStat = last.RoundStats[i];
            var lastStat = previous.RoundStats[i];
            currentStat.IsIn = lastStat.IsIn;
            currentStat.Rank = lastStat.Rank;

            currentStat.EndPoints = currentStat.StartPoints;
        }
    }

    private void CalculateLastRound(){
        var lastRound = _game.Rounds.Last();

        foreach(var stat in lastRound.RoundStats)
            CaclulatePlayerPoints(stat);
    }

    private void CaclulatePlayerPoints(RoundStatEntity stats){
        if(stats.Rank != null)
            return;

        var darts = stats.GetDartsList();
        stats.EndPoints = stats.StartPoints;
        var points = stats.StartPoints;

        foreach(var dart in darts){
            
            if(!stats.IsIn){
                var qualifies = DartQualifiesForInOut(_game.InModifier, dart);
                if(qualifies)
                    stats.IsIn = true;
                else
                    continue;
            }
            
            var newPoints = points - dart.GetCalculatedValue();

            if(newPoints == 0)
                if(DartQualifiesForInOut(_game.OutModifier, dart)){
                    points = newPoints;
                    SetRank(stats);
                    break;
                }else{
                    stats.IsBust = true;
                    return;
                }

            if(newPoints < GetBustBorder(_game.OutModifier)){
                stats.IsBust = true;
                return;
            }

            points = newPoints;
        }

        stats.EndPoints = points;
    }

    public bool DartQualifiesForInOut(InOutModifier modifier, DartStatEntity dart){
        switch(modifier){
            case InOutModifier.None:
                return true;
            case InOutModifier.Double:
                return dart.Modifier == DartModifier.Double;
            case InOutModifier.Triple:
                return dart.Modifier == DartModifier.Triple;
            case InOutModifier.FullBull:
                return dart.Value == 25 && dart.Modifier == DartModifier.Double;
            default:
                return false;
        };
    }

    public int GetBustBorder(InOutModifier modifier){
        switch(modifier){
            case InOutModifier.Double:
                return 2;
            case InOutModifier.Triple:
                return 3;
            case InOutModifier.FullBull:
                return 25;
            case InOutModifier.None:
            default:
                return 0;
        };
    }

    private void SetRank(RoundStatEntity player){
        var round = _game.Rounds.Last();
        var rank = round.RoundStats
            .Where(x => x.Player.Id != player.Player.Id)
            .Max(x => x.Rank ?? 0);

        player.Rank = rank + 1;

        var unranked = round.RoundStats.Where(x => x.Rank == null).ToList();
        if(unranked.Count==1){
            unranked.Single().Rank = _game.Players.Count;
        }
    }

    private List<PlayerState> GetPlayerState(){
        var stateResult = new List<PlayerState>();
        var lastRound = _game.Rounds.Last();
        foreach(var playerStats in lastRound.RoundStats){
            DartsState? dartState = null;
         
            var first = playerStats.FirstDart != null
                ? DartState.FromEntity(playerStats.FirstDart)
                : null;
            var second = playerStats.SecondDart != null
                ? DartState.FromEntity(playerStats.SecondDart)
                : null;
            var third = playerStats.ThirdDart != null
                ? DartState.FromEntity(playerStats.ThirdDart)
                : null;

            dartState = new DartsState(first, second, third);            
            
            stateResult.Add(new PlayerState(
                playerStats.Player.Id,
                playerStats.Player.Name,
                playerStats.IsIn,
                playerStats.IsBust,
                playerStats.Rank,
                playerStats.EndPoints,
                CountPlayerAverage(playerStats.Player.Id),
                dartState));
        }

        return stateResult;
    }

    private Guid? GetNextPlayerId(){
        var lastRound = _game.Rounds.Last();
        var players = lastRound.RoundStats;

        if(players.All(x => x.Rank != null))
            return null;

        var remainingPlayers = players
            .Where(x => (x.FirstDart == null || x.SecondDart == null || x.ThirdDart == null) &&
                x.IsBust == false &&
                x.Rank == null).ToList();

        var nextPlayer = remainingPlayers.FirstOrDefault(x => 
            x.ThirdDart == null || x.SecondDart == null || x.FirstDart == null);

        return nextPlayer != null
            ? nextPlayer.Player.Id
            : null;
    }

    private decimal CountPlayerAverage(Guid playerId){
        var allRounds = _game.Rounds.SelectMany(x => x.RoundStats)
            .Where(x => x.Player.Id == playerId)
            .ToList();

        var allPoints = allRounds.Select(x => x.StartPoints - x.EndPoints).Sum();

        if(allRounds.Count == 0)
            return 0;

        decimal avg = allPoints / allRounds.Count;
        return Math.Round(avg, 1);
    }
}