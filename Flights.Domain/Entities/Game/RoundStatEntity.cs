using Flights.Util;

namespace Flights.Domain.Entities.Game;
public class RoundStatEntity : BaseEntity
{
    public Guid RoundId { get; set; }

    public GameRoundEntity Round { get; set; } = null!;

    public Guid PlayerId { get; set; }

    public PlayerEntity Player { get; set; } = null!;

    public int OrderNumber { get; set; }

    public bool IsIn {get;set;}

    public bool IsBust {get;set;}
    public int? Rank {get;set;}

    public int StartPoints {get;set;}
    public int EndPoints {get;set;}

    public DartStatEntity? FirstDart {get; set;}
    public DartStatEntity? SecondDart {get; set;}
    public DartStatEntity? ThirdDart {get; set;}

    public bool AnyDartThrown(){
        return FirstDart != null || SecondDart != null || ThirdDart != null;
    }

    public List<DartStatEntity> GetDartsList(){
        var result = new List<DartStatEntity>();

        if(FirstDart != null)
            result.Add(FirstDart!);

        if(SecondDart != null)
            result.Add(SecondDart!);

        if(ThirdDart != null)
            result.Add(ThirdDart!);

        return result;
    }

    private static readonly List<int> Washmachine = [1, 5, 20];

    public bool IsWashmachine()
    {
        var darts = GetDartsList();

        if (darts.Count != 3)
            return false;
        
        if (darts.Any(x => x.Modifier != DartModifier.None))
            return false;

        var points = darts.Select(x => x.Value).ToList();

        return points.Matches(Washmachine);
    }
}
