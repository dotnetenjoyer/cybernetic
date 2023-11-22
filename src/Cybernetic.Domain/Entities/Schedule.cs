namespace Cybernetic.Domain.Entities;

/// <summary>
/// Schedule
/// </summary>
public class Schedule
{
    private List<Layer> layers;

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
    public DateTime? StartTime { get; private set; }
    
    /// <summary>
    /// Schedule end time.
    /// </summary>
    public DateTime? EndTime { get; private set; }

    /// <summary>
    /// Schedule duration.
    /// </summary>
    public TimeSpan? Duration => EndTime - StartTime;

    /// <summary>
    /// Number of pending tasks.
    /// </summary>
    public int PendingTasksNumber { get; private set; }
    
    /// <summary>
    /// Number of error tasks.
    /// </summary>
    public int ErrorTasksNumber { get; private set; }
    
    /// <summary>
    /// Number of completed tasks.
    /// </summary>
    public int CompletedTasksNumber { get; private set; }
    
    /// <summary>
    /// Constructor.
    /// </summary>
    public Schedule()
    {
        Id = Guid.NewGuid();
        layers = new List<Layer>();
    }
    
    /// <summary>
    /// Add a new layer to schedule.
    /// </summary>
    /// <param name="layer">Layer to add.</param>
    public void AddLayer(Layer layer)
    {
        layers.Add(layer);
        UpdateTasksRelatedProperties();

        layer.TaskAdded += HandleTaskAdded;
        layer.TaskRemoved += HandleTaskRemoved;
    }

    /// <summary>
    /// Remove a layer from schedule.
    /// </summary>
    /// <param name="layer">Layer to remove.</param>
    public void RemoveLayer(Layer layer)
    {
        var result = layers.Remove(layer);

        if (result)
        {
            UpdateTasksRelatedProperties();
            
            layer.TaskAdded -= HandleTaskAdded;
            layer.TaskRemoved -= HandleTaskRemoved;
        }
    }

    private void HandleTaskAdded(ScheduledTask task)
    {
        UpdateTasksRelatedProperties();
    }
    
    private void HandleTaskRemoved(ScheduledTask task)
    {
        UpdateTasksRelatedProperties();
    }
    
    private void UpdateTasksRelatedProperties()
    {
        StartTime = EndTime = default;
        PendingTasksNumber = ErrorTasksNumber = CompletedTasksNumber = default;
        
        var tasks = layers
            .SelectMany(l => l.Tasks)
            .ToList();
        
        var times = tasks
            .SelectMany(t => new[] { t.StartTime, t.EndTime })
            .ToList();

        if (!tasks.Any())
        {
            return;
        }
        
        StartTime = times.Min();
        EndTime = times.Max();

        CalculateTasksNumbersByStatus(tasks);
    }

    private void CalculateTasksNumbersByStatus(IEnumerable<ScheduledTask> tasks)
    {
        foreach (var task in tasks)
        {
            switch (task.Status)
            {
                case TaskStatus.Pending:
                    PendingTasksNumber++;       
                    break;
                case TaskStatus.Error:
                    ErrorTasksNumber++;
                    break;
                case TaskStatus.Completed:
                    CompletedTasksNumber++;
                    break;
            }
        }
    }
}