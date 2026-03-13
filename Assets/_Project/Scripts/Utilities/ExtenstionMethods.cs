/*
 * Copyright (c) 2026 Sagar Kumar
 * All Rights Reserved.
 */
using System;
using System.Collections.Generic;

public static class ExtenstionMethods
{
    private static readonly Random RandomSeed = new Random();
    
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = RandomSeed.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }

  
}
