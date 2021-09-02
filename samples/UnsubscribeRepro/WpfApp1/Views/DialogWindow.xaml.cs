using System.Windows;

namespace WpfApp1.Views
{
  public partial class DialogWindow
  {
    public DialogWindow()
    {
      InitializeComponent();
    }

    private void Button_OnClick(object sender, RoutedEventArgs e)
    {
      Content = null;
      Close();
    }
    
    private void Button2_OnClick(object sender, RoutedEventArgs e)
    {
      Close();
    }
  }
}