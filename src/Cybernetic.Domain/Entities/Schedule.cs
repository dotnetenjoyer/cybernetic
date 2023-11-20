namespace Cybernetic.Domain.Entities;

/// <summary>
/// Schedule
/// </summary>
public class Schedule
{
    private List<Layer> layers;
    
    /// <summary>
    /// Constructor.
    /// </summary>
    public Schedule()
    {
        Id = Guid.NewGuid();
        layers = new List<Layer>();
    }
    
    /// <summary>
    /// Schedule ID.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Schedule related layers.
    /// </summary>
    public IEnumerable<Layer> Layers => layers;
    
    /// <summary>
    /// Add a new layer to schedule.
    /// </summary>
    /// <param name="layer">Layer to add.</param>
    public void AddLayer(Layer layer)
    {
        layers.Add(layer);
    }

    /// <summary>
    /// Remove a layer from schedule.
    /// </summary>
    /// <param name="layer">Layer to remove.</param>
    public void RemoveLayer(Layer layer)
    {
        layers.Remove(layer);
    }
}