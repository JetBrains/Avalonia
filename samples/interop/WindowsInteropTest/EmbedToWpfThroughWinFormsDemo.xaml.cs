using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;
using Avalonia.Win32.Embedding;
using ControlCatalog;
using ControlCatalog.Pages;
using Window = System.Windows.Window;

namespace WindowsInteropTest
{
    /// <summary>
    /// Interaction logic for EmbedToWpfDemo.xaml
    /// </summary>
    public partial class EmbedToWpfThroughWinFormsDemo : Window
    {
        public EmbedToWpfThroughWinFormsDemo()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            
            var view = new InteropIssuesPage(new WindowInteropHelper(this).Handle);
            view.AttachedToVisualTree += delegate
            {
                ((TopLevel) view.GetVisualRoot()).AttachDevTools(); 
            };
            Host.Content = view;
        }
    }
    
    public class AvaloniaHostPresenter : WindowsFormsHost
    {
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            "Content",
            typeof(object),
            typeof(AvaloniaHostPresenter),
            new System.Windows.PropertyMetadata(default, OnContentChanged));

        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is AvaloniaHostPresenter host) || e.NewValue == null)
                return;

            var avaloniaHost = new WinFormsAvaloniaControlHost();
            var avaloniaContent = new Avalonia.Controls.Presenters.ContentPresenter {Content = e.NewValue};
            host.Child = avaloniaHost;
            avaloniaHost.Content = avaloniaContent;
      
            //bug in WinFormsAvaloniaControlHost. Renderer is not started by default
            ((TopLevel)avaloniaContent.GetVisualRoot()).Renderer.Start();
        }
    }
}
