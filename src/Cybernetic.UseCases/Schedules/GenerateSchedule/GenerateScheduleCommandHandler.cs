using Cybernetic.Domain.Entities;
using Cybernetic.Infrastructure.Abstraction.Services;
using MediatR;

namespace Cybernetic.UseCases.Schedules.GenerateSchedule;

/// <summary>
/// Handler for <see cref="GenerateScheduleCommand"/>.
/// </summary>
public class GenerateScheduleCommandHandler : IRequestHandler<GenerateScheduleCommand, Schedule>
{
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
        var schedule = GenerateTestSchedule();
        return Task.FromResult(schedule);
        
        // ValidateCommand(command);
        // var schedule = GenerateSchedule(command);
        // return Task.FromResult(schedule);
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
    }
    
    private Schedule GenerateSchedule(GenerateScheduleCommand command)
    {
        var schedule = new Schedule();

        for (int i = 0; i < command.MinLayersCount; i++)
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
        var periodStartTime = command.StartTime;

        for (int i = 0; i < tasksCount; i++)
        {
            var task = GenerateTaskInPeriod(periodStartTime, command.EndTime);
            periodStartTime = task.EndTime;
            
            layer.AddTask(task);
        }

        return layer;
    }

    private ScheduledTask GenerateTaskInPeriod(DateTime startTime, DateTime endTime)
    {
        var periodDuration = endTime - startTime;
        var periodDurationMinutes = (int)periodDuration.TotalMinutes;

        var fromMinutes = random.Next(periodDurationMinutes);
        var taskDuration = random.Next(periodDurationMinutes - fromMinutes);

        var taskStartTime = startTime.AddMinutes(fromMinutes);
        var taskEndTime = taskStartTime.AddMinutes(taskDuration);

        var taskNameLength = random.Next(10);
        var taskName = nameGenerator.Generate(taskNameLength);
        
        var task = new ScheduledTask(taskName, taskStartTime, taskEndTime);
        return task;
    }

    private Schedule GenerateTestSchedule()
    {
        var schedule = new Schedule();

        var layer = new Layer();
        schedule.AddLayer(layer);
        
        layer.AddTask(new ScheduledTask("First task", DateTime.Now.Date.AddDays(-2), DateTime.Now.Date.AddDays(-1)));
        layer.AddTask(new ScheduledTask("Second task", DateTime.Now.Date.AddDays(-1), DateTime.Now.Date));
        layer.AddTask(new ScheduledTask("Third task", DateTime.Now.Date, DateTime.Now.Date.AddDays(1)));

        var layer2 = new Layer();
        schedule.AddLayer(layer2);
        
        layer2.AddTask(new ScheduledTask("#1 task", DateTime.Now.AddDays(-1), DateTime.Now));

        return schedule;
    }
}