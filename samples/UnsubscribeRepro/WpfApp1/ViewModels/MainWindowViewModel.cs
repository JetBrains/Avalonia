using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ReactiveUI;
using WpfApp1.Views;

namespace AvaloniaAppTemplate.ViewModels
{
  public class MainWindowViewModel : ViewModelBase
  {
    public MainWindowViewModel()
    {
      Command = new Command(() =>
      {
        var window = new DialogWindow
        {
          Width = 300,
          Height = 150
        };
        var vm = new DialogWindowViewModel();
        window.DataContext = vm;
        
        window.ShowDialog();

        vm.Greeting = "test";
      });
    }

    public string Greeting => "Welcome to Avalonia!";
    public ICommand Command { get; }
    public static ObservableCollection<string> TraceLog { get; } = new();
  }

  public class Command : ICommand
  {
    private readonly Action _action;

    public Command(Action action)
    {
      _action = action;
    }

    public bool CanExecute(object parameter)
    {
      return true;
    }

    public void Execute(object parameter)
    {
      _action();
    }

    public event EventHandler CanExecuteChanged;
  }
}