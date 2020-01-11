using System;
using System.Collections.Generic;
using DynamicData;

namespace WordProcessor.Util.Extension
{
  public static class RandomExt
  {
    public static void Shuffle<T>(this Random rnd, IList<T> arr)
    {
      var n = arr.Count;
      while (n > 1)
      {
        var k = rnd.Next(n);

        if(k == n - 1)
          continue;
        
        n--;

        var value = arr[k];
        arr[k] = arr[n];
        arr[n] = value;
      }
    }

    public static void Shuffle<T>(this Random rnd, IList<T> arr, IList<T> ignoreValues, IEqualityComparer<T> comparer = default)
    {
      if(comparer == null)
        comparer = EqualityComparer<T>.Default;

      var n = arr.Count;
      while (n > 1)
      {
        var k = rnd.Next(n);
        if(k == n - 1)
          continue;
        
        n -= 1;
        var value = arr[k];
        
        if(ignoreValues.IndexOf(value, comparer) >= 0)
          continue;

        arr[k] = arr[n];
        arr[n] = value;
      }
    }
  }
}