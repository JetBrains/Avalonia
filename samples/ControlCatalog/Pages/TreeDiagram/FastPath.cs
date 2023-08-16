using Avalonia;
using Avalonia.Platform;

namespace ControlCatalog.Pages;

#pragma warning disable CS1591
public class FastPath : FastShape
{
    public IGeometryImpl Data { get; set; }

    protected override IGeometryImpl CreateDefiningGeometry() => Data;

    public void SetBounds(Rect rect)
    {
        Bounds = rect;
    }
}
