using Flights.Domain.Entities.Game;
using Flights.Domain.State.Checkout;

namespace Flights.Domain.State.Solvers.X01;
public class X01Solver(GameEntity entity) : IGameSolver
{
    private readonly CheckoutRepository _checkoutRepo = new();

    public GameState Solve(){
        var isGameStart = IsGameStart();

        if(isGameStart)
            InitFirstRound();
        else
            InitLastFromPreviousRound();

        CalculateLastRound();
        
        var finished = entity.Rounds.Last().RoundStats.All(x => x.Rank != null);
        
        entity.Finished = finished
            ? DateTimeOffset.UtcNow
            : null;
            
        var nextPlayerId = GetNextPlayerId();
        var playerStates = GetPlayerState(nextPlayerId ?? Guid.Empty);
        var rounds = entity.Rounds.Count;

        if(!finished && nextPlayerId == null){
            nextPlayerId = entity.Rounds.Last().RoundStats.FirstOrDefault(x => x.Rank == null)?.Player.Id;
            rounds++;
            playerStates = playerStates.Select(x => {
                if(x.PlayerId == nextPlayerId)
                    return x with { Darts = new DartsState(null, null, null),
                                    IsBust = false,
                                    Checkouts = entity.OutModifier == InOutModifier.Double 
                                                        ? _checkoutRepo.GetCheckout(x.Points, 3)
                                                        : null};
                                        
                return x;
                })
                .ToList();
        }
            
        return new GameState(
            entity.Id,
            entity.TournamentGame?.TournamentRound.TournamentId,
            entity.Type,
            entity.FinishAfterFirstRank,
            entity.InModifier,
            entity.OutModifier,
            entity.Started,
            rounds,
            finished,
            nextPlayerId,
            playerStates);
    }

    private bool IsGameStart(){
        return entity.Rounds.Count == 1 ;
    }

    private void InitFirstRound(){
        entity.Rounds
            .First()
            .RoundStats.ForEach(x => {
                x.StartPoints = entity.X01Target;
                x.IsIn = false;});
    }

    private void InitLastFromPreviousRound(){
        var last = entity.Rounds.Last();
        var lastIndex = entity.Rounds.IndexOf(last);
        var previous = entity.Rounds[lastIndex - 1];

        for(var i = 0; i < previous.RoundStats.Count; i++){
            var currentStat = last.RoundStats[i];
            var lastStat = previous.RoundStats[i];
            currentStat.IsIn = lastStat.IsIn;
            currentStat.Rank = lastStat.Rank;

            currentStat.EndPoints = currentStat.StartPoints;
        }
    }

    private void CalculateLastRound(){
        var lastRound = entity.Rounds.Last();

        foreach(var stat in lastRound.RoundStats)
            CalculatePlayerPoints(stat);
    }

    private void CalculatePlayerPoints(RoundStatEntity stats){
        if(stats.Rank != null)
            return;

        var darts = stats.GetDartsList();
        stats.EndPoints = stats.StartPoints;
        var points = stats.StartPoints;

        foreach(var dart in darts){
            
            if(!stats.IsIn){
                var qualifies = DartQualifiesForInOut(entity.InModifier, dart);
                if(qualifies)
                    stats.IsIn = true;
                else
                    continue;
            }
            
            var newPoints = points - dart.GetCalculatedValue();

            if(newPoints == 0)
                if(DartQualifiesForInOut(entity.OutModifier, dart)){
                    points = newPoints;
                    SetRank(stats);
                    break;
                }else{
                    stats.IsBust = true;
                    return;
                }

            if(newPoints < GetBustBorder(entity.OutModifier)){
                stats.IsBust = true;
                return;
            }

            points = newPoints;
        }

        stats.EndPoints = points;
    }

    private bool DartQualifiesForInOut(InOutModifier modifier, DartStatEntity dart){
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
        }
    }

    private int GetBustBorder(InOutModifier modifier){
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
        }
    }

    private void SetRank(RoundStatEntity player){
        var round = entity.Rounds.Last();
        var rank = 1;
        
        if(round.RoundStats.Count > 1)
        {
            rank = round.RoundStats            
                .Where(x => x.Player.Id != player.Player.Id)
                .Max(x => x.Rank ?? 0) + 1;
        }
        
        player.Rank = rank;

        var unranked = round.RoundStats.Where(x => x.Rank == null).ToList();
        if(unranked.Count==1){
            unranked.Single().Rank = entity.Players.Count;
            return;
        }

        //quick finish
        if (entity.FinishAfterFirstRank && unranked.Count != 0)
        {
            var pointGroups = unranked.OrderBy(x => x.EndPoints).GroupBy(x => x.EndPoints);

            foreach (var group in pointGroups)
            {
                rank++;
                foreach (var groupPlayer in group)
                    groupPlayer.Rank = rank;
            }
        }
    }

    private List<PlayerState> GetPlayerState(Guid currentPlayerId){
        var stateResult = new List<PlayerState>();
        var lastRound = entity.Rounds.Last();
        var roundBefore = entity.Rounds.Count > 1
            ? entity.Rounds[entity.Rounds.Count - 2]
            : null;
        
        foreach(var playerStats in lastRound.RoundStats){
            var refStat = playerStats;

            if(playerStats.GetDartsList().Count == 0 
                && !playerStats.IsBust 
                && playerStats.Rank == null
                && playerStats.Player.Id != currentPlayerId
                && roundBefore != null) //player has not thrown this round - refer to round before
            {
                var statIndex = lastRound.RoundStats.IndexOf(playerStats);
                refStat = roundBefore.RoundStats[statIndex];
            }
         
            var first = refStat.FirstDart != null
                ? DartState.FromEntity(refStat.FirstDart)
                : null;
            var second = refStat.SecondDart != null
                ? DartState.FromEntity(refStat.SecondDart)
                : null;
            var third = refStat.ThirdDart != null
                ? DartState.FromEntity(refStat.ThirdDart)
                : null;

            var dartState = new DartsState(first, second, third);

            var remainingDarts = dartState.RemainingDarts();
            remainingDarts = remainingDarts == 0 ? 3 : remainingDarts;
            List<DartsState> checkout = [];

            if(entity.OutModifier == InOutModifier.Double)
                checkout = _checkoutRepo.GetCheckout(refStat.EndPoints, remainingDarts);            
            
            stateResult.Add(new PlayerState(
                refStat.Player.Id,
                refStat.Player.Name,
                refStat.IsIn,
                refStat.IsBust,
                refStat.Rank,
                refStat.EndPoints,
                CountPlayerAverage(playerStats.Player.Id),
                GetMaxPointsInRounds(playerStats.Player.Id),
                dartState,
                checkout));
        }

        return stateResult;
    }

    private Guid? GetNextPlayerId(){
        var lastRound = entity.Rounds.Last();
        var players = lastRound.RoundStats;

        if(players.All(x => x.Rank != null))
            return null;

        var remainingPlayers = players
            .Where(x => (x.FirstDart == null || x.SecondDart == null || x.ThirdDart == null) &&
                x.IsBust == false &&
                x.Rank == null).ToList();

        var nextPlayer = remainingPlayers.FirstOrDefault(x => 
            x.ThirdDart == null || x.SecondDart == null || x.FirstDart == null);

        return nextPlayer?.Player.Id;
    }

    private decimal CountPlayerAverage(Guid playerId){
        var allRounds = entity.Rounds.SelectMany(x => x.RoundStats)
            .Where(x => x.Player.Id == playerId && x.GetDartsList().Count != 0)
            .ToList();

        var allPoints = allRounds.Select(x => x.StartPoints - x.EndPoints).Sum();

        if(allRounds.Count == 0)
            return 0;

        var avg = allPoints / (allRounds.Count * 1.0m);
        return Math.Round(avg, 1);
    }

    private int GetMaxPointsInRounds(Guid playerId)
    {
        var allRounds = entity.Rounds.SelectMany(x => x.RoundStats)
            .Where(x => x.Player.Id == playerId && x.GetDartsList().Count != 0 && x.IsBust == false)
            .ToList();
        
        if(allRounds.Count == 0)
            return 0;

        var max = allRounds.Max(x => x.GetDartsList().Sum(y => y.GetCalculatedValue()));
        return max;
    }
}