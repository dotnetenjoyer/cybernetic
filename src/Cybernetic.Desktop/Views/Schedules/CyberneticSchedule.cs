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
    #region DependencyProperties

    private static readonly DependencyProperty RulerHeightProperty = DependencyProperty
        .Register(nameof(RulerHeight), typeof(int), 
            typeof(CyberneticSchedule), new PropertyMetadata(50));

    /// <summary>
    /// Schedule ruler height.
    /// </summary>
    public int RulerHeight
    {
        get => (int)GetValue(RulerHeightProperty); 
        set => SetValue(RulerHeightProperty, value);
    }
    
    private static readonly DependencyProperty RibbonHeightProperty = DependencyProperty
        .Register(nameof(RibbonHeight), typeof(int), 
            typeof(CyberneticSchedule), new PropertyMetadata(25));

    /// <summary>
    /// Schedule ribbon height.
    /// </summary>
    public int RibbonHeight
    {
        get => (int)GetValue(RibbonHeightProperty); 
        set => SetValue(RibbonHeightProperty, value);
    }
    
    private static readonly DependencyProperty GridRowHeightProperty = DependencyProperty
        .Register(nameof(GridRowHeight), typeof(int), 
            typeof(CyberneticSchedule), new PropertyMetadata(25));

    /// <summary>
    /// Grid row height.
    /// </summary>
    public int GridRowHeight
    {
        get => (int)GetValue(GridRowHeightProperty); 
        set => SetValue(GridRowHeightProperty, value);
    }
    
    private static readonly DependencyProperty ShortestTaskWidthProperty = DependencyProperty
        .Register(nameof(ShortestTaskWidth), typeof(double), 
            typeof(CyberneticSchedule), new PropertyMetadata((double)30));

    /// <summary>
    /// Width of shortest scheduled task.
    /// </summary>
    public double ShortestTaskWidth
    {
        get => (double)GetValue(ShortestTaskWidthProperty); 
        set => SetValue(ShortestTaskWidthProperty, value);
    }

    private static readonly DependencyProperty TaskHeightProperty = DependencyProperty
        .Register(nameof(TaskHeight), typeof(int), 
            typeof(CyberneticSchedule), new PropertyMetadata(22));

    /// <summary>
    /// Task element heights.
    /// </summary>
    public int TaskHeight
    {
        get => (int)GetValue(TaskHeightProperty); 
        set => SetValue(TaskHeightProperty, value);
    }
    
    private static readonly DependencyProperty ScheduleProperty = DependencyProperty
        .Register(nameof(Schedule), typeof(Schedule),
            typeof(CyberneticSchedule), new PropertyMetadata(OnScheduleChanged));

    private static void OnScheduleChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
    {
        var control = (CyberneticSchedule)obj;
        if (control != null)
        {
            control.DrawSchedule();
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
    
    private const int PresentLineZIndex = 100;
    private const int PresentLineUpdatePeriod = 1000;

    private ScheduleGrid? scheduleGrid;
    
    private Rectangle? presentLine;
    private Timer? presentLineUpdateTimer;

    private double scaleFactor = 1;
    
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
            scheduleGrid.Width = Width;                
        }

        if (presentLine != null)
        {
            presentLine.Height = ActualHeight;
        }
    }
    
    private void DrawSchedule()
    {
        Children.Clear();
        
        if (Schedule == null || Schedule.Duration == null)
        {
            return;
        }

        CalculateScaleFactor();
        CalculateWidth();
        
        AddRulerAndGrid();
        AddPresentLine();
        AddLayers();
    }

    private void CalculateScaleFactor()
    {
        scaleFactor = ShortestTaskWidth / Schedule.ShortestTaskDuration.Value.Ticks;
    }

    private void CalculateWidth()
    {
        Width = Schedule.Duration.Value.Ticks * scaleFactor;
    }

    private void AddRulerAndGrid()
    {
        var stepWidth = TimeSpan.FromHours(1).Ticks * scaleFactor;
        var numberOfStepsInLargeStep = 5;
        AddRuler(stepWidth, numberOfStepsInLargeStep);

        var columnWidth = stepWidth * numberOfStepsInLargeStep;
        AddGrid(columnWidth);
    }

    private void AddRuler(double stepWidth, int numberOfStepsInLargeStep)
    {
        var ruler = new Ruler
        {
            Width = Width,
            Height = RulerHeight,
            Background = (Brush)FindResource("RulerBackground"),
            MarkupBrush = (Brush)FindResource("RulerMarkupBrush"),
            Label = Schedule.StartTime.Value.ToString("dddd, dd MMMM yyyy"),
            StepWidth = stepWidth,
            NumberOfStepsInLargeStep = numberOfStepsInLargeStep,
        };
        
        Children.Add(ruler);
    }

    private void AddGrid(double columnWidth)
    {
        var yAxisOffset = RulerHeight + RibbonHeight;
        
        scheduleGrid = new ScheduleGrid
        {
            Width = Width,
            Height = ActualHeight - yAxisOffset,
            RowHeight = GridRowHeight,
            ColumnWidth = columnWidth,
            FirstRowBackground = (SolidColorBrush)new BrushConverter().ConvertFrom("#9ca3ad"),
            SecondRowBackground = (SolidColorBrush)new BrushConverter().ConvertFrom("#a8b2bf"),
            ColumnBackground = (SolidColorBrush)new BrushConverter().ConvertFrom("#898f96")
        };

        SetTop(scheduleGrid, yAxisOffset);
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
        }, null, 0, PresentLineUpdatePeriod);
    }

    private void UpdatePresentLinePosition()
    {
        if (presentLine == null)
        {
            return;
        }

        var timeOffset = DateTime.Now - Schedule.StartTime;
        var positionOffset = timeOffset.Value.Ticks * scaleFactor;
        SetLeft(presentLine, positionOffset);
    }
    
    private void AddLayers()
    {
        var layerIndex = 0;
        foreach (var layer in Schedule.Layers)
        {
            AddLayer(layer, layerIndex);
            layerIndex++;
        }
    }

    private void AddLayer(Layer layer, int layerIndex)
    {
        const int rowOffset = 2;
        
        var layerCanvas = new Canvas
        {
            Height = GridRowHeight,
            Width = ActualWidth
        };

        int taskIndex = 0;
        foreach (var task in layer.Tasks)
        {
            AddTask(task, layerCanvas);
            taskIndex++;
        }

        SetTop(layerCanvas, RulerHeight + RibbonHeight + GridRowHeight * layerIndex + rowOffset);
        Children.Add(layerCanvas);
    }

    private void AddTask(ScheduledTask task, Canvas layer)
    {
        var taskControl = new ScheduleTaskControl
        {
            Height = TaskHeight,
            Width = (task.EndTime - task.StartTime).Ticks * scaleFactor,
            DataContext = task
        };
            
        SetLeft(taskControl, (task.StartTime - Schedule.StartTime.Value).Ticks * scaleFactor);
        layer.Children.Add(taskControl);
    }
}