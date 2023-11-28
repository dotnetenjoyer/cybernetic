using System;
using System.Windows;
using System.Windows.Media;

namespace Cybernetic.Desktop.Views.Schedules;

public class SGrid : IDrawable
{
    private readonly double height;
    private readonly double width;
    
    private readonly double rowHeight;
    private readonly double columnWidth;

    private readonly Brush firstRowBrush;
    private readonly Brush secondRowBrush;

    private readonly Brush columnBrush;

    public SGrid(double height, double width, double rowHeight, double columnWidth,
        Brush firstRowBrush, Brush secondRowBrush, Brush columnBrush)
    {
        this.height = height;
        this.width = width;
        
        this.rowHeight = rowHeight;
        this.columnWidth = columnWidth;

        this.firstRowBrush = firstRowBrush;
        this.secondRowBrush = secondRowBrush;
        this.columnBrush = columnBrush;
    }
    
    public void Draw(DrawingContext context)
    {
        DrawRows(context);
        DrawColumns(context);
    }
    
    private void DrawRows(DrawingContext context)
    {
        for (int i = 0; i < CalculateNumberOfRows(); i++)
        {
            var brush = i % 2 == 0 ? secondRowBrush : firstRowBrush; 

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