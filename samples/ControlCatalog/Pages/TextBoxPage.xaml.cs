using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Logging;
using Avalonia.Markup.Xaml;
using JetBrains.Annotations;

namespace ControlCatalog.Pages
{
    public class TextBoxPage : UserControl
    {
        public TextBoxPage()
        {
            this.InitializeComponent();
            DataContext = new TextBoxPageVm(this.FindControl<TextBox>("TextBox"));
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
    
    public class TextBoxPageVm : INotifyPropertyChanged
    {
        private string _focusedElement = "";
        public event PropertyChangedEventHandler PropertyChanged;

        public TextBoxPageVm(TextBox textBox)
        {
            textBox.GotFocus += (sender, args) =>
            {
                FocusedElement += "\r\n" + "GotFocus";
            };
            textBox.LostFocus += (sender, args) =>
            {
                FocusedElement += "\r\n" + "LostFocus";
            };
            Command = new DelegateCommand(() =>
            {
                textBox.IsVisible = !textBox.IsVisible;
            });
            Command2 = new DelegateCommand(() =>
            {
                textBox.IsVisible = !textBox.IsVisible;
                FocusManager.Instance.Focus(null);
            });
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand Command { get; set; }
        public ICommand Command2 { get; set; }
        
        public string FocusedElement
        {
            get => _focusedElement;
            set
            {
                if (value == _focusedElement) return;
                _focusedElement = value;
                OnPropertyChanged();
            }
        }
    }
    
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> myExecute;

        private readonly Predicate<object> myCanExecute;

        public DelegateCommand([NotNull] Action<object> execute, Predicate<object> canExecute = null)
        {
            if(execute == null)
                throw new ArgumentNullException("execute");
            myExecute = execute;
            myCanExecute = canExecute;
        }

        public DelegateCommand([NotNull] Action execute, Func<bool> canExecute = null)
        {
            if(execute == null)
                throw new ArgumentNullException("execute");
            myExecute = o => execute();
            myCanExecute = canExecute!=null ? o => canExecute() : (Predicate<object>)null;
        }

        public bool CanExecute(object parameter)
        {
            return myCanExecute == null || myCanExecute(parameter);
        }
    
        public void Execute(object parameter)
        {
            myExecute(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
