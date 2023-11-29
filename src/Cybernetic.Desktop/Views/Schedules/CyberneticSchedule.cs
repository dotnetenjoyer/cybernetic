using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Cybernetic.Domain.Entities;

namespace Cybernetic.Desktop.Views.Schedules;

public class CyberneticSchedule : Canvas
{
    #region DependencyProperties

    private static readonly DependencyProperty ScheduleProperty = DependencyProperty
        .Register(nameof(Schedule), typeof(Schedule),
            typeof(CyberneticSchedule), new PropertyMetadata(OnScheduleChanged));
    
    private static void OnScheduleChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
    {
        var control = (CyberneticSchedule)obj;
        if (control != null)
        {
            control.InitializeSchedule();
            control.InvalidateVisual();
        }
    }

    private static readonly DependencyProperty OneHourWidthProperty = DependencyProperty
        .Register(nameof(OneHourWidth), typeof(double), 
            typeof(CyberneticSchedule), new PropertyMetadata((double)40));

    private static readonly DependencyProperty ShortestTaskWidthProperty = DependencyProperty
        .Register(nameof(ShortestTaskWidth), typeof(double), 
            typeof(CyberneticSchedule), new PropertyMetadata((double)30));
    
    private static readonly DependencyProperty TaskHeightProperty = DependencyProperty
        .Register(nameof(TaskHeight), typeof(int), 
            typeof(CyberneticSchedule), new PropertyMetadata(22));
    
    private static readonly DependencyProperty GridRowHeightProperty = DependencyProperty
        .Register(nameof(GridRowHeight), typeof(double), typeof(CyberneticSchedule));
    
    private static readonly DependencyProperty CompletedTaskBackgroundProperty = DependencyProperty
        .Register(nameof(CompletedTaskBackground), typeof(Brush), typeof(CyberneticSchedule));
    
    private static readonly DependencyProperty ErrorTaskBackgroundProperty = DependencyProperty
        .Register(nameof(ErrorTaskBackground), typeof(Brush), typeof(CyberneticSchedule));
    
    private static readonly DependencyProperty PendingTaskBackgroundProperty = DependencyProperty
        .Register(nameof(PendingTaskBackground), typeof(Brush), typeof(CyberneticSchedule));

    #endregion
    
    private double scaleFactor = 1;
    
    /// <summary>
    /// Width of one hour on the schedule diagram.
    /// </summary>
    public double OneHourWidth
    {
        get => (double)GetValue(OneHourWidthProperty);
        set => SetValue(OneHourWidthProperty, value);
    }
    
    /// <summary>
    /// Width of shortest scheduled task.
    /// </summary>
    public double ShortestTaskWidth
    {
        get => (double)GetValue(ShortestTaskWidthProperty); 
        set => SetValue(ShortestTaskWidthProperty, value);
    }
    
    /// <summary>
    /// Task element heights.
    /// </summary>
    public int TaskHeight
    {
        get => (int)GetValue(TaskHeightProperty); 
        set => SetValue(TaskHeightProperty, value);
    }
    
    /// <summary>
    /// Schedule grid row height.
    /// </summary>
    public double GridRowHeight
    {
        get => (double)GetValue(GridRowHeightProperty);
        set => SetValue(GridRowHeightProperty, value);
    }
    
    /// <summary>
    /// Schedule domain entity.
    /// </summary>
    public Schedule? Schedule
    {
        get => (Schedule)GetValue(ScheduleProperty);
        set => SetValue(ScheduleProperty, value);
    }

    /// <summary>
    /// Completed task background brush.
    /// </summary>
    public Brush CompletedTaskBackground
    {
        get => (Brush)GetValue(CompletedTaskBackgroundProperty);
        set => SetValue(CompletedTaskBackgroundProperty, value);
    }
    
    /// <summary>
    /// Error task background brush.
    /// </summary>
    public Brush ErrorTaskBackground
    {
        get => (Brush)GetValue(ErrorTaskBackgroundProperty);
        set => SetValue(ErrorTaskBackgroundProperty, value);
    }
    
    /// <summary>
    /// Pending task background brush.
    /// </summary>
    public Brush PendingTaskBackground
    {
        get => (Brush)GetValue(PendingTaskBackgroundProperty);
        set => SetValue(PendingTaskBackgroundProperty, value);
    }
    
    private void InitializeSchedule()
    {
        if (Schedule == null || Schedule.Duration == null)
        {
            return;
        }

        CalculateScaleFactor();
        CalculateScheduleSize();

        OneHourWidth = TimeSpan.FromHours(1).Ticks * scaleFactor;
    }

    private void CalculateScaleFactor()
    {
        scaleFactor = ShortestTaskWidth / Schedule.ShortestTaskDuration.Value.Ticks;
    }

    private void CalculateScheduleSize()
    {
        Width = Schedule.Duration.Value.Ticks * scaleFactor;
        MinHeight = Schedule.Layers.Count * GridRowHeight;
    }

    /// <inheritdoc />
    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);
        DrawSchedule(drawingContext);
    }

    private void DrawSchedule(DrawingContext drawingContext)
    {
        if (Schedule?.Duration == null)
        {
            // Nothing to draw.
            return;
        }
        
        var layerIndex = 0;

        foreach (var layer in Schedule.Layers)
        {
            foreach (var task in layer.Tasks)
            {
                DrawTask(drawingContext, task, layerIndex);
            }
            
            layerIndex++;
        }
    }
    
    private void DrawTask(DrawingContext drawingContext, ScheduledTask task, int layerIndex)
    {
        const int taskElementOffset = 2;
        const int taskElementCornerRadius = 2;
        const int taskNameOffset = 2;
        
        var taskElement = new Rect
        {
            Width = (task.EndTime - task.StartTime).Ticks * scaleFactor,
            Height = TaskHeight,
            X = (task.StartTime - Schedule.StartTime.Value).Ticks * scaleFactor,
            Y = GridRowHeight * layerIndex + taskElementOffset
        };

        var brush = GetTaskBrush(task.Status);
        var pen = new Pen(Brushes.Black, 1);
        drawingContext.DrawRoundedRectangle(brush, pen, taskElement, taskElementCornerRadius, taskElementCornerRadius);

        var taskText = new FormattedText(task.Name, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, 
            new Typeface("Verdana"), 12, Brushes.White);
        taskText.MaxTextWidth = taskElement.Width - taskNameOffset;
        
        var textPosition = new Point(taskElement.X + taskNameOffset, taskElement.Y + taskNameOffset);

        drawingContext.DrawText(taskText, textPosition);
    }
    

    private Brush GetTaskBrush(TaskStatus status)
    {
        switch (status)
        {
            case TaskStatus.Completed:
                return CompletedTaskBackground;
            case TaskStatus.Error:
                return ErrorTaskBackground;
            case TaskStatus.Pending:
                return PendingTaskBackground;
            default:
                throw new NotImplementedException();
        }
    }
}