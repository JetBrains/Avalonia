using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AvaloniaAppTemplate.Views
{
  public class DialogWindow : Window
  {
    public DialogWindow()
    {
      InitializeComponent();
//-:cnd:noEmit
#if DEBUG
      this.AttachDevTools();
#endif
//+:cnd:noEmit
    }

    private void InitializeComponent()
    {
      AvaloniaXamlLoader.Load(this);
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
      Content = null;
      Close();
    }

    private void Button2_OnClick(object? sender, RoutedEventArgs e)
    {
      Close();
    }
  }
}