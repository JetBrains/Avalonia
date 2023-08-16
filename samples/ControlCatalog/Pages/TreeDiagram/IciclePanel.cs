namespace ControlCatalog.Pages;

public class IciclePanel : TreeDiagramPanel<CartesianTreeDiagramVisual>
{
    protected override CartesianTreeDiagramVisual CreateDiagramVisual() => new();
}