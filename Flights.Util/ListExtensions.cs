namespace Flights.Util;

public static class ListExtensions
{
    private static readonly Random Random = new ();

    public static void Shuffle<T>(this List<T> list)
    {
        int n = list.Count;
        while (n > 1) {  
            n--;  
            int k = Random.Next(n + 1);  
            (list[k], list[n]) = (list[n], list[k]);
        }  
    }

    public static bool None<T>(this List<T> list){
        return list.Count() == 0;
    }

    public static List<List<T>> ToGroupsOf<T>(this List<T> list, int count)
    {
        if(count < 1)
            throw new ArgumentException("'count' must be greater than 0");

        var result = new List<List<T>>();
        var currentCount = 0;
        var currentList = new List<T>();
        result.Add(currentList);

        foreach (var item in list)
        {
            if (currentCount == count)
            {
                currentList = new List<T>();
                result.Add(currentList);
                currentCount = 0;
            }
            
            currentList.Add(item);
            currentCount++;
        }
        
        return result;
    }

    public static T? RemoveLast<T>(this List<T> list, int numberOfItems){
        T? last = default(T);

        for(var i = 0; i < numberOfItems; i++)
            last = list.RemoveLast();

        return last;
    }

    public static T? RemoveLast<T>(this List<T> list){
        if(list.Count == 0)
            return default(T);

        var lastIndex = list.Count - 1;
        var last = list[lastIndex];
        list.RemoveAt(lastIndex);
        
        return last;
    }

    public static bool Matches<T>(this List<T> list, List<T> otherList)
    {
        if (list.Count != otherList.Count)
            return false;
        
        foreach(var other in otherList)
            if (!list.Contains(other))
                return false;

        return true;
    }
}
