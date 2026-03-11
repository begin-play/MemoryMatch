using System;
using System.Collections.Generic;

public static class ExtenstionMethods
{
    private static Random randomSeed = new Random();
    
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = randomSeed.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
}
