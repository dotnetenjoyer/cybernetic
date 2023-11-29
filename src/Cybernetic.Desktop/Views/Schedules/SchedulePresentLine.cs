using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Cybernetic.Desktop.Views.Schedules;

/// <summary>
/// Schedule present line.
/// </summary>
public class SchedulePresentLine : Canvas
{
    private const int UpdatePeriod = 5000;
    
    private static readonly DependencyProperty ScaleFactorProperty = DependencyProperty
        .Register(nameof(ScaleFactor), typeof(double), typeof(SchedulePresentLine));

    private static readonly DependencyProperty ScheduleStartTimeProperty = DependencyProperty
        .Register(nameof(ScheduleStartTime), typeof(DateTime?), typeof(SchedulePresentLine));
    
    private static readonly DependencyProperty LineBackgroundProperty = DependencyProperty
        .Register(nameof(LineBackground), typeof(Brush), typeof(SchedulePresentLine));

    private static readonly DependencyProperty LinePenProperty = DependencyProperty
        .Register(nameof(LinePen), typeof(Pen), typeof(SchedulePresentLine));
    
    private readonly Timer updateTimer;

    /// <summary>
    /// Scale factor.
    /// </summary>
    public double ScaleFactor
    {
        get => (double)GetValue(ScaleFactorProperty);
        set => SetValue(ScaleFactorProperty, value);
    }
    
    /// <summary>
    /// Schedule start time.
    /// </summary>
    public DateTime? ScheduleStartTime
    {
        get => (DateTime?)GetValue(ScheduleStartTimeProperty);
        set => SetValue(ScheduleStartTimeProperty, value);
    }
    
    /// <summary>
    /// Line background.
    /// </summary>
    public Brush LineBackground
    {
        get => (Brush)GetValue(LineBackgroundProperty);
        set => SetValue(LineBackgroundProperty, value);
    }
    
    /// <summary>
    /// Line pen.
    /// </summary>
    public Pen LinePen
    {
        get => (Pen)GetValue(LinePenProperty);
        set => SetValue(LinePenProperty, value);
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public SchedulePresentLine()
    {
        updateTimer = new Timer(state => Dispatcher.Invoke(InvalidateVisual), 
            null, UpdatePeriod, UpdatePeriod);
    }
    
    /// <inheritdoc />
    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);
        DrawPresentLine(drawingContext);
    }

    private void DrawPresentLine(DrawingContext drawingContext)
    {
        if (ScheduleStartTime == null)
        {
            return;
        }
        
        var line = new Rect
        {
            Width = 4,
            Height = ActualHeight,
            X = (DateTime.Now - ScheduleStartTime.Value).Ticks * ScaleFactor
        };

        drawingContext.DrawRectangle(LineBackground, LinePen, line); 
    }
}