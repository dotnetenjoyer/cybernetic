using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Cybernetic.Domain.Entities;

namespace Cybernetic.Desktop.Views.Schedules;

/// <summary>
/// Schedule ribon control.
/// </summary>
public partial class ScheduleRibbon : UserControl
{
    private static readonly DependencyProperty GenerateScheduleCommandProperty = DependencyProperty
        .Register(nameof(GenerateScheduleCommand), typeof(ICommand), typeof(ScheduleRibbon));

    /// <summary>
    /// Command to generate a new schedule.
    /// </summary>
    public ICommand GenerateScheduleCommand
    {
        get => (ICommand)GetValue(GenerateScheduleCommandProperty);
        set => SetValue(GenerateScheduleCommandProperty, value);
    }
    
    private static readonly DependencyProperty ScheduleProperty = DependencyProperty
        .Register(nameof(Schedule), typeof(Schedule), typeof(ScheduleRibbon));

    /// <summary>
    /// Schedule.
    /// </summary>
    public Schedule Schedule
    {
        get => (Schedule)GetValue(ScheduleProperty);
        set => SetValue(ScheduleProperty, value);
    }
    
    /// <summary>
    /// Constructor.
    /// </summary>
    public ScheduleRibbon()
    {
        InitializeComponent();
    }
}