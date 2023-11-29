using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.Input;
using MediatR;
using Cybernetic.Domain.Entities;
using Cybernetic.UseCases.Schedules.GenerateSchedule;

namespace Cybernetic.Desktop.MVVM.ViewModels;

/// <summary>
/// Schedule view model.
/// </summary>
public class ScheduleViewModel : BaseViewModel
{
    private readonly IMediator mediator;

    /// <summary>
    /// Schedule.
    /// </summary>
    public Schedule Schedule
    {
        get => schedule; 
        set => SetProperty(ref schedule, value);
    }

    private Schedule schedule;
    
    /// <summary>
    /// Command to generate schedule.
    /// </summary>
    public ICommand GenerateScheduleCommand { get; }
    
    /// <summary>
    /// Constructor.
    /// </summary>
    public ScheduleViewModel(IMediator mediator)
    {
        this.mediator = mediator;
        GenerateScheduleCommand = new RelayCommand(() 
            => GenerateScheduleAsync());
    }

    /// <inheritdoc />
    public override Task LoadAsync()
    {
        return GenerateScheduleAsync();
    }

    private async Task GenerateScheduleAsync()
    {
        var generateCommand = new GenerateScheduleCommand
        {
            MinLayersCount = 15,
            MaxLayersCount = 15,
            MinTasksCountPerLayer = 200,
            MaxTasksCountPerLayer = 200,
            StartTime = DateTime.Now.Date.AddDays(-30),
            EndTime = DateTime.Now.AddDays(30)
        };

        var schedule = await mediator.Send(generateCommand);    
        Schedule = schedule;
    }
}