using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArrayExtention
{
    public static T[] Shuffle<T>(this T[] array)
    {
        var length = array.Length;
        var result = new T[length];
        Array.Copy(array, result, length);

        var random = new System.Random();
        int n = length;
        while (1 < n)
        {
            n--;
            int k = random.Next(n + 1);
            var tmp = result[k];
            result[k] = result[n];
            result[n] = tmp;
        }
        return result;
    }
}
