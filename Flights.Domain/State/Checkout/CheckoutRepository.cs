using Flights.Domain.Entities.Game;

namespace Flights.Domain.State.Checkout;
public class CheckoutRepository
{
    private readonly Dictionary<int, List<DartsState>> _checkouts;

    public CheckoutRepository(){
        _checkouts = CreateCheckouts();
    }

    public List<DartsState> GetCheckout(int score, int remDarts){
        if(!_checkouts.TryGetValue(score, out var checkouts))
            return new();

        var matches = checkouts.Where(x => remDarts >= x.GetDartsList().Count)
            .ToList();

        return matches;
    }

    private Dictionary<int, List<DartsState>> CreateCheckouts(){
        return new Dictionary<int, List<DartsState>>
        {
            {170, [DartsState.Create(DartModifier.Triple, 20, DartModifier.Triple, 20, DartModifier.Double, 25)]},
            {167, [DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 19,  DartModifier.Double, 25)]},
            {164, [
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 18,  DartModifier.Double, 25),
                DartsState.Create(DartModifier.Triple, 19,  DartModifier.Triple, 19,  DartModifier.Double, 25)
                ]},
            {161, [DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 17,  DartModifier.Double, 25)]},
            {160, [DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 20,  DartModifier.Double, 20)]},
            {158, [DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 20,  DartModifier.Double, 19)]},
            {157, [DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 19,  DartModifier.Double, 20)]},
            {156, [DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 20,  DartModifier.Double, 18)]},
            {155, [DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 19,  DartModifier.Double, 19)]},
            {154, [DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 18,  DartModifier.Double, 20)]},
            {153, [DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 19,  DartModifier.Double, 18)]},
            {152, [DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 20,  DartModifier.Double, 16)]},
            {151, [
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 17,  DartModifier.Double, 20),
                DartsState.Create(DartModifier.Triple, 19,  DartModifier.Triple, 18,  DartModifier.Double, 20)
                ]},
            {150, [
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 18,  DartModifier.Double, 18),
                DartsState.Create(DartModifier.Triple, 19,  DartModifier.Triple, 19,  DartModifier.Double, 18)
                ]},
            {149, [DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 19,  DartModifier.Double, 16)]},
            {148, [
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 16,  DartModifier.Double, 20),
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 20,  DartModifier.Double, 14),
                DartsState.Create(DartModifier.Triple, 19,  DartModifier.Triple, 17,  DartModifier.Double, 20),
                ]},
            {147, [
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 17,  DartModifier.Double, 18),
                DartsState.Create(DartModifier.Triple, 19,  DartModifier.Triple, 18,  DartModifier.Double, 18)
                ]},
            {146, [
                DartsState.Create(DartModifier.Triple, 19,  DartModifier.Triple, 19,  DartModifier.Double, 16),
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 18,  DartModifier.Double, 16)
                ]},
            {145, [DartsState.Create(DartModifier.Triple, 19,  DartModifier.Triple, 20,  DartModifier.Double, 14)]},
            {144, [DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 20,  DartModifier.Double, 12)]},
            {143, [
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 17,  DartModifier.Double, 16),
                DartsState.Create(DartModifier.Triple, 19,  DartModifier.Triple, 18,  DartModifier.Double, 16)
                ]},
            {142, [
                DartsState.Create(DartModifier.Triple, 18,  DartModifier.Triple, 20,  DartModifier.Double, 14),
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 14,  DartModifier.Double, 20),
                DartsState.Create(DartModifier.Triple, 19,  DartModifier.Triple, 19,  DartModifier.Double, 14)
                ]},
            {141, [DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 19,  DartModifier.Double, 12)]},
            {140, [DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 20,  DartModifier.Double, 10)]},
            {139, [
                DartsState.Create(DartModifier.Triple, 19,  DartModifier.Triple, 14,  DartModifier.Double, 20),
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 13,  DartModifier.Double, 20),
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 19,  DartModifier.Double, 11)
                ]},
            {138, [
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 18,  DartModifier.Double, 12),
                DartsState.Create(DartModifier.Triple, 19,  DartModifier.Triple, 19,  DartModifier.Double, 12)
                ]},
            {137, [DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 15,  DartModifier.Double, 16)]},
            {136, [DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 20,  DartModifier.Double, 8)]},
            {135, [
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 17,  DartModifier.Double, 12),
                DartsState.Create(DartModifier.None, 25,  DartModifier.Triple, 20,  DartModifier.Double, 25)
                ]},
            {134, [DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 14,  DartModifier.Double, 16)]},
            {133, [DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 11,  DartModifier.Double, 20)]},
            {132, [
                DartsState.Create(DartModifier.Double, 25,  DartModifier.Triple, 14,  DartModifier.Double, 20),
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 16,  DartModifier.Double, 12),
                DartsState.Create(DartModifier.None, 25,  DartModifier.Triple, 19,  DartModifier.Double, 25)
                ]},
            {131, [
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 13,  DartModifier.Double, 16),
                DartsState.Create(DartModifier.Triple, 19,  DartModifier.Triple, 14,  DartModifier.Double, 16)
                ]},
            {130, [DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 20,  DartModifier.Double, 5)]},
            {129, [
                DartsState.Create(DartModifier.Triple, 19,  DartModifier.Triple, 16,  DartModifier.Double, 12),
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 19,  DartModifier.Double, 6)
                ]},
            {128, [
                DartsState.Create(DartModifier.Triple, 18,  DartModifier.Triple, 14,  DartModifier.Double, 16),
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 18,  DartModifier.Double, 7)
                ]},
            {127, [DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 17,  DartModifier.Double, 8)]},
            {126, [DartsState.Create(DartModifier.Triple, 19,  DartModifier.Triple, 19,  DartModifier.Double, 6)]},
            {125, [
                DartsState.Create(DartModifier.Double, 25,  DartModifier.Triple, 17,  DartModifier.Double, 12),
                DartsState.Create(DartModifier.Triple, 18,  DartModifier.Triple, 19,  DartModifier.Double, 7),
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 15,  DartModifier.Double, 10)
                ]},
            {124, [DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 16,  DartModifier.Double, 8)]},
            {123, [DartsState.Create(DartModifier.Triple, 19,  DartModifier.Triple, 16,  DartModifier.Double, 9)]},
            {122, [DartsState.Create(DartModifier.Triple, 18,  DartModifier.Triple, 18,  DartModifier.Double, 7)]},
            {121, [
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 11,  DartModifier.Double, 14),
                DartsState.Create(DartModifier.Triple, 17,  DartModifier.Triple, 20,  DartModifier.Double, 5)
            ]},
            {120, [DartsState.Create(DartModifier.Triple, 20,  DartModifier.None, 20,  DartModifier.Double, 20)]},
            {119, [DartsState.Create(DartModifier.Triple, 19,  DartModifier.Triple, 12,  DartModifier.Double, 13)]},
            {118, [DartsState.Create(DartModifier.Triple, 20,  DartModifier.None, 18,  DartModifier.Double, 20)]},
            {117, [
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.None, 17,  DartModifier.Double, 20),
                DartsState.Create(DartModifier.Triple, 19,  DartModifier.None, 20,  DartModifier.Double, 20)
            ]},
            {116, [
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.None, 16,  DartModifier.Double, 20),
                DartsState.Create(DartModifier.Triple, 19,  DartModifier.None, 19,  DartModifier.Double, 20)
                ]},
            {115, [
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.None, 15,  DartModifier.Double, 20),
                DartsState.Create(DartModifier.Triple, 19,  DartModifier.None, 18,  DartModifier.Double, 20)]},
            {114, [
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.None, 14,  DartModifier.Double, 20),
                DartsState.Create(DartModifier.Triple, 19,  DartModifier.None, 17,  DartModifier.Double, 20)
                ]},
            {113, [DartsState.Create(DartModifier.Triple, 19,  DartModifier.None, 16,  DartModifier.Double, 20)]},
            {112, [DartsState.Create(DartModifier.Triple, 20,  DartModifier.None, 12,  DartModifier.Double, 20)]},
            {111, [
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.None, 11,  DartModifier.Double, 20),
                DartsState.Create(DartModifier.Triple, 19,  DartModifier.None, 14,  DartModifier.Double, 20)]},
            {110, [
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.Double, 25),
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.Triple, 10,  DartModifier.Double, 10)
                ]},                
            {109, [DartsState.Create(DartModifier.Triple, 20,  DartModifier.None, 9,  DartModifier.Double, 20)]},
            {108, [
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.None, 16,  DartModifier.Double, 16),
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.None, 8,  DartModifier.Double, 20)
                ]},
            {107, [
                DartsState.Create(DartModifier.Triple, 19,  DartModifier.Double, 25),
                DartsState.Create(DartModifier.Triple, 19,  DartModifier.None, 18,  DartModifier.Double, 16),
                DartsState.Create(DartModifier.Triple, 19,  DartModifier.Triple, 10,  DartModifier.Double, 10)                
                ]},
            {106, [DartsState.Create(DartModifier.Triple, 20,  DartModifier.None, 14,  DartModifier.Double, 16)]},
            {105, [DartsState.Create(DartModifier.Triple, 20,  DartModifier.None, 13,  DartModifier.Double, 16)]},
            {104, [
                DartsState.Create(DartModifier.Triple, 18,  DartModifier.Double, 25),
                DartsState.Create(DartModifier.Triple, 18,  DartModifier.None, 18,  DartModifier.Double, 16),
                DartsState.Create(DartModifier.Triple, 19,  DartModifier.None, 15,  DartModifier.Double, 16)                
                ]},
            {103, [
                DartsState.Create(DartModifier.Triple, 19,  DartModifier.None, 14,  DartModifier.Double, 16),
                DartsState.Create(DartModifier.Triple, 19,  DartModifier.None, 10,  DartModifier.Double, 18),
                DartsState.Create(DartModifier.Triple, 19,  DartModifier.None, 6,  DartModifier.Double, 20)
                ]},
            {102, [
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.None, 10,  DartModifier.Double, 16),
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.None, 6,  DartModifier.Double, 18),
                ]},
            {101, [
                DartsState.Create(DartModifier.Triple, 17,  DartModifier.Double, 25),
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.None, 9,  DartModifier.Double, 16),                
                ]},                
            {100, [DartsState.Create(DartModifier.Triple, 20,  DartModifier.Double, 20)]},
            {99, [
                DartsState.Create(DartModifier.Triple, 19,  DartModifier.None, 10,  DartModifier.Double, 16),
                DartsState.Create(DartModifier.Triple, 19,  DartModifier.None, 6,  DartModifier.Double, 18)
                ]},
            {98, [DartsState.Create(DartModifier.Triple, 20,  DartModifier.Double, 19)]},
            {97, [DartsState.Create(DartModifier.Triple, 19,  DartModifier.Double, 20)]},
            {96, [DartsState.Create(DartModifier.Triple, 20,  DartModifier.Double, 18)]},
            {95, [
                DartsState.Create(DartModifier.Triple, 19,  DartModifier.Double, 19),
                DartsState.Create(DartModifier.None, 25,  DartModifier.Triple, 20,  DartModifier.Double, 5)
                ]},
            {94, [
                DartsState.Create(DartModifier.Triple, 18,  DartModifier.Double, 20),
                DartsState.Create(DartModifier.None, 25,  DartModifier.Triple, 19,  DartModifier.Double, 6)]},
            {93, [
                DartsState.Create(DartModifier.Triple, 19,  DartModifier.Double, 18),
                DartsState.Create(DartModifier.None, 25,  DartModifier.Triple, 18,  DartModifier.Double, 7)
                ]},
            {92, [
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.Double, 16),
                DartsState.Create(DartModifier.None, 25,  DartModifier.Triple, 17,  DartModifier.Double, 8)
                ]},
            {91, [
                DartsState.Create(DartModifier.Triple, 17,  DartModifier.Double, 20),
                DartsState.Create(DartModifier.None, 25,  DartModifier.Triple, 16,  DartModifier.Double, 9)
                ]},
            {90, [
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.Double, 15),
                DartsState.Create(DartModifier.Triple, 18,  DartModifier.Double, 18)
                ]},
            {89, [DartsState.Create(DartModifier.Triple, 19,  DartModifier.Double, 16)]},
            {88, [DartsState.Create(DartModifier.Triple, 20,  DartModifier.Double, 14)]},
            {87, [DartsState.Create(DartModifier.Triple, 17,  DartModifier.Double, 18)]},
            {86, [DartsState.Create(DartModifier.Triple, 18,  DartModifier.Double, 16)]},
            {85, [
                DartsState.Create(DartModifier.Triple, 15,  DartModifier.Double, 20),
                DartsState.Create(DartModifier.Triple, 19,  DartModifier.Double, 14)
                ]},
            {84, [DartsState.Create(DartModifier.Triple, 20,  DartModifier.Double, 12)]},
            {83, [DartsState.Create(DartModifier.Triple, 17,  DartModifier.Double, 16)]},
            {82, [
                DartsState.Create(DartModifier.Double, 25,  DartModifier.Double, 16),
                DartsState.Create(DartModifier.Triple, 14,  DartModifier.Double, 20),
                DartsState.Create(DartModifier.None, 25,  DartModifier.None, 17,  DartModifier.Double, 20)
                ]},
            {81, [
                DartsState.Create(DartModifier.Triple, 19,  DartModifier.Double, 12),
                DartsState.Create(DartModifier.Triple, 15,  DartModifier.Double, 18)
                ]},
            {80, [
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.Double, 10),
                DartsState.Create(DartModifier.Double, 20,  DartModifier.Double, 20)
                ]},
            {79, [
                DartsState.Create(DartModifier.Triple, 19,  DartModifier.Double, 11),
                DartsState.Create(DartModifier.Triple, 13,  DartModifier.Double, 20)
                ]},
            {78, [DartsState.Create(DartModifier.Triple, 18,  DartModifier.Double, 12)]},
            {77, [DartsState.Create(DartModifier.Triple, 19,  DartModifier.Double, 10)]},
            {76, [
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.Double, 8),
                DartsState.Create(DartModifier.Triple, 16,  DartModifier.Double, 14)
                ]},
            {75, [DartsState.Create(DartModifier.Triple, 17,  DartModifier.Double, 12)]},
            {74, [DartsState.Create(DartModifier.Triple, 14,  DartModifier.Double, 16)]},
            {73, [DartsState.Create(DartModifier.Triple, 17,  DartModifier.Double, 11)]},
            {72, [
                DartsState.Create(DartModifier.Triple, 16,  DartModifier.Double, 12),
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.Double, 6)
                ]},
            {71, [DartsState.Create(DartModifier.Triple, 13,  DartModifier.Double, 16)]},
            {70, [
                DartsState.Create(DartModifier.Triple, 18,  DartModifier.Double, 8),
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.Double, 5)
                ]},
            {69, [
                DartsState.Create(DartModifier.Triple, 15,  DartModifier.Double, 12),
                DartsState.Create(DartModifier.Triple, 19,  DartModifier.Double, 6)
                ]},
            {68, [
                DartsState.Create(DartModifier.Triple, 16,  DartModifier.Double, 10),
                DartsState.Create(DartModifier.Triple, 20,  DartModifier.Double, 4),
                DartsState.Create(DartModifier.Triple, 18,  DartModifier.Double, 7)
                ]},
            {67, [
                DartsState.Create(DartModifier.Triple, 17,  DartModifier.Double, 8),
                DartsState.Create(DartModifier.Triple, 9,  DartModifier.Double, 20)
                ]},
            {66, [
                DartsState.Create(DartModifier.Triple, 10,  DartModifier.Double, 18),
                DartsState.Create(DartModifier.Triple, 16,  DartModifier.Double, 9),
                DartsState.Create(DartModifier.Triple, 18,  DartModifier.Double, 6)
                ]},
            {65, [
                DartsState.Create(DartModifier.Triple, 11,  DartModifier.Double, 16),
                DartsState.Create(DartModifier.Triple, 19,  DartModifier.Double, 4),
                DartsState.Create(DartModifier.Triple, 15,  DartModifier.Double, 10)
                ]},
            {64, [
                DartsState.Create(DartModifier.Triple, 16,  DartModifier.Double, 8),
                DartsState.Create(DartModifier.Triple, 14,  DartModifier.Double, 11)
                ]},
            {63, [
                DartsState.Create(DartModifier.Triple, 13,  DartModifier.Double, 12),
                DartsState.Create(DartModifier.Triple, 17,  DartModifier.Double, 6)
                ]},
            {62, [
                DartsState.Create(DartModifier.Triple, 10,  DartModifier.Double, 16),
                DartsState.Create(DartModifier.Triple, 12,  DartModifier.Double, 13)
                ]},
            {61, [
                DartsState.Create(DartModifier.Triple, 15,  DartModifier.Double, 8),
                DartsState.Create(DartModifier.Triple, 7,  DartModifier.Double, 20),
                DartsState.Create(DartModifier.Triple, 11,  DartModifier.Double, 14)
                ]},
            {60, [DartsState.Create(DartModifier.None, 20,  DartModifier.Double, 20)]},
            {59, [DartsState.Create(DartModifier.None, 19,  DartModifier.Double, 20)]},
            {58, [DartsState.Create(DartModifier.None, 18,  DartModifier.Double, 20)]},
            {57, [DartsState.Create(DartModifier.None, 17,  DartModifier.Double, 20)]},
            {56, [DartsState.Create(DartModifier.None, 16,  DartModifier.Double, 20)]},
            {55, [DartsState.Create(DartModifier.None, 15,  DartModifier.Double, 20)]},
            {54, [DartsState.Create(DartModifier.None, 14,  DartModifier.Double, 20)]},
            {53, [DartsState.Create(DartModifier.None, 13,  DartModifier.Double, 20)]},
            {52, [DartsState.Create(DartModifier.None, 12,  DartModifier.Double, 20)]},
            {51, [
                DartsState.Create(DartModifier.None, 19,  DartModifier.Double, 16),
                DartsState.Create(DartModifier.None, 11,  DartModifier.Double, 20)
                ]},
            {50, [
                DartsState.Create(DartModifier.Double, 25),
                DartsState.Create(DartModifier.None, 18,  DartModifier.Double, 16),
                DartsState.Create(DartModifier.None, 10,  DartModifier.Double, 20)
                ]},
            {49, [
                DartsState.Create(DartModifier.None, 9,  DartModifier.Double, 20),
                DartsState.Create(DartModifier.None, 17,  DartModifier.Double, 16)
                ]},
            {48, [
                DartsState.Create(DartModifier.None, 16,  DartModifier.Double, 16),
                DartsState.Create(DartModifier.None, 8,  DartModifier.Double, 20)
                ]},
            {47, [
                DartsState.Create(DartModifier.None, 15,  DartModifier.Double, 16),
                DartsState.Create(DartModifier.None, 7,  DartModifier.Double, 20)
                ]},
            {46, [
                DartsState.Create(DartModifier.None, 14,  DartModifier.Double, 16),
                DartsState.Create(DartModifier.None, 6,  DartModifier.Double, 20),
                DartsState.Create(DartModifier.None, 10,  DartModifier.Double, 18)
                ]},
            {45, [
                DartsState.Create(DartModifier.None, 13,  DartModifier.Double, 16),
                DartsState.Create(DartModifier.None, 19,  DartModifier.Double, 13)
                ]},
            {44, [
                DartsState.Create(DartModifier.None, 12,  DartModifier.Double, 16),
                DartsState.Create(DartModifier.None, 4,  DartModifier.Double, 20)
                ]},
            {43, [
                DartsState.Create(DartModifier.None, 11,  DartModifier.Double, 16),
                DartsState.Create(DartModifier.None, 3,  DartModifier.Double, 20)
                ]},
            {42, [
                DartsState.Create(DartModifier.None, 10,  DartModifier.Double, 16),
                DartsState.Create(DartModifier.None, 6,  DartModifier.Double, 18)
                ]},
            {41, [DartsState.Create(DartModifier.None, 9,  DartModifier.Double, 16)]},
            {40, [DartsState.Create(DartModifier.Double, 20)]},
            {39, [DartsState.Create(DartModifier.None, 7,  DartModifier.Double, 16)]},
            {38, [DartsState.Create(DartModifier.Double, 19)]},
            {37, [DartsState.Create(DartModifier.None, 5,  DartModifier.Double, 16)]},
            {36, [DartsState.Create(DartModifier.Double, 18)]},
            {35, [DartsState.Create(DartModifier.None, 3,  DartModifier.Double, 16)]},
            {34, [DartsState.Create(DartModifier.Double, 17)]},
            {33, [DartsState.Create(DartModifier.None, 1,  DartModifier.Double, 16)]},
            {32, [DartsState.Create(DartModifier.Double, 16)]},
            {31, [DartsState.Create(DartModifier.None, 15,  DartModifier.Double, 8)]},
            {30, [DartsState.Create(DartModifier.Double, 15)]},
            {29, [DartsState.Create(DartModifier.None, 13,  DartModifier.Double, 8)]},
            {28, [DartsState.Create(DartModifier.Double, 14)]},
            {27, [DartsState.Create(DartModifier.None, 11,  DartModifier.Double, 8)]},
            {26, [DartsState.Create(DartModifier.Double, 13)]},
            {25, [DartsState.Create(DartModifier.None, 9,  DartModifier.Double, 8)]},
            {24, [DartsState.Create(DartModifier.Double, 12)]},
            {23, [DartsState.Create(DartModifier.None, 7,  DartModifier.Double, 8)]},
            {22, [DartsState.Create(DartModifier.Double, 11)]},
            {21, [DartsState.Create(DartModifier.None, 5,  DartModifier.Double, 8)]},
            {20, [DartsState.Create(DartModifier.Double, 10)]},
            {19, [DartsState.Create(DartModifier.None, 3,  DartModifier.Double, 8)]},
            {18, [DartsState.Create(DartModifier.Double, 9)]},
            {17, [DartsState.Create(DartModifier.None, 1,  DartModifier.Double, 8)]},
            {16, [DartsState.Create(DartModifier.Double, 8)]},
            {15, [DartsState.Create(DartModifier.None, 7,  DartModifier.Double, 4)]},
            {14, [DartsState.Create(DartModifier.Double, 7)]},
            {13, [DartsState.Create(DartModifier.None, 5,  DartModifier.Double, 4)]},
            {12, [DartsState.Create(DartModifier.Double, 6)]},
            {11, [DartsState.Create(DartModifier.None, 3,  DartModifier.Double, 4)]},
            {10, [DartsState.Create(DartModifier.Double, 5)]},
            {9, [DartsState.Create(DartModifier.None, 1,  DartModifier.Double, 4)]},
            {8, [DartsState.Create(DartModifier.Double, 4)]},
            {7, [DartsState.Create(DartModifier.None, 3,  DartModifier.Double, 2)]},
            {6, [DartsState.Create(DartModifier.Double, 3)]},
            {5, [DartsState.Create(DartModifier.None, 1,  DartModifier.Double, 2)]},
            {4, [DartsState.Create(DartModifier.Double, 2)]},
            {3, [DartsState.Create(DartModifier.None, 1,  DartModifier.Double, 1)]},
            {2, [DartsState.Create(DartModifier.Double, 1)]}
        };
    }
}