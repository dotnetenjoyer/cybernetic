using Cybernetic.Domain.Entities;
using MediatR;

namespace Cybernetic.UseCases.Schedules.GenerateSchedule;

/// <summary>
/// Command to generate some schedule.
/// </summary>
public class GenerateScheduleCommand : IRequest<Schedule>
{
    /// <summary>
    /// Maximum number of layers.
    /// </summary>
    public int MaxLayersCount { get; set; } = 1;
    
    /// <summary>
    /// Minimum number of layers.
    /// </summary>
    public int MinLayersCount { get; set; } = 1;
    
    /// <summary>
    /// Minimum number of tasks in layer.
    /// </summary>
    public int MinTasksCountPerLayer { get; set; } = 1;
    
    /// <summary>
    /// Maximum number of tasks in layer.
    /// </summary>
    public int MaxTasksCountPerLayer { get; set; } = 1;
    
    /// <summary>
    /// Schedule start time.
    /// </summary>
    public DateTime StartTime { get; set; }
    
    /// <summary>
    /// Schedule end time.
    /// </summary>
    public DateTime EndTime { get; set; }
}