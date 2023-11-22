namespace Cybernetic.Domain.Entities;

/// <summary>
/// Schedule layer.
/// </summary>
public class Layer
{
    private readonly List<ScheduledTask> tasks;

    /// <summary>
    /// Layer ID.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Layer related tasks.
    /// </summary>
    public IReadOnlyCollection<ScheduledTask> Tasks => tasks;

    /// <summary>
    /// Occurs when adding a task to a layer.
    /// </summary>
    public event Action<ScheduledTask> TaskAdded;

    /// <summary>
    /// Occurs when removing a task from a layer.
    /// </summary>
    public event Action<ScheduledTask> TaskRemoved;
    
    /// <summary>
    /// Constructor.
    /// </summary>
    public Layer()
    {
        Id = Guid.NewGuid();
        tasks = new List<ScheduledTask>();
    }
    
    /// <summary>
    /// Add a new task to layer.
    /// </summary>
    /// <param name="task">Task to add.</param>
    public void AddTask(ScheduledTask task)
    {
        tasks.Add(task);
        TaskAdded?.Invoke(task);
    }

    /// <summary>
    /// Remove a task from layer.
    /// </summary>
    /// <param name="task">Task to remove.</param>
    public void RemoveTask(ScheduledTask task)
    {
        var result = tasks.Remove(task);
        if (result)
        {
            TaskRemoved?.Invoke(task);
        }
    }
}