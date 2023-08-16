namespace ControlCatalog.Pages;

public interface ITreeDiagramItem
{
    ITreeDiagramVisual DiagramVisual { get; }
    void InvalidateDiagramVisual();
}