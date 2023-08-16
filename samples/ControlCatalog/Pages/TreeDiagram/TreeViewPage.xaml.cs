using System;
using System.Collections;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using ControlCatalog.ViewModels;

namespace ControlCatalog.Pages
{
    public class TreeViewPage : UserControl
    {
        private Ellipse? _pointer1;
        private Ellipse? _pointer2;
        private IciclePanel? _control1;
        private SunburstPanel? _control2;
        private FastPath? _selectedItem1;
        private FastPath? _selectedItem2;

        private bool _updating;

        public TreeViewPageViewModel ViewModel => DataContext as TreeViewPageViewModel;

        public TreeViewPage()
        {
            InitializeComponent();
            DataContext = new TreeViewPageViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            _pointer1 = this.FindControl<Ellipse>("Pointer1");
            _pointer2 = this.FindControl<Ellipse>("Pointer2");
            _control1 = this.FindControl<IciclePanel>("IciclePanel");
            _control2 = this.FindControl<SunburstPanel>("SunburstPanel");
        }

        private void IciclePanel_OnPointerMoved(object sender, PointerEventArgs e)
        {
            var relativePosition = e.GetPosition(_control1);
            Canvas.SetLeft(_pointer1, relativePosition.X - _pointer1.Bounds.Width / 2);
            Canvas.SetTop(_pointer1, relativePosition.Y - _pointer1.Bounds.Height / 2);
            
            if (_updating)
                return;
            
            try
            {
                _updating = true;

                var initPoint = ToCartesian(30 + relativePosition.Y, relativePosition.X, _control2.MaxAngle, _control1.Bounds.Width);
                var transformedPosition = initPoint
                    .Transform(Matrix.CreateTranslation(_control2.Bounds.Width/2, _control2.Bounds.Height/2));
                Dispatcher.UIThread.Post(() =>
                {
                    var selectedItem2 = _control2.GetVisualAt(initPoint, null) as FastPath;
                    if (selectedItem2 != _selectedItem2 && _selectedItem2 != null)
                    {
                        _selectedItem2.RaiseEvent(
                            new PointerEventArgs(PointerExitedEvent, e.Source, e.Pointer, _control2,
                                transformedPosition, e.Timestamp, new PointerPointProperties(), e.KeyModifiers));    
                    }
                    if (selectedItem2 != null)
                    {
                        selectedItem2.RaiseEvent(
                            new PointerEventArgs(PointerEnteredEvent, e.Source, e.Pointer, _control2,
                                transformedPosition, e.Timestamp, new PointerPointProperties(), e.KeyModifiers));    
                    }
                    
                    _selectedItem2 = selectedItem2;
                    
                    Canvas.SetLeft(_pointer2, transformedPosition.X);
                    Canvas.SetTop(_pointer2, transformedPosition.Y);
                    
                    // _control2.RaiseEvent(
                    //     new PointerEventArgs(e.RoutedEvent, e.Source, e.Pointer, _control2,
                    //         transformedPosition, e.Timestamp, new PointerPointProperties(), e.KeyModifiers));
                });
            }
            finally
            {
                _updating = false;
            }
        }

        private void IciclePanel_OnPointerEntered(object sender, PointerEventArgs e)
        {
            // var relativePosition = e.GetPosition((Visual?)_control1.Parent);
            //
            // var p = RadialToCartesian.Transform(30 + relativePosition.Y, relativePosition.X, _control1.Bounds.Width);
            // var intitPoint = new Point(p.Item1, p.Item2);
            // var transformedPosition = intitPoint
            //     .Transform(Matrix.CreateTranslation(_control2.Bounds.Width/2, _control2.Bounds.Height/2));
            // Dispatcher.UIThread.Post(() =>
            // {
            //     _control2.RaiseEvent(
            //         new PointerEventArgs(e.RoutedEvent, e.Source, e.Pointer, (Visual?)_control1.Parent,
            //             transformedPosition, e.Timestamp, new PointerPointProperties(), e.KeyModifiers));
            // });
        }

        private void IciclePanel_OnPointerExited(object sender, PointerEventArgs e)
        {
            // var relativePosition = e.GetPosition((Visual?)_control1.Parent);
            //
            // var p = RadialToCartesian.Transform(30 + relativePosition.Y, relativePosition.X, _control1.Bounds.Width);
            // var intitPoint = new Point(p.Item1, p.Item2);
            // var transformedPosition = intitPoint
            //     .Transform(Matrix.CreateTranslation(_control2.Bounds.Width/2, _control2.Bounds.Height/2));
            // Dispatcher.UIThread.Post(() =>
            // {
            //     _control2.RaiseEvent(
            //         new PointerEventArgs(e.RoutedEvent, e.Source, e.Pointer, (Visual?)_control1.Parent,
            //             transformedPosition, e.Timestamp, new PointerPointProperties(), e.KeyModifiers));
            // });
        }

        private void SunburstPanel_OnPointerMoved(object sender, PointerEventArgs e)
        {
            var relativePosition = e.GetPosition(_control2);
            Canvas.SetLeft(_pointer2, relativePosition.X - _pointer2.Bounds.Width / 2);
            Canvas.SetTop(_pointer2, relativePosition.Y - _pointer2.Bounds.Height / 2);
            
            // try
            // {
            //     _updating = true;
            //
            //     var position = e.GetPosition(null);
            //     var root = (Visual)VisualRoot;
            //     // var transformedPosition = position.Transform(Matrix.CreateTranslation())
            //     var p = RadialToCartesian.TransformBack(relativePosition);
            //     var transformedPosition = new Point(p.Item1, p.Item2)
            //         .Transform(Matrix.CreateTranslation(- _control1.Bounds.Width - _control2.Bounds.Width/2, -_control2.Bounds.Height/2));
            //     // .TransformBack();
            //     _control1.RaiseEvent(
            //         new PointerEventArgs(e.RoutedEvent, e.Source, e.Pointer, (Visual?)_control2.Parent, transformedPosition, e.Timestamp, new PointerPointProperties(), e.KeyModifiers));
            // }
            // finally
            // {
            //     _updating = false;
            // }
        }
        
        private static Point ToCartesian(double radius, double width, double maxAngle, double totalWidth)
        {
            // Calculate angle proportionally to the total width
            double angleInDegrees = maxAngle * width / totalWidth;
        
            // Use the existing method to calculate the Cartesian coordinates
            return RadialToCartesian.ToCartesian(radius, angleInDegrees);
        }
    }

    public class IciclePanel3 : Panel
    {
        private const int NodeHeight = 10;
        
        protected override Size MeasureOverride(Size availableSize)
        {
            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (DataContext is not TreeViewPageViewModel vm)
                return base.ArrangeOverride(finalSize); 
            
            if (Children.Count == 0)
                InitChildren(vm);               
            
            Arrange(vm.Root, 0, 0, finalSize.Width);

            return finalSize;
        }
        
        private void Arrange(Node node, double x, double y, double width)
        {
            if (node == null) throw new ArgumentNullException(nameof(node), "Node cannot be null.");
            if (width <= 0) throw new ArgumentException(nameof(width), "Width must be positive.");

            node.Control.Arrange(new Rect(x, y, width, NodeHeight)); 
            
            if (node.Children.Count == 0)
                return;

            var childWidth = width / node.Children.Count;
            for (int index = 0; index < node.Children.Count; index++)
            {
                var child = node.Children[index];
                Arrange(child, x + index*childWidth, y + NodeHeight, childWidth);
            }
        }

        private void InitChildren(TreeViewPageViewModel vm)
        {
            vm.Root.Control = new ContentPresenter { Content = vm.Root };
            Children.Add(vm.Root.Control);
            
            InitChildren(vm.Root.Children);
        }

        private void InitChildren(IEnumerable<Node> nodes)
        {
            foreach (var node in nodes)
            {
                node.Control = new ContentPresenter { Content = node };
                Children.Add(node.Control);
            
                InitChildren(node.Children);
            }
        }
    }

    public class IcicleCanvas : Control
    {
        private const int NodeHeight = 10;
        private readonly IImmutableSolidColorBrush _nodeBrush = Brushes.Blue;
        private readonly Pen _nodePen = new(Brushes.White, 0.5);

        public override void Render(DrawingContext context)
        {
            if (DataContext is TreeViewPageViewModel vm && Bounds.Width > 0)
                Render(context, vm.Root, 0, 0, Bounds.Width);
            
            base.Render(context);
        }

        private void Render(DrawingContext context, Node node, double x, double y, double width)
        {
            if (context == null) throw new ArgumentNullException(nameof(context), "DrawingContext cannot be null.");
            if (node == null) throw new ArgumentNullException(nameof(node), "Node cannot be null.");
            if (width <= 0) throw new ArgumentException(nameof(width), "Width must be positive.");

            context.DrawRectangle(_nodeBrush, _nodePen, new Rect(x, y, width, NodeHeight));

            if (node.Children.Count == 0)
                return;

            var childWidth = width / node.Children.Count;
            for (int index = 0; index < node.Children.Count; index++)
            {
                var child = node.Children[index];
                Render(context, child, x + index*childWidth, y + NodeHeight, childWidth);
            }
        }
    }
}
