using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Logging;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Avalonia.Win32;
using ControlCatalog.ViewModels;
using JetBrains.Annotations;

namespace ControlCatalog.Pages
{
    public class InteropIssuesPage : UserControl, INotifyPropertyChanged
    {
        public static IntPtr MainWindowHandle;
        private Window myMainWindow;

        public InteropIssuesPage()
        {
            this.InitializeComponent();
        }
        public InteropIssuesPage(IntPtr windowHandle)
        {
            MainWindowHandle = windowHandle;
            this.InitializeComponent();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            
            if (MainWindowHandle != IntPtr.Zero)
                myMainWindow = new Window(new WpfToAvaloniaMainWindowImpl(MainWindowHandle));
            else
                myMainWindow = this.GetVisualRoot() as Window;
            
            DataContext = new BindingPageVm(myMainWindow);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        public async void OnClick(object sender, RoutedEventArgs e)
        {
            (App.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime).MainWindow.Closing+=MainWindowOnClosing;
            
            (App.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Shutdown();
            
   
            // ((BindingPageVm)DataContext).StringValue =
            //     ((BindingPageVm)DataContext).StringValue ?? "" + " click";
        }

        private void MainWindowOnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            
            Dispatcher.UIThread.Post(() => GC.GetTotalMemory(true));
        }
    }
    
    public class BindingPageVm : INotifyPropertyChanged
    {
        private string _stringValue;
        public event PropertyChangedEventHandler PropertyChanged;

        public BindingPageVm(Window myMainWindow)
        {
            ListBox = new ListBoxPageViewModel();
            Command = new DelegateCommand(() => Execute(myMainWindow));
        }

        private async void Execute(Window myMainWindow)
        {
            var dialog = new OpenFileDialog();
            var result = await dialog.ShowAsync(myMainWindow);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string StringValue
        {
            get => _stringValue;
            set
            {
                if (value == _stringValue) return;
                _stringValue = value;
                OnPropertyChanged();
            }
        }

        public ICommand Command { get; }
        public ListBoxPageViewModel ListBox { get; }
    }
    
    internal class WpfToAvaloniaMainWindowImpl : WindowImpl
    {
        private readonly IntPtr _mainWindowHandle;

        public WpfToAvaloniaMainWindowImpl(IntPtr mainWindowHandle)
        {
            _mainWindowHandle = mainWindowHandle;
        }

        protected override IntPtr CreateWindowOverride(ushort atom)
        {
            return InteropIssuesPage.MainWindowHandle;
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
            try
            {
                return myCanExecute == null || myCanExecute(parameter);
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    
        public void Execute(object parameter)
        {
            try
            {
                myExecute(parameter);
            }
            catch(Exception ex)
            {
            }
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
    public class DelegateCommand<T> : Command<T>
    {
        private readonly Action<T> myExecute;
        private readonly Predicate<T> myCanExecute;

        public DelegateCommand([NotNull] Action<T> execute, Predicate<T> canExecute = null)
        {
            if (execute == null) throw new ArgumentNullException("execute");

            myExecute = execute;
            myCanExecute = canExecute;
        }

        public override void Execute(T parameter)
        {
            try
            {
                myExecute(parameter);
            }
            catch(Exception ex)
            {
            }
        }

        public override bool CanExecute(T parameter)
        {
            try
            {
                return myCanExecute == null || myCanExecute(parameter);
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public void RaiseCanExecuteChanged()
        {
            RaiseCanExecuteChangedOnCorrectThread();
        }
    }
    
    public abstract class Command<T> : ICommand
    {
        public abstract void Execute(T parameter);

        public abstract bool CanExecute(T parameter);

        void ICommand.Execute(object parameter)
        {
            Execute((T) parameter);
        }

        public event EventHandler CanExecuteChanged;

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute((T)parameter);
        }
        
        protected void RaiseCanExecuteChangedOnCorrectThread()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
            // myDispatcher.AssertAccess();
            // if(PlatformUtil.IsRunningUnderWindows)
            //     CommandManager.InvalidateRequerySuggested();
        }
    }
    
}
