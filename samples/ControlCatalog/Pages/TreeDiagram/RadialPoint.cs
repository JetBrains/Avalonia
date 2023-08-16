using System;
using System.Globalization;
using System.Numerics;
using Avalonia;
using Avalonia.Utilities;
using Vector = Avalonia.Vector;

namespace ControlCatalog.Pages;

#pragma warning disable CS1591
public readonly struct RadialPoint : IEquatable<RadialPoint>
{
    /// <summary>
    /// The X position.
    /// </summary>
    private readonly double _radius;

    /// <summary>
    /// The Y position.
    /// </summary>
    private readonly double _angle;

    /// <summary>
    /// Initializes a new instance of the <see cref="RadialPoint"/> structure.
    /// </summary>
    /// <param name="radius">The X position.</param>
    /// <param name="angle">The Y position.</param>
    public RadialPoint(double radius, double angle)
    {
        _radius = radius;
        _angle = angle;
    }

    /// <summary>
    /// Gets the X position.
    /// </summary>
    public double Radius => _radius;

    /// <summary>
    /// Gets the Y position.
    /// </summary>
    public double Angle => _angle;

    /// <summary>
    /// Converts the <see cref="RadialPoint"/> to a <see cref="Vector"/>.
    /// </summary>
    /// <param name="p">The point.</param>
    public static implicit operator Vector(RadialPoint p)
    {
        return new Vector(p._radius, p._angle);
    }

    /// <summary>
    /// Converts the <see cref="RadialPoint"/> to a <see cref="Point"/>.
    /// </summary>
    /// <param name="p">The point.</param>
    public static implicit operator Point(RadialPoint p)
    {
        return new Point(p._radius, p._angle);
    }
    
    public static implicit operator RadialPoint(Point p)
    {
        return new RadialPoint(p.X, p.Y);
    }

    /// <summary>
    /// Negates a point.
    /// </summary>
    /// <param name="a">The point.</param>
    /// <returns>The negated point.</returns>
    public static RadialPoint operator -(RadialPoint a)
    {
        return new RadialPoint(-a._radius, -a._angle);
    }

    /// <summary>
    /// Checks for equality between two <see cref="RadialPoint"/>s.
    /// </summary>
    /// <param name="left">The first point.</param>
    /// <param name="right">The second point.</param>
    /// <returns>True if the points are equal; otherwise false.</returns>
    public static bool operator ==(RadialPoint left, RadialPoint right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Checks for inequality between two <see cref="RadialPoint"/>s.
    /// </summary>
    /// <param name="left">The first point.</param>
    /// <param name="right">The second point.</param>
    /// <returns>True if the points are unequal; otherwise false.</returns>
    public static bool operator !=(RadialPoint left, RadialPoint right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Adds two points.
    /// </summary>
    /// <param name="a">The first point.</param>
    /// <param name="b">The second point.</param>
    /// <returns>A point that is the result of the addition.</returns>
    public static RadialPoint operator +(RadialPoint a, RadialPoint b)
    {
        return new RadialPoint(a._radius + b._radius, a._angle + b._angle);
    }

    /// <summary>
    /// Adds a vector to a point.
    /// </summary>
    /// <param name="a">The point.</param>
    /// <param name="b">The vector.</param>
    /// <returns>A point that is the result of the addition.</returns>
    public static RadialPoint operator +(RadialPoint a, Vector b)
    {
        return new RadialPoint(a._radius + b.X, a._angle + b.Y);
    }

    /// <summary>
    /// Subtracts two points.
    /// </summary>
    /// <param name="a">The first point.</param>
    /// <param name="b">The second point.</param>
    /// <returns>A point that is the result of the subtraction.</returns>
    public static RadialPoint operator -(RadialPoint a, RadialPoint b)
    {
        return new RadialPoint(a._radius - b._radius, a._angle - b._angle);
    }

    /// <summary>
    /// Subtracts a vector from a point.
    /// </summary>
    /// <param name="a">The point.</param>
    /// <param name="b">The vector.</param>
    /// <returns>A point that is the result of the subtraction.</returns>
    public static RadialPoint operator -(RadialPoint a, Vector b)
    {
        return new RadialPoint(a._radius - b.X, a._angle - b.Y);
    }

    /// <summary>
    /// Multiplies a point by a factor coordinate-wise
    /// </summary>
    /// <param name="p">RadialPoint to multiply</param>
    /// <param name="k">Factor</param>
    /// <returns>RadialPoints having its coordinates multiplied</returns>
    public static RadialPoint operator *(RadialPoint p, double k) => new(p.Radius * k, p.Angle * k);

    /// <summary>
    /// Multiplies a point by a factor coordinate-wise
    /// </summary>
    /// <param name="p">RadialPoint to multiply</param>
    /// <param name="k">Factor</param>
    /// <returns>RadialPoints having its coordinates multiplied</returns>
    public static RadialPoint operator *(double k, RadialPoint p) => new(p.Radius * k, p.Angle * k);

    /// <summary>
    /// Divides a point by a factor coordinate-wise
    /// </summary>
    /// <param name="p">RadialPoint to divide by</param>
    /// <param name="k">Factor</param>
    /// <returns>RadialPoints having its coordinates divided</returns>
    public static RadialPoint operator /(RadialPoint p, double k) => new(p.Radius / k, p.Angle / k);

    /// <summary>
    /// Applies a matrix to a point.
    /// </summary>
    /// <param name="point">The point.</param>
    /// <param name="matrix">The matrix.</param>
    /// <returns>The resulting point.</returns>
    public static RadialPoint operator *(RadialPoint point, Matrix matrix) => matrix.Transform(point);

    /// <summary>
    /// Computes the Euclidean distance between the two given points.
    /// </summary>
    /// <param name="value1">The first point.</param>
    /// <param name="value2">The second point.</param>
    /// <returns>The Euclidean distance.</returns>
    public static double Distance(RadialPoint value1, RadialPoint value2)
    {
        double distanceSquared = ((value2.Radius - value1.Radius) * (value2.Radius - value1.Radius)) +
                                 ((value2.Angle - value1.Angle) * (value2.Angle - value1.Angle));
        return Math.Sqrt(distanceSquared);
    }

    /// <summary>
    /// Parses a <see cref="RadialPoint"/> string.
    /// </summary>
    /// <param name="s">The string.</param>
    /// <returns>The <see cref="RadialPoint"/>.</returns>
    public static RadialPoint Parse(string s)
    {
        using (var tokenizer = new StringTokenizer(s, CultureInfo.InvariantCulture, exceptionMessage: "Invalid RadialPoint."))
        {
            return new RadialPoint(
                tokenizer.ReadDouble(),
                tokenizer.ReadDouble()
            );
        }
    }

    /// <summary>
    /// Returns a boolean indicating whether the point is equal to the other given point (bitwise).
    /// </summary>
    /// <param name="other">The other point to test equality against.</param>
    /// <returns>True if this point is equal to other; False otherwise.</returns>
    public bool Equals(RadialPoint other)
    {
        // ReSharper disable CompareOfFloatsByEqualityOperator
        return _radius == other._radius &&
               _angle == other._angle;
        // ReSharper enable CompareOfFloatsByEqualityOperator
    }

    /// <summary>
    /// Returns a boolean indicating whether the point is equal to the other given point
    /// (numerically).
    /// </summary>
    /// <param name="other">The other point to test equality against.</param>
    /// <returns>True if this point is equal to other; False otherwise.</returns>
    public bool NearlyEquals(RadialPoint other)
    {
        return MathUtilities.AreClose(_radius, other._radius) &&
               MathUtilities.AreClose(_angle, other._angle);
    }

    /// <summary>
    /// Checks for equality between a point and an object.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <returns>
    /// True if <paramref name="obj"/> is a point that equals the current point.
    /// </returns>
    public override bool Equals(object? obj) => obj is RadialPoint other && Equals(other);

    /// <summary>
    /// Returns a hash code for a <see cref="RadialPoint"/>.
    /// </summary>
    /// <returns>The hash code.</returns>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = (hash * 23) + _radius.GetHashCode();
            hash = (hash * 23) + _angle.GetHashCode();
            return hash;
        }
    }

    /// <summary>
    /// Returns the string representation of the point.
    /// </summary>
    /// <returns>The string representation of the point.</returns>
    public override string ToString()
    {
        return string.Format(CultureInfo.InvariantCulture, "{0}, {1}", _radius, _angle);
    }

    /// <summary>
    /// Transforms the point by a matrix.
    /// </summary>
    /// <param name="transform">The transform.</param>
    /// <returns>The transformed point.</returns>
    public RadialPoint Transform(Matrix transform) => transform.Transform(this);
    
    internal RadialPoint Transform(Matrix4x4 matrix)
    {
        var vec = Vector2.Transform(new Vector2((float)Radius, (float)Angle), matrix);
        return new RadialPoint(vec.X, vec.Y);
    }

    /// <summary>
    /// Returns a new point with the specified X coordinate.
    /// </summary>
    /// <param name="x">The X coordinate.</param>
    /// <returns>The new point.</returns>
    public RadialPoint WithRadius(double x)
    {
        return new RadialPoint(x, _angle);
    }

    /// <summary>
    /// Returns a new point with the specified Y coordinate.
    /// </summary>
    /// <param name="y">The Y coordinate.</param>
    /// <returns>The new point.</returns>
    public RadialPoint WithAngle(double y)
    {
        return new RadialPoint(_radius, y);
    }

    /// <summary>
    /// Deconstructs the point into its X and Y coordinates.
    /// </summary>
    /// <param name="x">The X coordinate.</param>
    /// <param name="y">The Y coordinate.</param>
    public void Deconstruct(out double x, out double y)
    {
        x = this._radius;
        y = this._angle;
    }
}
