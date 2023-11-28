using System;
using System.Windows;
using System.Windows.Media;

namespace Cybernetic.Desktop.Views.Schedules;

/// <summary>
/// The class that draws schedule grid.
/// </summary>
public class ScheduleGrid : IDrawable
{
    private readonly double height;
    private readonly double width;
    
    private readonly double rowHeight;
    private readonly double columnWidth;

    private readonly Brush oddRowBrush;
    private readonly Brush evenRowBrush;
    private readonly Brush columnBrush;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="width">Grid width.</param>
    /// <param name="height">Grid height.</param>
    /// <param name="columnWidth">Grid column width.</param>
    /// <param name="rowHeight">Grid row height.</param>
    /// <param name="evenRowBrush">Brush of event rows.</param>
    /// <param name="oddRowBrush">Brush of odd rows.</param>
    /// <param name="columnBrush">Brush of the column separator.</param>
    public ScheduleGrid(double width, double height, double columnWidth, double rowHeight, 
        Brush evenRowBrush, Brush oddRowBrush, Brush columnBrush)
    {
        this.height = height;
        this.width = width;
        
        this.rowHeight = rowHeight;
        this.columnWidth = columnWidth;

        this.oddRowBrush = oddRowBrush;
        this.evenRowBrush = evenRowBrush;
        this.columnBrush = columnBrush;
    }
    
    /// <inheritdoc />
    public void Draw(DrawingContext context)
    {
        DrawRows(context);
        DrawColumns(context);
    }
    
    private void DrawRows(DrawingContext context)
    {
        for (int i = 0; i < CalculateNumberOfRows(); i++)
        {
            var isEven = i % 2 == 0;
            var brush = isEven ? evenRowBrush : oddRowBrush; 

            var row = new Rect
            {
                Height = rowHeight,
                Width = width,
                Y = rowHeight * i
            };
            
            context.DrawRectangle(brush, null, row);
        }
    }

    private int CalculateNumberOfRows()
    {
        return (int)Math.Round(height / rowHeight, 0, MidpointRounding.ToPositiveInfinity);
    }
    
    private void DrawColumns(DrawingContext context)
    {
        for (int i = 1; i <= CalculateNumberOfColumns(); i++)
        {
            var column = new Rect
            {
                Height = height,
                Width = 1,
                X = columnWidth * i
            };
            
            context.DrawRectangle(columnBrush, null, column);
        }
    }

    private int CalculateNumberOfColumns()
    {
        return (int)(width / columnWidth);
    }
}

public class ScheduleGridBuilder
{
    private double width;
    private double height;

    private double columnWidth;
    private double rowHeight;
    
    private Brush oddRowBrush;
    private Brush evenRowBrush;
    private Brush columnBrush;
    
    /// <summary>
    /// Configure schedule grid size.
    /// </summary>
    /// <param name="width">Width.</param>
    /// <param name="height">Height.</param>
    public ScheduleGridBuilder HasSize(double width, double height)
    {
        if (width <= 0)
        {
            throw new ArgumentException("The width of the ruler must be greater than zero");
        }

        if (height <= 0)
        {
            throw new ArgumentException("The height of the ruler must be greater than zero");
        }
        
        this.width = width;
        this.height = height;
        
        return this;
    }

    public ScheduleGridBuilder W(double columnWidth, double rowHeight)
    {
        this.columnWidth = columnWidth;
        this.rowHeight = rowHeight;

        return this;
    }
    
    public ScheduleGridBuilder HasRowBrushes(Brush evenRowBrush, Brush oddRowBrush)
    {
        this.evenRowBrush = evenRowBrush;
        this.oddRowBrush = oddRowBrush;

        return this;
    }

    public ScheduleGridBuilder HasColumnBrush(Brush columnBrush)
    {
        this.columnBrush = columnBrush;
        
        return this;
    }
    
    public ScheduleGrid Build()
    {
        return new ScheduleGrid(width, height, columnWidth, rowHeight, 
            evenRowBrush, oddRowBrush, columnBrush);
    }
}