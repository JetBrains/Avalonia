using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace AvaloniaAppTemplate.ViewModels
{
  public class ViewModelBase : INotifyPropertyChanged
  {
    event PropertyChangedEventHandler? INotifyPropertyChanged.PropertyChanged
    {
      add
      {
        MainWindowViewModel.TraceLog.Add($"PropertyChanged.Add ({GetType().Name})");
        PropertyChanged += value;
      }
      remove
      {
        MainWindowViewModel.TraceLog.Add($"PropertyChanged.Remove ({GetType().Name})");
        PropertyChanged -= value;
      }
    }

    private event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}