using Avalonia;
using Avalonia.Platform;

namespace ControlCatalog.Pages;

public interface ITreeDiagramVisual
{
    IGeometryImpl CreateGeometryImpl();
    bool Contains(Point screenPoint);
    void Arrange(double x, double y, double width, double height, Size totalSize);
}