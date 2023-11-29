using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Cybernetic.Desktop.Views.Schedules;

/// <summary>
/// Schedule grid control.
/// </summary>
public class ScheduleGrid : Canvas
{
    private static readonly DependencyProperty EvenRowBrushProperty = DependencyProperty
        .Register(nameof(EvenRowBrush), typeof(Brush), typeof(ScheduleGrid));
    
    private static readonly DependencyProperty OddRowBrushProperty = DependencyProperty
        .Register(nameof(OddRowBrush), typeof(Brush), typeof(ScheduleGrid));
    
    private static readonly DependencyProperty ColumnSeparatorBrushProperty = DependencyProperty
        .Register(nameof(ColumnSeparatorBrush), typeof(Brush), typeof(ScheduleGrid));

    private static readonly DependencyProperty RowHeightProperty = DependencyProperty
        .Register(nameof(RowHeight), typeof(double), 
            typeof(ScheduleGrid), new PropertyMetadata((double)25));
    
    private static readonly DependencyProperty ColumnWidthProperty = DependencyProperty
        .Register(nameof(ColumnWidth), typeof(double), typeof(ScheduleGrid));
    
    /// <summary>
    /// Even row brush.
    /// </summary>
    public Brush EvenRowBrush
    {
        get => (Brush)GetValue(EvenRowBrushProperty);
        set => SetValue(EvenRowBrushProperty, value);
    }
    
    /// <summary>
    /// Odd row brush.
    /// </summary>
    public Brush OddRowBrush
    {
        get => (Brush)GetValue(OddRowBrushProperty);
        set => SetValue(OddRowBrushProperty, value);
    }
    
    /// <summary>
    /// Column separator brush.
    /// </summary>
    public Brush ColumnSeparatorBrush
    {
        get => (Brush)GetValue(ColumnSeparatorBrushProperty);
        set => SetValue(ColumnSeparatorBrushProperty, value);
    }
    
    /// <summary>
    /// Grid row height.
    /// </summary>
    public double RowHeight
    {
        get => (double)GetValue(RowHeightProperty);
        set => SetValue(RowHeightProperty, value);
    }
    
    /// <summary>
    /// Grid column width.
    /// </summary>
    public double ColumnWidth
    {
        get => (double)GetValue(ColumnWidthProperty);
        set => SetValue(ColumnWidthProperty, value);
    }
    
    /// <inheritdoc />
    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);
        
        DrawRows(drawingContext);
        DrawColumns(drawingContext);
    }

    private void DrawRows(DrawingContext context)
    {
        for (int i = 0; i < CalculateNumberOfRows(); i++)
        {
            var isEven = i % 2 == 0;
            var brush = isEven ? EvenRowBrush : OddRowBrush; 

            var row = new Rect
            {
                Height = RowHeight,
                Width = Width,
                Y = RowHeight * i
            };
            
            context.DrawRectangle(brush, null, row);
        }
    }

    private int CalculateNumberOfRows()
    {
        return (int)Math.Round(ActualHeight / RowHeight, 0, MidpointRounding.ToPositiveInfinity);
    }
    
    private void DrawColumns(DrawingContext context)
    {
        for (int i = 1; i <= CalculateNumberOfColumns(); i++)
        {
            var column = new Rect
            {
                Height = ActualHeight,
                Width = 1,
                X = ColumnWidth * i
            };
            
            context.DrawRectangle(ColumnSeparatorBrush, null, column);
        }
    }

    private int CalculateNumberOfColumns()
    {
        return (int)(Width / ColumnWidth);
    }
}