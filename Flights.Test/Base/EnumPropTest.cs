using Flights.Domain.State;

namespace Flights.Test.Base;

class TestClass{
    public CricketValue V15 {get;set;}
    public CricketValue V16 {get;set;}
} 

public class EnumPropTests
{
    [Fact]
    public void EnumReflection(){
        var one = new TestClass();
        var others = new List<TestClass> {new(), new()};

        one.V15++;

        Assert.True(one.V15 == CricketValue.Single);

        var propInfo = typeof(TestClass).GetProperties().First(x => x.Name == "V15");

        foreach(var other in others)
            propInfo.SetValue(other, CricketValue.Closed);

        foreach(var other in others)
            Assert.True(other.V15 == CricketValue.Closed);
    }
}