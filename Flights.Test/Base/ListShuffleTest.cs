using Flights.Util;

namespace Flights.Test.Base;

public class ListShuffleTest
{
    [Fact]
    public void TestListShuffle()
    {
        var list = new List<string>();
        list.Add("A");
        list.Add("B");
        list.Add("C");
        list.Add("D");
        list.Add("E");
        list.Add("F");
        list.Add("G");
        
        list.Shuffle();
        
        Assert.True(list[0] != "A");
        Assert.True(list[1] != "B");
        Assert.True(list[2] != "C");
        Assert.True(list[3] != "D");
        Assert.True(list[4] != "E");
        Assert.True(list[5] != "F");
        Assert.True(list[6] != "G");

        var newList = list.ToList();

        foreach (var item in newList)
        {
            var index = newList.IndexOf(item);
            Assert.True(item == list[index]);
        }
    }
}