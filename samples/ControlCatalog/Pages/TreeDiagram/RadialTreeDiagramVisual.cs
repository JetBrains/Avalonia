using System;
using Avalonia;
using Avalonia.Media;
using Avalonia.Platform;

namespace ControlCatalog.Pages;

public class RadialTreeDiagramVisual : ITreeDiagramVisual 
{
    private readonly SunburstPanel _sunburstPanel;

    public RadialTreeDiagramVisual(SunburstPanel sunburstPanel)
    {
        _sunburstPanel = sunburstPanel ?? throw new ArgumentNullException(nameof(sunburstPanel));
    }

    public RadialRect Bounds { get; private set; }

    public IGeometryImpl CreateGeometryImpl()
    {
        var factory = AvaloniaLocator.Current.GetRequiredService<IPlatformRenderInterface>();
        var geometry = factory.CreateStreamGeometry();

        using var ctx = new StreamGeometryContext(geometry.Open());
        
        ctx.SetFillRule(FillRule.NonZero);
            
        ctx.BeginFigure(Bounds.TopLeft.ToCartesian(), true);

        ctx.ArcTo(Bounds.TopRight.ToCartesian(), new Size(Bounds.CenterRadius + Bounds.RadialHeight, Bounds.CenterRadius + Bounds.RadialHeight), 0, Bounds.RadialWidth > 180, SweepDirection.Clockwise);
        ctx.LineTo(Bounds.BottomRight.ToCartesian());
        ctx.ArcTo(Bounds.BottomLeft.ToCartesian(), new Size(Bounds.CenterRadius, Bounds.CenterRadius), 0, Bounds.RadialWidth > 180, SweepDirection.CounterClockwise);

        ctx.EndFigure(true);

        return geometry;
    }

    public bool Contains(Point screenPoint)
    {
        var radialPoint = screenPoint.ToRadial();
        return Bounds.Contains(radialPoint);
    }

    public void Arrange(double x, double y, double width, double height, Size totalSize)
    {
        double maxAngle = _sunburstPanel.MaxAngle;
        double centerAngle = maxAngle * (x + width/2) / totalSize.Width;
        double radialWidth = maxAngle * width / totalSize.Width;
        Bounds = new RadialRect(_sunburstPanel.InitialRadius + y, centerAngle, radialWidth, height);
    }
}