using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Cybernetic.Desktop.Views.Schedules;

/// <summary>
/// Schedule grid control.
/// </summary>
public class ScheduleGrid : Canvas
{   
    private static readonly DependencyProperty RowHeightProperty = DependencyProperty
        .Register(nameof(RowHeight), typeof(double), typeof(ScheduleGrid));

    /// <summary>
    /// Grid row height.
    /// </summary>
    public double RowHeight
    {
        get => (double)GetValue(RowHeightProperty); 
        set => SetValue(RowHeightProperty, value);
    }
    
    private static readonly DependencyProperty ColumnWidthProperty = DependencyProperty
        .Register(nameof(ColumnWidth), typeof(double), typeof(ScheduleGrid));

    /// <summary>
    /// Column width.
    /// </summary>
    public double ColumnWidth
    {
        get => (double)GetValue(ColumnWidthProperty); 
        set => SetValue(ColumnWidthProperty, value);
    }
    
    private static readonly DependencyProperty FirstRowBackgroundProperty = DependencyProperty
        .Register(nameof(FirstRowBackground), typeof(Brush), typeof(ScheduleGrid));

    /// <summary>
    /// First row background.
    /// </summary>
    public Brush FirstRowBackground
    {
        get => (Brush)GetValue(FirstRowBackgroundProperty); 
        set => SetValue(FirstRowBackgroundProperty, value);
    }
    
    private static readonly DependencyProperty SecondRowBackgroundProperty = DependencyProperty
        .Register(nameof(SecondRowBackground), typeof(Brush), typeof(ScheduleGrid));

    /// <summary>
    /// Second row background.
    /// </summary>
    public Brush SecondRowBackground
    {
        get => (Brush)GetValue(SecondRowBackgroundProperty); 
        set => SetValue(SecondRowBackgroundProperty, value);
    }
    
    private static readonly DependencyProperty ColumnBackgroundProperty = DependencyProperty
        .Register(nameof(ColumnBackground), typeof(Brush), typeof(ScheduleGrid));

    /// <summary>
    /// Column background.
    /// </summary>
    public Brush ColumnBackground
    {
        get => (Brush)GetValue(ColumnBackgroundProperty); 
        set => SetValue(ColumnBackgroundProperty, value);
    }
    
    /// <summary>
    /// Constructor.
    /// </summary>
    public ScheduleGrid()
    {
        Loaded += HandleLoaded;
    }

    private void HandleLoaded(object? sender, RoutedEventArgs args)
    {
        AddMarkup();
    }

    private void AddMarkup()
    {
        Children.Clear();

        AddRows();
        AddColumns();
    }

    private void AddRows()
    {
        var numberOfRows = (int)Math.Round(ActualHeight / RowHeight, 0, MidpointRounding.ToPositiveInfinity);

        for (int i = 0; i < numberOfRows; i++)
        {
            var row = new Rectangle
            {
                Height = RowHeight,
                Width = ActualWidth,
                Fill = i % 2 == 0 ? SecondRowBackground : FirstRowBackground
            };
            
            SetTop(row, i * RowHeight);

            Children.Add(row);
        }
    }

    private void AddColumns()
    {
        var numberOfColumns = (int)(ActualWidth / ColumnWidth);

        for (int i = 1; i <= numberOfColumns; i++)
        {
            var column = new Rectangle
            {
                Height = ActualHeight,
                Width = 1,
                Stroke = ColumnBackground
            };
            
            SetLeft(column, ColumnWidth * i);

            Children.Add(column);
        }
    }
}