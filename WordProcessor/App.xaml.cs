using System.Windows;
using WordProcessor.DataTypes;

namespace WordProcessor
{
  public partial class App : Application
  {
    public App()
    {
      InitializeComponent();

      LocalizationManager.SetupLocalization();
    }
  }
}
