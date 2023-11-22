using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Cybernetic.Domain.Entities;

namespace Cybernetic.Desktop.Views.Schedules;

public class CyberneticSchedule : Canvas
{
    private const int RulerHeight = 50;
    private const int TaskElementHeight = 22;
    private const int RowHeight = 25;

    private const int PresentLineZIndex = 100;
    private const int PresentLineUpdatePeriod = 1000;
    
    private ScheduleGrid? scheduleGrid;
    
    private Rectangle? presentLine;
    private Timer? presentLineUpdateTimer;
    
    /// <summary>
    /// Scale factor. A distance of 500 relative units contains 1 task lasting 1 day.
    /// </summary>
    private static readonly double ScaleFactor = (double)500 / TimeSpan.FromDays(1).Ticks;

    private static readonly DependencyProperty ScheduleProperty = DependencyProperty
        .Register(nameof(Schedule), typeof(Schedule),
            typeof(CyberneticSchedule), new PropertyMetadata(OnScheduleChanged));

    private static void OnScheduleChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
    {
        var schedule = (Schedule)args.NewValue;
        var scheduleControl = (CyberneticSchedule)obj;

        if (scheduleControl != null && schedule.Duration.HasValue)
        {
            scheduleControl.Width = schedule.Duration.Value.Ticks * ScaleFactor;
            scheduleControl.AddScheduleElements();
        }
    }
    
    /// <summary>
    /// Schedule domain entity.
    /// </summary>
    public Schedule? Schedule
    {
        get => (Schedule)GetValue(ScheduleProperty);
        set => SetValue(ScheduleProperty, value);
    }

    #region PresentLineProperties

    private static readonly DependencyProperty PresentLineFillBrushProperty = DependencyProperty
        .Register(nameof(PresentLineFillBrush), typeof(Brush), 
            typeof(CyberneticSchedule), new PropertyMetadata(Brushes.Yellow));

    /// <summary>
    /// Present line fill brush.
    /// </summary>
    public Brush PresentLineFillBrush
    {
        get => (Brush)GetValue(PresentLineFillBrushProperty);
        set => SetValue(PresentLineFillBrushProperty, value);
    }
    
    private static readonly DependencyProperty PresentLineStrokeBrushProperty = DependencyProperty
        .Register(nameof(PresentLineStrokeBrush), typeof(Brush), 
            typeof(CyberneticSchedule), new PropertyMetadata(Brushes.Black));

    /// <summary>
    /// Present line stroke brush.
    /// </summary>
    public Brush PresentLineStrokeBrush
    {
        get => (Brush)GetValue(PresentLineStrokeBrushProperty);
        set => SetValue(PresentLineStrokeBrushProperty, value);
    }

    #endregion

    /// <summary>
    /// Constructor.
    /// </summary>
    public CyberneticSchedule()
    {
        SizeChanged += HandleSizeChange;
    }

    private void HandleSizeChange(object sender, RoutedEventArgs args)
    {
        if (scheduleGrid != null)
        {
            scheduleGrid.Height = ActualHeight - RulerHeight;
            scheduleGrid.Width = ActualWidth;                
        }

        if (presentLine != null)
        {
            presentLine.Height = ActualHeight;
        }
    }

    private void AddScheduleElements()
    {
        if (Schedule == null)
        {
            return;
        }
        
        Children.Clear();

        var stepWidth = TimeSpan.FromHours(1).Ticks * ScaleFactor;
        var numberOfStepsInLargeStep = 5;
        AddRuler(stepWidth, numberOfStepsInLargeStep);

        var columnWidth = stepWidth * numberOfStepsInLargeStep;
        AddGrid(columnWidth);
        
        AddPresentLine();
        UpdatePresentLinePosition();
        
        AddLayers();
    }

    private void AddRuler(double stepWidth, int numberOfStepsInLargeStep)
    {
        var ruler = new Ruler
        {
            Label = Schedule.StartTime.Value.ToString("dddd, dd MMMM yyyy"),
            StepWidth = stepWidth,
            NumberOfStepsInLargeStep = numberOfStepsInLargeStep,
            Width = Width,
            Height = RulerHeight,
            Background = (Brush)FindResource("RulerBackground"),
            MarkupBrush = (Brush)FindResource("RulerMarkupBrush")
        };
        
        Children.Add(ruler);
    }

    private void AddGrid(double columnWidth)
    {
        scheduleGrid = new ScheduleGrid
        {
            Width = Width,
            Height = ActualHeight - RulerHeight - RowHeight,
            RowHeight = RowHeight,
            ColumnWidth = columnWidth,
            FirstRowBackground = (SolidColorBrush)new BrushConverter().ConvertFrom("#9ca3ad"),
            SecondRowBackground = (SolidColorBrush)new BrushConverter().ConvertFrom("#a8b2bf"),
            ColumnBackground = (SolidColorBrush)new BrushConverter().ConvertFrom("#898f96")
        };
        
        SetTop(scheduleGrid, RulerHeight + RowHeight);
        
        Children.Add(scheduleGrid);
    }

    private void AddPresentLine()
    {
        presentLine = new Rectangle
        {
            Width = 4,
            StrokeThickness = 1,
            Height = ActualHeight,
            Stroke = PresentLineStrokeBrush,
            Fill = PresentLineFillBrush
        };     
        
        SetZIndex(presentLine, PresentLineZIndex);
        Children.Add(presentLine);
        
        presentLineUpdateTimer = new Timer(state =>
        {
            Application.Current.Dispatcher.Invoke(UpdatePresentLinePosition);
        }, null, PresentLineUpdatePeriod, PresentLineUpdatePeriod);
    }

    private void UpdatePresentLinePosition()
    {
        if (presentLine == null)
        {
            return;
        }

        var timeOffset = DateTime.Now - Schedule.StartTime;
        var positionOffset = timeOffset.Value.Ticks * ScaleFactor;
        SetLeft(presentLine, positionOffset);
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
                taskControl.DataContext = task;
                
                
                var left = (task.StartTime - Schedule.StartTime.Value).Ticks * ScaleFactor;
                SetLeft(taskControl, left);
                SetTop(taskControl, RulerHeight + RowHeight + layerIndex * RowHeight + 2);
                
                Children.Add(taskControl);
                taskIndex++;
            }

            layerIndex++;
        }
    }
}