using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ReactiveUI;

namespace AvaloniaAppTemplate.ViewModels
{
  public class DialogWindowViewModel : ViewModelBase
  {
    public DialogWindowViewModel()
    {
      Items = Enumerable.Range(0, 10000).Select(i => $"Item {i}").ToList();
    }

    public string Greeting => "Welcome to Avalonia!";
    public List<string> Items { get; }
  }
}