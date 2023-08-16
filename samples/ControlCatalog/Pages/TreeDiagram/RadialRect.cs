using System;
using System.Globalization;
using System.Numerics;
using Avalonia;
using Avalonia.Utilities;
using Vector = Avalonia.Vector;

namespace ControlCatalog.Pages;

#pragma warning disable CS1591
public readonly struct RadialRect : IEquatable<RadialRect>
{
    /// <summary>
    /// The X position.
    /// </summary>
    private readonly double _centerRadius;

    /// <summary>
    /// The Y position.
    /// </summary>
    private readonly double _centerAngle;

    /// <summary>
    /// The width.
    /// </summary>
    private readonly double _radialWidth;

    /// <summary>
    /// The height.
    /// </summary>
    private readonly double _radialHeight;

    /// <summary>
    /// Initializes a new instance of the <see cref="RadialRect"/> structure.
    /// </summary>
    /// <param name="centerRadius">The X position.</param>
    /// <param name="centerAngle">The Y position.</param>
    /// <param name="radialWidth">The width.</param>
    /// <param name="radialHeight">The height.</param>
    public RadialRect(double centerRadius, double centerAngle, double radialWidth, double radialHeight)
    {
        _centerRadius = centerRadius;
        _centerAngle = centerAngle;
        _radialWidth = radialWidth;
        _radialHeight = radialHeight;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RadialRect"/> structure.
    /// </summary>
    /// <param name="size">The size of the rectangle.</param>
    public RadialRect(Size size)
    {
        _centerRadius = 0;
        _centerAngle = 0;
        _radialWidth = size.Width;
        _radialHeight = size.Height;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RadialRect"/> structure.
    /// </summary>
    /// <param name="position">The position of the rectangle.</param>
    /// <param name="size">The size of the rectangle.</param>
    public RadialRect(RadialPoint position, Size size)
    {
        _centerRadius = position.Radius;
        _centerAngle = position.Angle;
        _radialWidth = size.Width;
        _radialHeight = size.Height;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RadialRect"/> structure.
    /// </summary>
    /// <param name="topLeft">The top left position of the rectangle.</param>
    /// <param name="bottomRight">The bottom right position of the rectangle.</param>
    public RadialRect(Point topLeft, Point bottomRight)
    {
        _centerRadius = topLeft.X;
        _centerAngle = topLeft.Y;
        _radialWidth = bottomRight.X - topLeft.X;
        _radialHeight = bottomRight.Y - topLeft.Y;
    }

    /// <summary>
    /// Gets the X position.
    /// </summary>
    public double CenterRadius => _centerRadius;

    /// <summary>
    /// Gets the Y position.
    /// </summary>
    public double CenterAngle => _centerAngle;

    /// <summary>
    /// Gets the width.
    /// </summary>
    public double RadialWidth => _radialWidth;

    /// <summary>
    /// Gets the height.
    /// </summary>
    public double RadialHeight => _radialHeight;

    /// <summary>
    /// Gets the position of the rectangle.
    /// </summary>
    public Point Position => new(_centerRadius, _centerAngle);

    /// <summary>
    /// Gets the size of the rectangle.
    /// </summary>
    public Size Size => new(_radialWidth, _radialHeight);

    /// <summary>
    /// Gets the right position of the rectangle.
    /// </summary>
    public double Right => _centerRadius + _radialWidth;

    /// <summary>
    /// Gets the bottom position of the rectangle.
    /// </summary>
    public double Bottom => _centerAngle + _radialHeight;
    
    /// <summary>
    /// Gets the left position.
    /// </summary>
    public double Left => _centerRadius;

    /// <summary>
    /// Gets the top position.
    /// </summary>
    public double Top => _centerAngle;

    public RadialPoint TopLeft => new(_centerRadius + _radialHeight, _centerAngle - _radialWidth/2);

    public RadialPoint TopRight => new(_centerRadius + _radialHeight, _centerAngle + _radialWidth/2);

    public RadialPoint BottomLeft => new(_centerRadius, _centerAngle - _radialWidth/2);

    public RadialPoint BottomRight => new(_centerRadius, _centerAngle + _radialWidth/2);

    // public Point Center => new Point(_centerRadius, _centerAngle);
    
    public RadialPoint TopCenter => new(_centerRadius + _radialHeight, _centerAngle);

    public RadialPoint BottomCenter => new(_centerRadius, _centerAngle);


    /// <summary>
    /// Checks for equality between two <see cref="RadialRect"/>s.
    /// </summary>
    /// <param name="left">The first rect.</param>
    /// <param name="right">The second rect.</param>
    /// <returns>True if the rects are equal; otherwise false.</returns>
    public static bool operator ==(RadialRect left, RadialRect right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Checks for inequality between two <see cref="RadialRect"/>s.
    /// </summary>
    /// <param name="left">The first rect.</param>
    /// <param name="right">The second rect.</param>
    /// <returns>True if the rects are unequal; otherwise false.</returns>
    public static bool operator !=(RadialRect left, RadialRect right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Multiplies a rectangle by a scaling vector.
    /// </summary>
    /// <param name="rect">The rectangle.</param>
    /// <param name="scale">The vector scale.</param>
    /// <returns>The scaled rectangle.</returns>
    public static RadialRect operator *(RadialRect rect, Vector scale)
    {
        return new RadialRect(
            rect.CenterRadius * scale.X,
            rect.CenterAngle * scale.Y,
            rect.RadialWidth * scale.X,
            rect.RadialHeight * scale.Y);
    }
    
    /// <summary>
    /// Multiplies a rectangle by a scale.
    /// </summary>
    /// <param name="rect">The rectangle.</param>
    /// <param name="scale">The scale.</param>
    /// <returns>The scaled rectangle.</returns>
    public static RadialRect operator *(RadialRect rect, double scale)
    {
        return new RadialRect(
            rect.CenterRadius * scale,
            rect.CenterAngle * scale,
            rect.RadialWidth * scale,
            rect.RadialHeight * scale);
    }

    /// <summary>
    /// Divides a rectangle by a vector.
    /// </summary>
    /// <param name="rect">The rectangle.</param>
    /// <param name="scale">The vector scale.</param>
    /// <returns>The scaled rectangle.</returns>
    public static RadialRect operator /(RadialRect rect, Vector scale)
    {
        return new RadialRect(
            rect.CenterRadius / scale.X, 
            rect.CenterAngle / scale.Y, 
            rect.RadialWidth / scale.X, 
            rect.RadialHeight / scale.Y);
    }

    /// <summary>
    /// Determines whether a point is in the bounds of the rectangle.
    /// </summary>
    /// <param name="p">The point.</param>
    /// <returns>true if the point is in the bounds of the rectangle; otherwise false.</returns>
    public bool Contains(RadialPoint p)
    {
        return p.Radius >= _centerRadius && p.Radius <= _centerRadius + _radialHeight &&
               p.Angle >= _centerAngle - _radialWidth/2 && p.Angle <= _centerAngle + _radialWidth/2;
    }
    
    /// <summary>
    /// Determines whether a point is in the bounds of the rectangle, exclusive of the
    /// rectangle's bottom/right edge.
    /// </summary>
    /// <param name="p">The point.</param>
    /// <returns>true if the point is in the bounds of the rectangle; otherwise false.</returns>    
    public bool ContainsExclusive(RadialPoint p)
    {
        return p.Radius >= _centerRadius && p.Radius < _centerRadius + _radialWidth &&
               p.Angle >= _centerAngle && p.Angle < _centerAngle + _radialHeight;
    }

    /// <summary>
    /// Determines whether the rectangle fully contains another rectangle.
    /// </summary>
    /// <param name="r">The rectangle.</param>
    /// <returns>true if the rectangle is fully contained; otherwise false.</returns>
    public bool Contains(RadialRect r)
    {
        return Contains(r.TopLeft) && Contains(r.BottomRight);
    }

    /// <summary>
    /// Centers another rectangle in this rectangle.
    /// </summary>
    /// <param name="rect">The rectangle to center.</param>
    /// <returns>The centered rectangle.</returns>
    public RadialRect CenterRect(RadialRect rect)
    {
        return new RadialRect(
            _centerRadius + ((_radialWidth - rect._radialWidth) / 2),
            _centerAngle + ((_radialHeight - rect._radialHeight) / 2),
            rect._radialWidth,
            rect._radialHeight);
    }

    /// <summary>
    /// Inflates the rectangle.
    /// </summary>
    /// <param name="thickness">The thickness to be subtracted for each side of the rectangle.</param>
    /// <returns>The inflated rectangle.</returns>
    public RadialRect Inflate(double thickness)
    {
        return Inflate(new Thickness(thickness));
    }

    /// <summary>
    /// Inflates the rectangle.
    /// </summary>
    /// <param name="thickness">The thickness to be subtracted for each side of the rectangle.</param>
    /// <returns>The inflated rectangle.</returns>
    public RadialRect Inflate(Thickness thickness)
    {
        return new RadialRect(
            new Point(_centerRadius - thickness.Left, _centerAngle - thickness.Top),
            Size.Inflate(thickness));
    }

    /// <summary>
    /// Deflates the rectangle.
    /// </summary>
    /// <param name="thickness">The thickness to be subtracted for each side of the rectangle.</param>
    /// <returns>The deflated rectangle.</returns>
    public RadialRect Deflate(double thickness)
    {
        return Deflate(new Thickness(thickness));
    }

    /// <summary>
    /// Deflates the rectangle by a <see cref="Thickness"/>.
    /// </summary>
    /// <param name="thickness">The thickness to be subtracted for each side of the rectangle.</param>
    /// <returns>The deflated rectangle.</returns>
    public RadialRect Deflate(Thickness thickness)
    {
        return new RadialRect(
            new Point(_centerRadius + thickness.Left, _centerAngle + thickness.Top),
            Size.Deflate(thickness));
    }

    /// <summary>
    /// Returns a boolean indicating whether the rect is equal to the other given rect.
    /// </summary>
    /// <param name="other">The other rect to test equality against.</param>
    /// <returns>True if this rect is equal to other; False otherwise.</returns>
    public bool Equals(RadialRect other)
    {
        // ReSharper disable CompareOfFloatsByEqualityOperator
        return _centerRadius == other._centerRadius &&
               _centerAngle == other._centerAngle &&
               _radialWidth == other._radialWidth &&
               _radialHeight == other._radialHeight;
        // ReSharper enable CompareOfFloatsByEqualityOperator
    }

    /// <summary>
    /// Returns a boolean indicating whether the given object is equal to this rectangle.
    /// </summary>
    /// <param name="obj">The object to compare against.</param>
    /// <returns>True if the object is equal to this rectangle; false otherwise.</returns>
    public override bool Equals(object? obj) => obj is RadialRect other && Equals(other);

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>The hash code.</returns>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = (hash * 23) + CenterRadius.GetHashCode();
            hash = (hash * 23) + CenterAngle.GetHashCode();
            hash = (hash * 23) + RadialWidth.GetHashCode();
            hash = (hash * 23) + RadialHeight.GetHashCode();
            return hash;
        }
    }

    /// <summary>
    /// Gets the intersection of two rectangles.
    /// </summary>
    /// <param name="rect">The other rectangle.</param>
    /// <returns>The intersection.</returns>
    public RadialRect Intersect(RadialRect rect)
    {
        var newLeft = (rect.CenterRadius > CenterRadius) ? rect.CenterRadius : CenterRadius;
        var newTop = (rect.CenterAngle > CenterAngle) ? rect.CenterAngle : CenterAngle;
        var newRight = (rect.Right < Right) ? rect.Right : Right;
        var newBottom = (rect.Bottom < Bottom) ? rect.Bottom : Bottom;

        if ((newRight > newLeft) && (newBottom > newTop))
        {
            return new RadialRect(newLeft, newTop, newRight - newLeft, newBottom - newTop);
        }
        else
        {
            return default;
        }
    }

    /// <summary>
    /// Determines whether a rectangle intersects with this rectangle.
    /// </summary>
    /// <param name="rect">The other rectangle.</param>
    /// <returns>
    /// True if the specified rectangle intersects with this one; otherwise false.
    /// </returns>
    public bool Intersects(RadialRect rect)
    {
        return (rect.CenterRadius < Right) && (CenterRadius < rect.Right) && (rect.CenterAngle < Bottom) && (CenterAngle < rect.Bottom);
    }

    /// <summary>
    /// Returns the axis-aligned bounding box of a transformed rectangle.
    /// </summary>
    /// <param name="matrix">The transform.</param>
    /// <returns>The bounding box</returns>
    public RadialRect TransformToAABB(Matrix matrix)
    {
        ReadOnlySpan<Point> points = stackalloc Point[4]
        {
            TopLeft.Transform(matrix),
            TopRight.Transform(matrix),
            BottomRight.Transform(matrix),
            BottomLeft.Transform(matrix)
        };

        var left = double.MaxValue;
        var right = double.MinValue;
        var top = double.MaxValue;
        var bottom = double.MinValue;

        foreach (var p in points)
        {
            if (p.X < left) left = p.X;
            if (p.X > right) right = p.X;
            if (p.Y < top) top = p.Y;
            if (p.Y > bottom) bottom = p.Y;
        }

        return new RadialRect(new Point(left, top), new Point(right, bottom));
    }
    
    internal RadialRect TransformToAABB(Matrix4x4 matrix)
    {
        ReadOnlySpan<Point> points = stackalloc Point[4]
        {
            TopLeft.Transform(matrix),
            TopRight.Transform(matrix),
            BottomRight.Transform(matrix),
            BottomLeft.Transform(matrix)
        };

        var left = double.MaxValue;
        var right = double.MinValue;
        var top = double.MaxValue;
        var bottom = double.MinValue;

        foreach (var p in points)
        {
            if (p.X < left) left = p.X;
            if (p.X > right) right = p.X;
            if (p.Y < top) top = p.Y;
            if (p.Y > bottom) bottom = p.Y;
        }

        return new RadialRect(new Point(left, top), new Point(right, bottom));
    }

    /// <summary>
    /// Translates the rectangle by an offset.
    /// </summary>
    /// <param name="offset">The offset.</param>
    /// <returns>The translated rectangle.</returns>
    public RadialRect Translate(Vector offset)
    {
        return new RadialRect(Position + offset, Size);
    }

    /// <summary>
    /// Normalizes the rectangle so both the <see cref="RadialWidth"/> and <see 
    /// cref="RadialHeight"/> are positive, without changing the location of the rectangle
    /// </summary>
    /// <returns>Normalized Rect</returns>
    /// <remarks>
    /// Empty rect will be return when Rect contains invalid values. Like NaN.
    /// </remarks>
    public RadialRect Normalize()
    {
        RadialRect rect = this;

        if(double.IsNaN(rect.Right) || double.IsNaN(rect.Bottom) || 
           double.IsNaN(rect.CenterRadius) || double.IsNaN(rect.CenterAngle) || 
           double.IsNaN(RadialHeight) || double.IsNaN(RadialWidth))
        {
            return default;
        }

        if (rect.RadialWidth < 0)
        {
            var x = CenterRadius + RadialWidth;
            var width = CenterRadius - x;

            rect = rect.WithX(x).WithWidth(width);
        }

        if (rect.RadialHeight < 0)
        {
            var y = CenterAngle + RadialHeight;
            var height = CenterAngle - y;

            rect = rect.WithY(y).WithHeight(height);
        }

        return rect;
    }

    /// <summary>
    /// Gets the union of two rectangles.
    /// </summary>
    /// <param name="rect">The other rectangle.</param>
    /// <returns>The union.</returns>
    public RadialRect Union(RadialRect rect)
    {
        if (RadialWidth == 0 && RadialHeight == 0)
        {
            return rect;
        }
        else if (rect.RadialWidth == 0 && rect.RadialHeight == 0)
        {
            return this;
        }
        else
        {
            var x1 = Math.Min(CenterRadius, rect.CenterRadius);
            var x2 = Math.Max(Right, rect.Right);
            var y1 = Math.Min(CenterAngle, rect.CenterAngle);
            var y2 = Math.Max(Bottom, rect.Bottom);

            return new RadialRect(new Point(x1, y1), new Point(x2, y2));
        }
    }

    internal static RadialRect? Union(RadialRect? left, RadialRect? right)
    {
        if (left == null)
            return right;
        if (right == null)
            return left;
        return left.Value.Union(right.Value);
    }

    /// <summary>
    /// Returns a new <see cref="RadialRect"/> with the specified X position.
    /// </summary>
    /// <param name="x">The x position.</param>
    /// <returns>The new <see cref="RadialRect"/>.</returns>
    public RadialRect WithX(double x)
    {
        return new RadialRect(x, _centerAngle, _radialWidth, _radialHeight);
    }

    /// <summary>
    /// Returns a new <see cref="RadialRect"/> with the specified Y position.
    /// </summary>
    /// <param name="y">The y position.</param>
    /// <returns>The new <see cref="RadialRect"/>.</returns>
    public RadialRect WithY(double y)
    {
        return new RadialRect(_centerRadius, y, _radialWidth, _radialHeight);
    }

    /// <summary>
    /// Returns a new <see cref="RadialRect"/> with the specified width.
    /// </summary>
    /// <param name="width">The width.</param>
    /// <returns>The new <see cref="RadialRect"/>.</returns>
    public RadialRect WithWidth(double width)
    {
        return new RadialRect(_centerRadius, _centerAngle, width, _radialHeight);
    }

    /// <summary>
    /// Returns a new <see cref="RadialRect"/> with the specified height.
    /// </summary>
    /// <param name="height">The height.</param>
    /// <returns>The new <see cref="RadialRect"/>.</returns>
    public RadialRect WithHeight(double height)
    {
        return new RadialRect(_centerRadius, _centerAngle, _radialWidth, height);
    }

    /// <summary>
    /// Returns the string representation of the rectangle.
    /// </summary>
    /// <returns>The string representation of the rectangle.</returns>
    public override string ToString()
    {
        return string.Format(
            CultureInfo.InvariantCulture,
            "{0}, {1}, {2}, {3}",
            _centerRadius,
            _centerAngle,
            _radialWidth,
            _radialHeight);
    }

    /// <summary>
    /// Parses a <see cref="RadialRect"/> string.
    /// </summary>
    /// <param name="s">The string.</param>
    /// <returns>The parsed <see cref="RadialRect"/>.</returns>
    public static RadialRect Parse(string s)
    {
        using (var tokenizer = new StringTokenizer(s, CultureInfo.InvariantCulture, exceptionMessage: "Invalid Rect."))
        {
            return new RadialRect(
                tokenizer.ReadDouble(),
                tokenizer.ReadDouble(),
                tokenizer.ReadDouble(),
                tokenizer.ReadDouble()
            );
        }
    }

    /// <summary>
    /// This method should be used internally to check for the rect emptiness
    /// Once we add support for WPF-like empty rects, there will be an actual implementation
    /// For now it's internal to keep some loud community members happy about the API being pretty 
    /// </summary>
    internal bool IsEmpty() => this == default;
}
