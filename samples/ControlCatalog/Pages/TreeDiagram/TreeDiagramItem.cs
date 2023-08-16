using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using ControlCatalog.ViewModels;

namespace ControlCatalog.Pages;

public class TreeDiagramItem : TemplatedControl, ITreeDiagramItem
{
    public static readonly StyledProperty<int> TreeLevelProperty = AvaloniaProperty.Register<TreeDiagramItem, int>(nameof(TreeLevelProperty));
    
    public int TreeLevel
    {
        get => GetValue(TreeLevelProperty);
        set => SetValue(TreeLevelProperty, value);
    }
    
    public Node Node { get; }
    public FastPath? Path { get; private set; }
    
    public ITreeDiagramVisual DiagramVisual { get; }

    public TreeDiagramItem(Node node, int treeLevel, ITreeDiagramVisual diagramVisual)
    {
        if (treeLevel < 0) throw new ArgumentOutOfRangeException(nameof(treeLevel));
        
        DiagramVisual = diagramVisual ?? throw new ArgumentNullException(nameof(diagramVisual));
        Node = node ?? throw new ArgumentNullException(nameof(node));
        TreeLevel = treeLevel;
        DataContext = node;
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
            
        Path = e.NameScope.Find<FastPath>("PART_Path");
    }

    public void InvalidateDiagramVisual()
    {
        // dummy. We need to init IsMeasureValid/IsArrangedValid only once.
        // while we always pass the same value, this has an affect only one time
        if (!IsMeasureValid)
            Measure(new Size());
        if (!IsArrangeValid)
            Arrange(new Rect());
            
        ApplyStyling();
        ApplyTemplate();

        if (Path != null)
        {
            Path.Data = DiagramVisual.CreateGeometryImpl();

            if (Path.Clip is FastRectangleGeometry clip)
                clip.Rect = Path.Data.Bounds;
            else
                Path.Clip = new FastRectangleGeometry(Path.Data.Bounds); // setting Avalonia property is slow
            
            Path.InvalidateVisual();
        }
    }
}