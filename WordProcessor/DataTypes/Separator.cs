using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using WordProcessor.Util;

namespace WordProcessor.DataTypes
{
  public sealed class Separator
  {
    private static readonly Lazy<IReadOnlyList<Separator>> LazyValues 
      = new Lazy<IReadOnlyList<Separator>>(() => ReflectionUtil.GetPublicStaticFields<Separator>(typeof(Separator)));

    public static readonly Separator NewLine = new Separator("m_separator_name_NewLine");
    public static readonly Separator Custom = new Separator("m_separator_name_Custom");

    private string _localizationKey;

    public string Name => LocalizationManager.GetLocalizationString(_localizationKey);

    public static IReadOnlyList<Separator> Values
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => LazyValues.Value;
    }

    private Separator(string localizationKey)
    {
      _localizationKey = localizationKey;
    }
  }
}