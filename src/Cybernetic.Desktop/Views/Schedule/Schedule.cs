using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Cybernetic.Domain.Entities;

namespace Cybernetic.Desktop.Views.Schedules;

public class ScheduleControl : Canvas
{
    private const int RulerHeight = 50;
    private const int TaskElementHeight = 25;
    
    /// <summary>
    /// Scale factor. A distance of 500 relative units contains 1 task lasting 1 day.
    /// </summary>
    private static readonly double ScaleFactor = (double)500 / TimeSpan.FromDays(1).Ticks;

    private static readonly DependencyProperty ScheduleProperty = DependencyProperty
        .Register(nameof(Schedule), typeof(Schedule), typeof
            (ScheduleControl), new PropertyMetadata(OnScheduleChanged));

    private static void OnScheduleChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
    {
        var schedule = (Schedule)args.NewValue;
        var scheduleControl = (ScheduleControl)obj;

        if (scheduleControl != null)
        {
            scheduleControl.Width = schedule.Duration.Ticks * ScaleFactor;
            scheduleControl.AddScheduleElements();
        }
    }
    
    /// <summary>
    /// Schedule domain entity.
    /// </summary>
    public Schedule Schedule
    {
        get => (Schedule)GetValue(ScheduleProperty);
        set => SetValue(ScheduleProperty, value);
    }

    private void AddScheduleElements()
    {
        Children.Clear();

        var stepWidth = TimeSpan.FromHours(1).Ticks * ScaleFactor;
        var numberOfStepsInLargeStep = 5;
        AddRuler(stepWidth, numberOfStepsInLargeStep);

        var columnWidth = stepWidth * numberOfStepsInLargeStep;
        AddGrid(columnWidth);
    }

    private void AddRuler(double stepWidth, int numberOfStepsInLargeStep)
    {
        var ruler = new Ruler
        {
            Label = Schedule.StartTime.ToString("dddd, dd MMMM yyyy"),
            StepWidth = stepWidth,
            NumberOfStepsInLargeStep = numberOfStepsInLargeStep,
            Width = Width,
            Height = RulerHeight,
            Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#adafb3"),
            MarkupBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#363637")
        };
        
        Children.Add(ruler);
    }

    private void AddGrid(double columnWidth)
    {
        var grid = new ScheduleGrid
        {
            Width = Width,
            Height = ActualHeight - RulerHeight,
            RowHeight = TaskElementHeight,
            ColumnWidth = columnWidth,
            FirstRowBackground = (SolidColorBrush)new BrushConverter().ConvertFrom("#9ca3ad"),
            SecondRowBackground = (SolidColorBrush)new BrushConverter().ConvertFrom("#a8b2bf"),
            ColumnBackground = (SolidColorBrush)new BrushConverter().ConvertFrom("#898f96")
        };
        
        SetTop(grid, RulerHeight);

        Children.Add(grid);
    }
    
    private void AddLayers()
    {
        int layerIndex = 0;
        foreach (var layer in Schedule.Layers)
        {
            int taskIndex = 0;
            foreach (var task in layer.Tasks)
            {
                var taskPeriod = task.EndTime - task.StartTime;

                var taskControl = new ScheduleTaskControl();
                taskControl.Height = TaskElementHeight;
                taskControl.Width = taskPeriod.Ticks * ScaleFactor;
                taskControl.Content = task.Name;
                
                var left = (task.StartTime - Schedule.StartTime).Ticks * ScaleFactor;
                SetLeft(taskControl, left);
                SetTop(taskControl, 50 + layerIndex * TaskElementHeight);
                
                Children.Add(taskControl);
                taskIndex++;
            }

            layerIndex++;
        }
    }
}