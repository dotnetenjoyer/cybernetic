namespace Cybernetic.Domain.Entities;

/// <summary>
/// Represent scheduled tasks.
/// </summary>
public class ScheduledTask
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="name"></param>
    public ScheduledTask(string name, DateTime startTime, DateTime endTime)
    {
        Id = new Guid();
        Name = name;
        StartTime = startTime;
        EndTime = endTime;
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