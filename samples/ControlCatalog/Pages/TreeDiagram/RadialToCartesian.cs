using System;
using Avalonia;

namespace ControlCatalog.Pages;

#pragma warning disable CS1591
public static class RadialToCartesian
{
    public static Point ToCartesian(this RadialPoint radialPoint)
    {
        return ToCartesian(radialPoint.Radius, radialPoint.Angle);
    }
    
    public static Point ToCartesian(double radius, double angleInDegrees)
    {
        // Convert angle in degrees to radians
        double angleInRadians = Math.PI * angleInDegrees / 180.0;

        // Calculate the x and y coordinates
        double x = radius * Math.Cos(angleInRadians);
        double y = radius * Math.Sin(angleInRadians);

        return new Point(x, y);
    }
    
    public static RadialPoint ToRadial(this Point point)
    {
        return ToRadial(point.X, point.Y);
    }
    
    // Function to convert cartesian coordinates to radial coordinates
    public static RadialPoint ToRadial(double x, double y)
    {
        // Calculate the radius
        double radius = Math.Sqrt(x * x + y * y);

        // Calculate the angle in radians
        double angleInRadians = Math.Atan2(y, x);

        // Convert angle in radians to degrees
        double angleInDegrees = angleInRadians * 180.0 / Math.PI;

        if (angleInDegrees < 0)
            angleInDegrees = 360 + angleInDegrees;

        // Return the radius and angle as a tuple
        return new RadialPoint(radius, angleInDegrees);
    }
}
