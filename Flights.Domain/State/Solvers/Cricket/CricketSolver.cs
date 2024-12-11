using System.Reflection;
using Flights.Domain.Entities.Game;

namespace Flights.Domain.State.Solvers.Cricket;
public class CricketSolver(GameEntity game) : IGameSolver
{
    public GameState Solve()
    {
        var playerStates = GetPlayerStates();
        var gameState = SummarizeStates(playerStates.Select(x => x.CricketState!).ToList());
        var rounds = game.Rounds.Count;
        var finished = playerStates.All(x => x.Rank != null);
        var currentPlayerId = GetNextPlayer(playerStates);

        if(!finished && currentPlayerId ==  null){
            rounds++;
            var remaining = playerStates.Where(x => x.Rank == null).ToList();
            currentPlayerId = remaining.FirstOrDefault()?.PlayerId;
            playerStates = playerStates.Select(x => x with {Darts = null}).ToList();        
        }

        if(finished && game.Finished == null)
            game.Finished = DateTimeOffset.UtcNow;

        return new GameState(
            Id: game.Id,
            Type: game.Type,
            FinishAfterFirstRank: game.FinishAfterFirstRank,
            InModifier: InOutModifier.None,
            OutModifier: InOutModifier.None,
            Started: game.Started,
            Round: rounds,
            Finished: finished,
            CurrentPlayerId: currentPlayerId,
            PlayerStates: playerStates,
            CricketState: gameState
        );
    }
    
    private List<PlayerState> GetPlayerStates(){
        var playerStateDtos = game.Players
            .Select(x => new CricketStateDto
            {
                PlayerId = x.Player.Id,
            })
            .ToList();

        foreach(var round in game.Rounds){
            playerStateDtos.ForEach(x =>
            {
                var roundPlayer = round.RoundStats.First(y => y.Player.Id == x.PlayerId);
                x.Darts = null;
                x.Rank = roundPlayer.Rank;
            });

            foreach(var player in round.RoundStats){
                
                var playerState = playerStateDtos.First(x => x.PlayerId == player.Player.Id);

                var darts = player.GetDartsList();

                if(!darts.Any()){
                    player.Rank = playerState.Rank;
                    player.EndPoints = playerState.Points;
                    continue;
                }                    
               
                var otherPlayers = playerStateDtos.ToList();
                otherPlayers.Remove(playerState);

                ProcessPlayerRound(player, playerState, otherPlayers);
            }

            foreach(var player in round.RoundStats){
                var state = playerStateDtos.First(x => x.PlayerId == player.Player.Id);
                player.Rank = state.Rank;
                player.EndPoints = state.Points;
            }
        }

        var result = new List<PlayerState>();
        foreach(var state in playerStateDtos){
            var playerEntity = game.Players.First(x => x.Player.Id == state.PlayerId);
            var playerState = new PlayerState(
                PlayerId: playerEntity.Player.Id,
                PlayerName: playerEntity.Player.Name,
                IsIn: true,
                IsBust: false,
                Rank: state.Rank,
                Points: state.Points,
                PointAvg: 0m,
                Darts: state.Darts,
                Checkout: null,
                CricketState: state.GetCricketState()
            );
            result.Add(playerState);
        }

        return result;
    }

    private void ProcessPlayerRound(RoundStatEntity entity, CricketStateDto playerState, List<CricketStateDto> otherPlayers){
        DartState? first = null;
        DartState? second = null;
        DartState? third = null;

        if(entity.FirstDart != null)
            first = DartState.FromEntity(entity.FirstDart);

        if(entity.SecondDart != null)
            second = DartState.FromEntity(entity.SecondDart);

        if(entity.ThirdDart != null)
            third = DartState.FromEntity(entity.ThirdDart);

        playerState.Darts = new DartsState(first, second, third);

        var darts = entity.GetDartsList();

        var all = otherPlayers.ToList();
        all.Add(playerState);

        foreach(var dart in darts){
            ProcessDart(dart, playerState, otherPlayers);
            CheckRanks(all);
        }
    }


    private void ProcessDart(DartStatEntity dart, CricketStateDto player, List<CricketStateDto> otherPlayers){
        if(!DartIsRelevant(dart))
            return;

        var mod = 1;
        if(dart.Modifier == DartModifier.Double)
            mod = 2;
        else if(dart.Modifier == DartModifier.Triple)
            mod = 3;

        for(var i = mod; i > 0; i--){
            ProcessDartPoints(dart.Value, player, otherPlayers);
        }
    }

    private void ProcessDartPoints(int value, CricketStateDto player, List<CricketStateDto> otherPlayers){
        PropertyInfo? vProp;

        switch(value){
            case 15:
                vProp = typeof(CricketStateDto).GetProperties().First(x => x.Name == "V15");
                ProcessReflectedValue(vProp, 15, player, otherPlayers);
                break;
            case 16:
                vProp = typeof(CricketStateDto).GetProperties().First(x => x.Name == "V16");
                ProcessReflectedValue(vProp, 16, player, otherPlayers);
                break;
            case 17:
                vProp = typeof(CricketStateDto).GetProperties().First(x => x.Name == "V17");
                ProcessReflectedValue(vProp, 17, player, otherPlayers);
                break;
            case 18:
                vProp = typeof(CricketStateDto).GetProperties().First(x => x.Name == "V18");
                ProcessReflectedValue(vProp, 18, player, otherPlayers);
                break;
            case 19:
                vProp = typeof(CricketStateDto).GetProperties().First(x => x.Name == "V19");
                ProcessReflectedValue(vProp, 19, player, otherPlayers);
                break;
            case 20:
                vProp = typeof(CricketStateDto).GetProperties().First(x => x.Name == "V20");
                ProcessReflectedValue(vProp, 20, player, otherPlayers);
                break;
            case 25:
                vProp = typeof(CricketStateDto).GetProperties().First(x => x.Name == "Bulls");
                ProcessReflectedValue(vProp, 25, player, otherPlayers);
                break;
        }
    }

    private void ProcessReflectedValue(PropertyInfo vProp, int value, CricketStateDto player, List<CricketStateDto> otherPlayers){        
        var vValue = (CricketValue)vProp.GetValue(player)!;

        if(vValue == CricketValue.Closed){
            return;
        }

        var justReachedOpen = false;

        if(vValue < CricketValue.Open){
            var newValue = vValue + 1;
            justReachedOpen = newValue == CricketValue.Open;
            vProp.SetValue(player, newValue);
            if(newValue < CricketValue.Open)
                return;
        }

        var allPlayers = otherPlayers.ToList();
        allPlayers.Add(player);
        
        if(allPlayers.All(x => (CricketValue)vProp.GetValue(x)! == CricketValue.Open)){
            allPlayers.ForEach(x => vProp.SetValue(x, CricketValue.Closed));
            return;
        }

        if(justReachedOpen)
            return;

        if(game.Type == GameType.Cricket){
            if(otherPlayers.Any(x => (CricketValue)vProp.GetValue(x)! < CricketValue.Open))
                player.Points += value;
        }
        if(game.Type == GameType.CtCricket){
            otherPlayers.ForEach(x => {
                if((CricketValue)vProp.GetValue(x)! < CricketValue.Open)
                    x.Points += value;
            });
        }
    }

    private bool DartIsRelevant(DartStatEntity dart){
        if((dart.Value >= 15 && dart.Value <= 20) || dart.Value == 25)
            return true;
        
        return false;
    }

    private void CheckRanks(List<CricketStateDto> players)
    {
        var unranked = players.Where(x => x.Rank == null)
            .ToList();

        if (unranked.Count == 0)
            return;

        var unrankedGroups = game.Type == GameType.Cricket
            ? unranked.OrderByDescending(x => x.Points)
                .GroupBy(x => x.Points)
                .ToList()
            : unranked.OrderBy(x => x.Points)
                .GroupBy(x => x.Points)
                .ToList();
        
        var firstGroup = unrankedGroups.First();
        var nextRank = players.Max(x => x.Rank) ?? 0;
        nextRank++;

        //last player
        if (unrankedGroups.Count == 1 
            && firstGroup.Count() == 1
            && players.Count > 1) //<== single player game
        {
            firstGroup.First().Rank = nextRank;
            return;
        }

        //multiple players with same points
        if (firstGroup.Count() > 1)
        {
            var openPlayers = firstGroup.Where(x => x.AllOpen()).ToList();

            if (!openPlayers.Any())
                return;

            //multiple open
            if (openPlayers.Count != 1)
            {
                //when all remaining players are AllOpen
                if (unranked.All(x => x.AllOpen()))
                {
                    foreach (var ply in openPlayers)
                        ply.Rank = nextRank;
                    
                    CheckRanks(players);
                }
                return;
            }

            var openPlayer = openPlayers.Single();
            openPlayer.Rank = nextRank;
            CheckRanks(players);
            return;
        }
            
        //single player
        var player = firstGroup.Single();
        if (player.AllOpen())
        {
            player.Rank = nextRank;
            CheckRanks(players);
        }
        
        //quick finish
        var ranked = players.Where(x => x.Rank != null)
            .ToList();
        unranked = players.Where(x => x.Rank == null)
            .ToList();
        if (game.FinishAfterFirstRank && ranked.Any() && unranked.Any())
        {
            nextRank = ranked.Max(x => x.Rank ?? 0) + 1;
            unrankedGroups = game.Type == GameType.Cricket
                ? unranked.OrderByDescending(x => x.Points)
                    .GroupBy(x => x.Points)
                    .ToList()
                : unranked.OrderBy(x => x.Points)
                    .GroupBy(x => x.Points)
                    .ToList();

            foreach (var group in unrankedGroups)
            {
                foreach(var p in group)
                    p.Rank = nextRank;

                nextRank++;
            }
        }
    }

    private CricketState SummarizeStates(List<CricketState> states){
        var gameState = new CricketState();

        foreach(var playerState in states){
            if(playerState.V15 > gameState.V15)
                gameState = gameState with {V15 = playerState.V15};
            
            if(playerState.V16 > gameState.V16)
                gameState = gameState with {V16 = playerState.V16};

            if(playerState.V17 > gameState.V17)
                gameState = gameState with {V17 = playerState.V17};

            if(playerState.V18 > gameState.V18)
                gameState = gameState with {V18 = playerState.V18};

            if(playerState.V19 > gameState.V19)
                gameState = gameState with {V19 = playerState.V19};

            if(playerState.V20 > gameState.V20)
                gameState = gameState with {V20 = playerState.V20};
            
            if(playerState.Bulls > gameState.Bulls)
                gameState = gameState with {Bulls = playerState.Bulls};
        }

        return gameState;
    }

    private Guid? GetNextPlayer(List<PlayerState> players){
        var remaining = players.Where(x => x.Rank == null).ToList();

        if(!remaining.Any()){
            return null;
        }

        var firstWithMissingDarts = remaining.FirstOrDefault(x => 
            x.Darts?.D3 == null ||
            x.Darts?.D2 == null ||
            x.Darts?.D1 == null)?.PlayerId;

        return firstWithMissingDarts;
    }
}