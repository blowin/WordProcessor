using System.Collections.Generic;
using System.Reflection;
using FlexEnum.DataTypes.Enums;

namespace WordProcessor.Util
{
  public static class FlexEnumHelper
  {
    public static IReadOnlyList<TEnum> Values<TEnum>() 
      where TEnum : BaseEnum0
    {
      return Cache<TEnum>.Values;
    }

    private static class Cache<TEnum>
      where TEnum : BaseEnum0
    {
      private static TEnum[] _fields;
      private static IReadOnlyList<TEnum> _values;

      public static IReadOnlyList<TEnum> Values => _values;

      static Cache()
      {
        var f = typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static);
        _fields = new TEnum[f.Length];
        for (var index = 0; index < f.Length; index++)
        {
          var fieldInfo = f[index];
          _fields[index] = (TEnum)fieldInfo.GetValue(null);
        }

        _values = _fields;
      }
    }
  }
}