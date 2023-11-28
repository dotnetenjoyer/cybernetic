using System;
using System.Globalization;
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

    private static readonly DependencyProperty OneHourWidthProperty = DependencyProperty
        .Register(nameof(OneHourWidth), typeof(double), 
            typeof(CyberneticSchedule), new PropertyMetadata((double)40));

    /// <summary>
    /// Width of one hour on the schedule diagram.
    /// </summary>
    public double OneHourWidth
    {
        get => (double)GetValue(OneHourWidthProperty);
        set => SetValue(OneHourWidthProperty, value);
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
            control.InvalidateVisual();
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

    // private ScheduleGrid? scheduleGrid;
    
    private Rectangle? presentLine;
    private Timer? presentLineUpdateTimer;

    private double scaleFactor = 1;
    
    /// <summary>
    /// Constructor.
    /// </summary>
    public CyberneticSchedule()
    {
    }
    
    private void DrawSchedule()
    {
        Children.Clear();
        
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

    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);

        if (Schedule == null || Schedule.Duration == null) 
        {
            return;
        }

        // var stepWidth = TimeSpan.FromHours(1).Ticks * scaleFactor;
        var numberOfStepsInLargeStep = 5;
        var columnWidth = OneHourWidth * numberOfStepsInLargeStep;
        //
        // var ruler = new RulerBuilder()
        //     .Labeled(Schedule.StartTime.Value.ToString("dddd, dd MMMM yyyy"))
        //     .HasSize(ActualWidth, RulerHeight)
        //     .WithSteps(stepWidth, numberOfStepsInLargeStep)
        //     .WithMarkupBrush((Brush)FindResource("RulerMarkupBrush"))
        //     .Build();
        //
        // ruler.Draw(drawingContext);

        var grid = new ScheduleGridBuilder()
            .HasSize(Width, ActualHeight)
            .W(columnWidth, GridRowHeight)
            .HasRowBrushes((Brush)FindResource("GridEvenRowBackground"),
                (Brush)FindResource("GridOddRowBackground"))
            .HasColumnBrush((Brush)FindResource("GridColumnSeparatorBackground"))
            .Build();

        grid.Draw(drawingContext);

        var completedTaskBackground = (Brush)FindResource("CompletedTaskBackground");
        var errorTaskBackground = (Brush)FindResource("ErrorTaskBackground");
        var pendingTaskBackground = (Brush)FindResource("PendingTaskBackground");
        
        var layerIndex = 0;
        foreach (var layer in Schedule.Layers)
        {
            foreach (var task in layer.Tasks)
            {
                var rect = new Rect
                {
                    Width = (task.EndTime - task.StartTime).Ticks * scaleFactor,
                    Height = TaskHeight,
                    X = (task.StartTime - Schedule.StartTime.Value).Ticks * scaleFactor,
                    Y = GridRowHeight * layerIndex + 2
                };

                var brush = task.Status == TaskStatus.Completed
                    ? completedTaskBackground
                    : task.Status == TaskStatus.Error
                        ? errorTaskBackground
                        : pendingTaskBackground;
                
                var text = new FormattedText(
                    task.Name,
                    CultureInfo.GetCultureInfo("en-us"),
                    FlowDirection.LeftToRight,
                    new Typeface("Verdana"),
                    12,
                    Brushes.White);

                text.MaxTextWidth = rect.Width - 2;
                
                var textPosition = new Point(rect.X + 2, rect.Y + 2);

                var pen = new Pen(Brushes.Black, 1);
                drawingContext.DrawRoundedRectangle(brush, pen, rect, 2, 2);
                
                drawingContext.DrawText(text, textPosition);
            }
            

            layerIndex++;
        }
    }

    private void DrawRuler(DrawingContext context)
    {
        
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
}