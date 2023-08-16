using Avalonia;
using Avalonia.Platform;

namespace ControlCatalog.Pages;

public class CartesianTreeDiagramVisual : ITreeDiagramVisual
{
    public Rect Bounds { get; private set; }

    public IGeometryImpl CreateGeometryImpl()
    {
        var factory = AvaloniaLocator.Current.GetRequiredService<IPlatformRenderInterface>();
        return factory.CreateRectangleGeometry(Bounds);
    }

    public bool Contains(Point screenPoint)
    {
        return Bounds.Contains(screenPoint);
    }
    
    public void Arrange(double x, double y, double width, double height, Size totalSize)
    {
        Bounds = new Rect(x, y, width, height);
    }
}