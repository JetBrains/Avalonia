using System.Collections.ObjectModel;
using System.Windows.Input;
using AvaloniaAppTemplate.Views;
using ReactiveUI;

namespace AvaloniaAppTemplate.ViewModels
{
  public class MainWindowViewModel : ViewModelBase
  {
    public MainWindowViewModel()
    {
      Command = ReactiveCommand.Create(() =>
      {
        var window = new DialogWindow
        {
          Width = 300,
          Height = 150
        };
        window.DataContext = new DialogWindowViewModel();
        
        window.Show();
      });
    }

    public string Greeting => "Welcome to Avalonia!";
    public ICommand Command { get; }
    public static ObservableCollection<string> TraceLog { get; } = new();
  }
}