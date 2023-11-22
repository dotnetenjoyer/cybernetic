using System.Windows.Input;
using MediatR;
using Cybernetic.Domain.Entities;
using Cybernetic.UseCases.Schedules.GenerateSchedule;
using Microsoft.Toolkit.Mvvm.Input;

namespace Cybernetic.Desktop.MVVM.ViewModels;

/// <summary>
/// Main window view model.
/// </summary>
public class MainViewModel : BaseViewModel
{
    private readonly IMediator mediator;
    
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="mediator"></param>
    public MainViewModel(IMediator mediator)
    {
        this.mediator = mediator;

        GenerateScheduleCommand = new RelayCommand(() => GenerateScheduleAsync());
    }

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

    public override Task LoadAsync()
    {
        return GenerateScheduleAsync();
    }

    private async Task GenerateScheduleAsync()
    {
        var generateCommand = new GenerateScheduleCommand
        {
            MinLayersCount = 10,
            MaxLayersCount = 10,
            MinTasksCountPerLayer = 4,
            MaxTasksCountPerLayer = 20,
            StartTime = DateTime.Now.Date.AddDays(-4),
            EndTime = DateTime.Now.AddDays(2)
        };

        var schedule = await mediator.Send(generateCommand);    
        Schedule = schedule;
    }
}