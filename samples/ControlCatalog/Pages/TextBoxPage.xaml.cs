using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ReactiveUI;

namespace ControlCatalog.Pages
{
    public class TextBoxPage : UserControl
    {
        public TextBoxPage()
        {
            DataContext = this;
            Command = ReactiveCommand.Create(() =>
            {
                var r = 0;
            });
                
            this.InitializeComponent();
        }

        public void OnClick(object sender, RoutedEventArgs e)
        {
            var r = 0;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public ICommand Command { get; }
    }
}
