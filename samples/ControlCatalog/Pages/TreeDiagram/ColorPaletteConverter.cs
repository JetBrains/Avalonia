using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace ControlCatalog.Pages;

public class ColorPaletteConverter : IValueConverter
{
    private static readonly List<Color> GradientPalette;

    static ColorPaletteConverter()
    {
        GradientPalette = GenerateGradient(Colors.Blue, Colors.Gold, 30);
    }

    private static List<Color> GenerateGradient(Color start, Color end, int steps)
    {
        var gradient = new List<Color>();

        for (int i = 0; i < steps; i++)
        {
            var r = start.R + ((end.R - start.R) * i / (steps - 1));
            var g = start.G + ((end.G - start.G) * i / (steps - 1));
            var b = start.B + ((end.B - start.B) * i / (steps - 1));
            gradient.Add(Color.FromRgb((byte)r, (byte)g, (byte)b));
        }

        return gradient;
    }

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int treeLevel)
        {
            if (treeLevel >= 0 && treeLevel < GradientPalette.Count)
            {
                return new SolidColorBrush(GradientPalette[treeLevel]);
            }
        }

        return Brushes.Blue;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

