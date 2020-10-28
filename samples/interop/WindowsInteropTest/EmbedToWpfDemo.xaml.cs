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
    public partial class EmbedToWpfDemo : Window
    {
        public EmbedToWpfDemo()
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
            // var btn = (Avalonia.Controls.Button) RightBtn.Content;
            // btn.Click += delegate
            // {
            //     btn.Content += "!";
            // };
        }
    }
}
