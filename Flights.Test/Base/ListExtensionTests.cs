using Flights.Util;

namespace Flights.Test.Base;

public class ListExtensionTests
{
    [Fact]
    public void GroupTo_Test()
    {
        var list = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        var groupsOf = list.ToGroupsOf(4);
        
        Assert.True(groupsOf.Count == 3);
        Assert.True(groupsOf[0].Count == 4);
        Assert.True(groupsOf[1].Count == 4);
        Assert.True(groupsOf[2].Count == 2);
        
        //
        groupsOf = list.ToGroupsOf(5);
        
        Assert.True(groupsOf.Count == 2);
        Assert.True(groupsOf[0].Count == 5);
        Assert.True(groupsOf[1].Count == 5);
        
        //
        groupsOf = list.ToGroupsOf(7);
        
        Assert.True(groupsOf.Count == 2);
        Assert.True(groupsOf[0].Count == 7);
        Assert.True(groupsOf[1].Count == 3);
    }
}