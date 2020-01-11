using MahApps.Metro.Controls;
using WordProcessor.ViewModel;

namespace WordProcessor
{
  public partial class MainWindow : MetroWindow
  {
    public MainWindow()
    {
      InitializeComponent();

      DataContext = new MainWindowViewModel();
    }
  }
}
