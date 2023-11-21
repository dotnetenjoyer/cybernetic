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
    public IReadOnlyCollection<Layer> Layers => layers;

    /// <summary>
    /// Schedule start time.
    /// </summary>
    public DateTime StartTime { get; private set; }
    
    /// <summary>
    /// Schedule end time.
    /// </summary>
    public DateTime EndTime { get; private set; }

    /// <summary>
    /// Schedule duration.
    /// </summary>
    public TimeSpan Duration => EndTime - StartTime;

    /// <summary>
    /// Add a new layer to schedule.
    /// </summary>
    /// <param name="layer">Layer to add.</param>
    public void AddLayer(Layer layer)
    {
        layers.Add(layer);
        CalculateScheduleTimes();
    }

    /// <summary>
    /// Remove a layer from schedule.
    /// </summary>
    /// <param name="layer">Layer to remove.</param>
    public void RemoveLayer(Layer layer)
    {
        layers.Remove(layer);
        CalculateScheduleTimes();
    }

    private void CalculateScheduleTimes()
    {
        var times = layers.SelectMany(x => x.Tasks.SelectMany(x => new[] { x.StartTime, x.EndTime }));

        if (times.Any())
        {
            StartTime = times.Min();
            EndTime = times.Max();            
        }
    }
}