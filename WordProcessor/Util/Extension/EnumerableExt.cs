using System;
using System.Collections.Generic;
using System.Linq;

namespace WordProcessor.Util.Extension
{
  public static class EnumerableExt
  {
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T> self) => self.Where(arg => arg != null);

    public static IEnumerable<string> WhereNotBlank(this IEnumerable<string> self) =>
      self.Where(s => !string.IsNullOrWhiteSpace(s));
    
    public static IEnumerable<TRes> SelectNotNull<TIn, TRes>(this IEnumerable<TIn> self, Func<TIn, TRes> map)
    {
      foreach (var @in in self)
      {
        if (@in != null)
          yield return map(@in);
      }
    }
  }
}