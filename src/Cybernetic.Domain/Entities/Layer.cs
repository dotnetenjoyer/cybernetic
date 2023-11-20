namespace Cybernetic.Domain.Entities;

/// <summary>
/// Schedule layer.
/// </summary>
public class Layer
{
    private List<ScheduledTask> tasks;

    /// <summary>
    /// Constructor.
    /// </summary>
    public Layer()
    {
        Id = Guid.NewGuid();
        tasks = new List<ScheduledTask>();
    }
    
    /// <summary>
    /// Layer ID.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Layer related tasks.
    /// </summary>
    public IEnumerable<ScheduledTask> Tasks => tasks;

    /// <summary>
    /// Add a new task to layer.
    /// </summary>
    /// <param name="task">Task to add.</param>
    public void AddTask(ScheduledTask task)
    {
        tasks.Add(task);
    }

    /// <summary>
    /// Remove a task from layer.
    /// </summary>
    /// <param name="task">Task to remove.</param>
    public void RemoveTask(ScheduledTask task)
    {
        tasks.Remove(task);
    }
}