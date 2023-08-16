using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia.Platform;
using ControlCatalog.ViewModels;

namespace ControlCatalog.Pages;

#pragma warning disable CS1591
public class SunburstControl : Control
{
    public static readonly StyledProperty<double> MaxAngleProperty
        = AvaloniaProperty.Register<SunburstPanel, double>(nameof(MaxAngleProperty), 360);
    
    public double MaxAngle
    {
        get => GetValue(MaxAngleProperty);
        set => SetValue(MaxAngleProperty, value);
    }
    
    static SunburstControl()
    {
        // AffectsArrange<SunburstControl>(MaxAngleProperty);
        AffectsRender<SunburstControl>(MaxAngleProperty);
    }
    
    private const double InitialRadius = 30;
    private const int NodeHeight = 10;

    public override void Render(DrawingContext context)
    {
        if (DataContext is not TreeViewPageViewModel vm)
            return; 
        
        if (Bounds.Width > 0)
            Render(context, vm.Root, 0, 0, Bounds.Width, Bounds.Size);
        
        base.Render(context);
    }

    private void Render(DrawingContext context, Node node, double x, double y, double width, Size totalSize)
    {
        if (node == null) throw new ArgumentNullException(nameof(node), "Node cannot be null.");
        if (width <= 0) throw new ArgumentException(nameof(width), "Width must be positive.");

        RenderNode(context, node, x, y, width, NodeHeight, totalSize);
            
        if (node.Children.Count == 0)
            return;

        var childWidth = width / node.Children.Count;
        for (int index = 0; index < node.Children.Count; index++)
        {
            var child = node.Children[index];
            Render(context, child, x + index*childWidth, y + NodeHeight, childWidth, totalSize);
        }
    }

    private void RenderNode(DrawingContext context, Node node, double x, double y, double width, double height, Size totalSize)
    {
        double maxAngle = MaxAngle;
        double angle = maxAngle * (x + width / 2) / totalSize.Width;
        double radialWidth = maxAngle * width / totalSize.Width;
        var rect = new RadialRect(InitialRadius + y, angle, radialWidth, height);

        var geometry = CreateDefiningGeometry(rect, 0, 0);

        var stroke = Brushes.White;
        var pen = new ImmutablePen(stroke.ToImmutable());
        context.DrawGeometry(Brushes.Blue, pen, geometry);
    }

    private IGeometryImpl CreateDefiningGeometry(RadialRect rect, double offsetX, double offsetY)
    {
        var factory = AvaloniaLocator.Current.GetRequiredService<IPlatformRenderInterface>();
        var geometry = factory.CreateStreamGeometry();

        using var ctx = new StreamGeometryContext(geometry.Open());
        
        ctx.SetFillRule(FillRule.NonZero);
            
        ctx.BeginFigure(rect.TopLeft.ToCartesian(), true);

        ctx.ArcTo(rect.TopRight.ToCartesian(), new Size(rect.CenterRadius + rect.RadialHeight, rect.CenterRadius + rect.RadialHeight), 0, false, SweepDirection.Clockwise);
        ctx.LineTo(rect.BottomRight.ToCartesian());
        ctx.ArcTo(rect.BottomLeft.ToCartesian(), new Size(rect.CenterRadius + rect.RadialHeight, rect.CenterRadius + rect.RadialHeight), 0, false, SweepDirection.CounterClockwise);

        ctx.EndFigure(true);

        return geometry;
    }
}
