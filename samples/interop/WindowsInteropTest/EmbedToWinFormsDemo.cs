using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Avalonia.Controls;
using Avalonia.VisualTree;
using ControlCatalog;
using ControlCatalog.Pages;

namespace WindowsInteropTest
{
    public partial class EmbedToWinFormsDemo : Form
    {
        public EmbedToWinFormsDemo()
        {
            InitializeComponent();
            avaloniaHost.Content = new InteropIssuesPage(this.Handle);
            // avaloniaHost.Content = new MainView();

            Width = 1200;
            Height = 800;
            
            ((TopLevel)avaloniaHost.Content.GetVisualRoot()).Renderer.Start();
        }
    }
}
