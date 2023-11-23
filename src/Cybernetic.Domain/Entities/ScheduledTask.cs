namespace Cybernetic.Domain.Entities;

/// <summary>
/// Represent scheduled tasks.
/// </summary>
public class ScheduledTask
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="name">Task name.</param>
    /// <param name="startTime">Start time.</param>
    /// <param name="endTime">End time.</param>
    /// <param name="status">Task status.</param>
    public ScheduledTask(string name, DateTime startTime, DateTime endTime, TaskStatus status = TaskStatus.Pending)
    {
        Id = Guid.NewGuid();
        Name = name;
        StartTime = startTime;
        EndTime = endTime;
        Status = status;
    }
    
    /// <summary>
    /// Task ID.
    /// </summary>
    public Guid Id { get; }
    
    /// <summary>
    /// Task name.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Task start time.
    /// </summary>
    public DateTime StartTime { get; set; }
    
    /// <summary>
    /// Task end time.
    /// </summary>
    public DateTime EndTime { get; set; }
    
    /// <summary>
    /// Task status.
    /// </summary>
    public TaskStatus Status { get; set; }
}