using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;
using Avalonia.Rendering;
using ControlCatalog.ViewModels;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace ControlCatalog.Pages;

public class SunburstPanel : TreeDiagramPanel<RadialTreeDiagramVisual>
{
    public static readonly StyledProperty<double> MaxAngleProperty = AvaloniaProperty.Register<SunburstPanel, double>(nameof(MaxAngleProperty), 360);
    
    public double MaxAngle
    {
        get => GetValue(MaxAngleProperty);
        set => SetValue(MaxAngleProperty, value);
    }

    public double InitialRadius { get; set; } = 30;
    
    static SunburstPanel()
    {
        AffectsArrange<SunburstPanel>(MaxAngleProperty);
        AffectsRender<SunburstPanel>(MaxAngleProperty);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var result = base.ArrangeOverride(finalSize);

        RenderTransform = new TranslateTransform(result.Width / 2, result.Height / 2);
        
        return result;
    }

    protected override RadialTreeDiagramVisual CreateDiagramVisual() => new(this);
}

