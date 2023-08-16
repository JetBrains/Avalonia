using Avalonia;
using Avalonia.Media;
using Avalonia.Platform;

namespace ControlCatalog.Pages;

#pragma warning disable CS1591
/// <summary>
/// Represents a rectangle geometry optimized for performance.
/// </summary>
/// <remarks>
/// The <see cref="FastRectangleGeometry"/> class is designed to provide a faster, lightweight representation 
/// of a rectangular geometry by directly using fields for storage and management. This can reduce overhead 
/// compared to mechanisms that use styled properties or other richer features. As a result, 
/// <see cref="FastRectangleGeometry"/> may be more suitable for performance-critical scenarios where 
/// the overhead of additional features is not required or desirable.
/// </remarks>
public class FastRectangleGeometry : Geometry
{
    private Rect _rect;

    /// <summary>
    /// Initializes a new instance of the <see cref="FastRectangleGeometry"/> class.
    /// </summary>
    public FastRectangleGeometry()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FastRectangleGeometry"/> class.
    /// </summary>
    /// <param name="rect">The rectangle bounds.</param>
    public FastRectangleGeometry(Rect rect)
    {
        Rect = rect;
    }

    /// <summary>
    /// Gets or sets the bounds of the rectangle.
    /// </summary>
    public Rect Rect
    {
        get => _rect;
        set
        {
            _rect = value;
            InvalidateGeometry();
        }
    }

    /// <inheritdoc/>
    public override Geometry Clone() => new FastRectangleGeometry(Rect);

    private protected sealed override IGeometryImpl? CreateDefiningGeometry()
    {
        var factory = AvaloniaLocator.Current.GetRequiredService<IPlatformRenderInterface>();
        return factory.CreateRectangleGeometry(Rect);
    }
}
