using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Rendering;
using ControlCatalog.ViewModels;

namespace ControlCatalog.Pages;

public abstract class TreeDiagramPanel<TDiagramVisual> : TreeDiagramPanel, ICustomHitTest2 where TDiagramVisual : ITreeDiagramVisual
{
    private const int NodeHeight = 10;
    
    private ITreeDiagramDataSource? _dataSource;
    private readonly Dictionary<Node, ITreeDiagramItem> _itemsMap = new();

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == DataSourceProperty)
        {
            if (change.OldValue is ITreeDiagramDataSource oldValue)
            {
                //detach from data source                
            }
            if (change.NewValue is ITreeDiagramDataSource newValue)
                Initialize(newValue);
        }
    }

    private void Initialize(ITreeDiagramDataSource dataSource)
    {
        _dataSource = dataSource;
        _itemsMap.Clear();
        Children.Clear();
        AddChildren(dataSource.Root, 0);
    }

    private void AddChildren(Node node, int level)
    {
        var item = new TreeDiagramItem(node, level, CreateDiagramVisual());
        _itemsMap.Add(node, item);
        
        Children.Add(item);
        
        foreach (var child in node.Children) 
            AddChildren(child, ++level);
    }
    
    protected override Size MeasureOverride(Size availableSize)
    {
        return new Size();
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        if (_dataSource != null)
            ArrangeNode(_dataSource.Root, 0, 0, finalSize.Width, NodeHeight, finalSize);

        return finalSize;
    }

    private void ArrangeNode(Node node, double x, double y, double width, double height, Size totalSize)
    {
        if (node == null) throw new ArgumentNullException(nameof(node), "Node cannot be null.");
        if (width <= 0) throw new ArgumentException(nameof(width), "Width must be positive.");
        if (height <= 0) throw new ArgumentException(nameof(width), "Height must be positive.");
        
        ArrangeNode(_itemsMap[node], node, x, y, width, height, totalSize);
            
        if (node.Children.Count == 0)
            return;

        //TODO: extract size calculation
        var childWidth = width / node.Children.Count;
        var childHeight = NodeHeight;
        // var childHeight = NodeHeight + s_rand.Next(-3, 3);
        
        for (int index = 0; index < node.Children.Count; index++)
        {
            var child = node.Children[index];
            ArrangeNode(child, x + index*childWidth, y + height, childWidth, childHeight, totalSize);
        }
    }

    protected void ArrangeNode(ITreeDiagramItem item, Node node, double x, double y, double width, double height, Size totalSize)
    {
        item.DiagramVisual.Arrange(x, y, width, height, totalSize);
        item.InvalidateDiagramVisual();
    }

    protected abstract TDiagramVisual CreateDiagramVisual();
    
    public Visual? GetVisualAt(Point p, Func<Visual, bool>? filter)
    {
        for (var i = 0; i < Children.Count; i++)
        {
            if (Children[i] is TreeDiagramItem item && item.DiagramVisual.Contains(p))
                return item.Path;
        }

        return Bounds.Contains(p) ? this : null;
        // return Children
        //     .OfType<TreeDiagramItem>()
        //     .Where(item => item.DiagramVisual.Contains(p))
        //     .Select(item => item.Path as Visual)
        //     .FirstOrDefault() ?? (Bounds.Contains(p) ? this : null);
    }
}

public abstract class TreeDiagramPanel : Panel
{
    public static readonly StyledProperty<ITreeDiagramDataSource> DataSourceProperty =
        AvaloniaProperty.Register<TreeDiagramPanel, ITreeDiagramDataSource>(nameof(DataSource));
    
    public ITreeDiagramDataSource DataSource
    {
        get => GetValue(DataSourceProperty);
        set => SetValue(DataSourceProperty, value);
    }
}
