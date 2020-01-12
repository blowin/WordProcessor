using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WordProcessor.Util
{
  public static class ReflectionUtil
  {
    public static List<T> GetPublicStaticFields<T>(Type typeHolder) 
      where T : class
    {
      return typeHolder
        .GetFields(BindingFlags.Public | BindingFlags.Static)
        .Select(s => s.GetValue(null) as T)
        .Where(s => s != null)
        .ToList();
    }
  }
}