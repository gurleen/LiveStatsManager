namespace LiveStatsManager.Services.Graphics;

public class GraphicsManager
{
    private const int MaxLayers = 10;
    private GraphicLayer[] _layers { get; } = new GraphicLayer[MaxLayers];
    public List<GraphicLayer> Layers => _layers.ToList();

    private void InitLayers()
    {
        Enumerable.Range(0, MaxLayers)
            .Select(i => new GraphicLayer(i, string.Empty))
            .ToArray()
            .CopyTo(_layers, 0);
    }
    
    public GraphicsManager()
    {
        InitLayers();
    }
}