using MediatR;
using Cybernetic.Domain.Entities;
using Cybernetic.Infrastructure.Abstraction.Services;
using TaskStatus = Cybernetic.Domain.Entities.TaskStatus;

namespace Cybernetic.UseCases.Schedules.GenerateSchedule;

/// <summary>
/// Handler for <see cref="GenerateScheduleCommand"/>.
/// </summary>
public class GenerateScheduleCommandHandler : IRequestHandler<GenerateScheduleCommand, Schedule>
{
    private const int MaxTaskNameLength = 10;
    
    private readonly INameGenerator nameGenerator;
    private readonly Random random;

    /// <summary>
    /// Constructor.
    /// </summary>
    public GenerateScheduleCommandHandler(INameGenerator nameGenerator)
    {
        this.nameGenerator = nameGenerator;
        random = new Random();
    }

    /// <inheritdoc />
    public Task<Schedule> Handle(GenerateScheduleCommand command, CancellationToken cancellationToken)
    {
        ValidateCommand(command);
        var schedule = GenerateSchedule(command);
        return Task.FromResult(schedule);
    }

    private void ValidateCommand(GenerateScheduleCommand command)
    {
        if (command.MinLayersCount > command.MaxLayersCount || command.MinLayersCount< 1)
        {
            throw new ArgumentException("Invalid values of layers count.");
        }

        if (command.MinTasksCountPerLayer > command.MaxTasksCountPerLayer || command.MinTasksCountPerLayer < 1)
        {
            throw new ArgumentException("Invalid values of tasks count.");
        }

        if (command.StartTime > command.EndTime)
        {
            throw new ArgumentException("Invalid time period.");
        }

        var scheduleDuration = command.EndTime - command.StartTime;
        var tasksCount = scheduleDuration / command.MinTaskDuration;

        if (tasksCount < command.MaxTasksCountPerLayer)
        {
            throw new ArgumentException("With a specified schedule duration and minimum task duration, " +
                                        "you can't contain that many tasks.");
        }
    }
    
    private Schedule GenerateSchedule(GenerateScheduleCommand command)
    {
        var schedule = new Schedule();
        var numberOfLayers = random.Next(command.MinLayersCount, command.MaxLayersCount);
        
        for (int i = 0; i < numberOfLayers; i++)
        {
            var layer = GenerateLayer(command);
            schedule.AddLayer(layer);
        }

        return schedule;
    }

    private Layer GenerateLayer(GenerateScheduleCommand command)
    {
        var layer = new Layer();

        var tasksCount = random.Next(command.MinTasksCountPerLayer, command.MaxTasksCountPerLayer);
        var periods = SplitPeriodIntoSubPeriods(command.StartTime, command.EndTime, tasksCount);
        
        DateTime periodStarTime = periods.First().StartTime;
        foreach (var period in periods) 
        {
            var task = GenerateTaskInPeriod(command.MinTaskDuration, periodStarTime, period.EndTime);
            layer.AddTask(task);

            periodStarTime = task.EndTime;
        }

        return layer;
    }

    private List<(DateTime StartTime, DateTime EndTime)> SplitPeriodIntoSubPeriods(DateTime startTime, DateTime endTime, int numberOfSubPeriods)
    {
        var periodDuration = endTime - startTime;
        var subPeriodDuration = periodDuration / numberOfSubPeriods;

        var subPeriods = new List<(DateTime, DateTime)>();

        var subPeriodStartTime = startTime;
        for (int i = 0; i < numberOfSubPeriods; i++)
        {
            var subPeriodEndTime = subPeriodStartTime.Add(subPeriodDuration);
            subPeriods.Add(new (subPeriodStartTime, subPeriodEndTime));

            subPeriodStartTime = subPeriodEndTime;
        }

        return subPeriods;
    }

    private ScheduledTask GenerateTaskInPeriod(TimeSpan minDuration, DateTime startTime, DateTime endTime)
    {
        var periodDurationInMinutes = (int)(endTime - startTime).TotalMinutes;

        var taskDuration = random.Next((int)minDuration.TotalMinutes, periodDurationInMinutes);
        var fromMinutes = random.Next(periodDurationInMinutes - taskDuration);

        var taskStartTime = startTime.AddMinutes(fromMinutes);
        var taskEndTime = taskStartTime.AddMinutes(taskDuration);

        var nameLength = random.Next(MaxTaskNameLength);
        var name = nameGenerator.Generate(nameLength);

        var status = RandomTaskStatus();

        var task = new ScheduledTask(name, taskStartTime, taskEndTime, status);
        return task;
    }

    private TaskStatus RandomTaskStatus()
    {
        var statuses = Enum.GetValues(typeof(TaskStatus));
        return (TaskStatus)statuses.GetValue(random.Next(statuses.Length));
    }
}