using Flights.Domain.State.Checkout;

namespace Flights.Test.Base;

public class CheckoutsTest
{
    [Fact]
    public void TestCheckoutsSum()
    {
        var checkouts = new CheckoutRepository();

        for (int i = 2; i <= 170; i++)
        {
            var chk = checkouts.GetCheckout(i, 3);

            foreach (var c in chk)
            {
                var darts = c.GetDartsList();
                var value = darts.Sum(x => x.Calculated);
                
                Assert.True(value == i);
            }
        }
    }
}