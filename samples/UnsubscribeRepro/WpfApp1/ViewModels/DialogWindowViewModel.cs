using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ReactiveUI;

namespace AvaloniaAppTemplate.ViewModels
{
  public class DialogWindowViewModel : ViewModelBase
  {
    private string _greeting;

    public DialogWindowViewModel()
    {
      Items = Enumerable.Range(0, 10000).Select(i => $"Item {i}").ToList();
      Greeting = "Welcome to Avalonia!";
    }

    public string Greeting
    {
      get => _greeting;
      set
      {
        _greeting = value;
        OnPropertyChanged();
      }
    }

    public List<string> Items { get; }
  }
}