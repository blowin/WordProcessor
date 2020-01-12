using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;

namespace WordProcessor.DataTypes
{
  public static class LocalizationManager
  {
    #region Fields

    private static ResourceDictionary LocalizationResourceDictionary;

    private static readonly Dictionary<string, Uri> CacheLanguages = new Dictionary<string, Uri>();

    #endregion

    public static ObservableCollection<CultureInfo> Languages { get; } = CreateAvailableLanguages();

    public static CultureInfo Language
    {
      get => Thread.CurrentThread.CurrentUICulture;
      set
      {
        if(value == null)
          throw new InvalidOperationException("Language can not be null");

        if(Equals(value, Thread.CurrentThread.CurrentUICulture))
          return;

        Thread.CurrentThread.CurrentUICulture = value;
        Thread.CurrentThread.CurrentCulture = value;

        LocalizationResourceDictionary.Source = GetUri(value);

        Settings.Default.DefaultLanguage = value;
        Settings.Default.Save();
      }
    }

    public static void SetupLocalization()
    {
      var localizationDictionary = Application.Current.Resources.MergedDictionaries
        .First(s => s.Source != null && s.Source.OriginalString.StartsWith("Resources/lang"));
      
      LocalizationResourceDictionary = localizationDictionary;
      
      var currentLocalization = Settings.Default.DefaultLanguage;
      LocalizationResourceDictionary.Source = GetUri(currentLocalization);
      Thread.CurrentThread.CurrentUICulture = currentLocalization;
      Thread.CurrentThread.CurrentCulture= currentLocalization;
    }

    public static string GetLocalizationString(string key)
    {
      return (string)LocalizationResourceDictionary[key];
    }

    #region Private methods

    private static Uri GetUri(CultureInfo info)
    {
      if (CacheLanguages.TryGetValue(info.Name, out var res))
        return res;

      res = new Uri($"Resources/lang.{info.Name}.xaml", UriKind.Relative);
      CacheLanguages.Add(info.Name, res);
      return res;
    }

    private static ObservableCollection<CultureInfo> CreateAvailableLanguages()
    {
      return new ObservableCollection<CultureInfo>
      {
        new CultureInfo("en-US"),
        new CultureInfo("ru-RU")
      };
    }

    #endregion
  }
}