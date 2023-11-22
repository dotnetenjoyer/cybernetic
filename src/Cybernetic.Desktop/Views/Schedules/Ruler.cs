using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Cybernetic.Desktop.Views.Schedules;

/// <summary>
/// Schedule ruler control.
/// </summary>
public class Ruler : Canvas
{
    private static readonly DependencyProperty MarkupBrushProperty = DependencyProperty
        .Register(nameof(MarkupBrush), typeof(Brush), typeof(Ruler), new PropertyMetadata(Brushes.Black));

    /// <summary>
    /// Ruler markup brush.
    /// </summary>
    public Brush MarkupBrush
    {
        get => (Brush)GetValue(MarkupBrushProperty);
        set => SetValue(MarkupBrushProperty, value);
    }
    
    private static readonly DependencyProperty StepWidthProperty = DependencyProperty
        .Register(nameof(StepWidth), typeof(double), typeof(Ruler));

    /// <summary>
    /// Ruler one step width.
    /// </summary>
    public double StepWidth
    {
        get => (double)GetValue(StepWidthProperty);
        set => SetValue(StepWidthProperty, value);
    }
    
    private static readonly DependencyProperty NumberOfStepsInLargeStepProperty = DependencyProperty
        .Register(nameof(NumberOfStepsInLargeStep), typeof(int), typeof(Ruler));

    /// <summary>
    /// Number of steps in large ruler step..
    /// </summary>
    public int NumberOfStepsInLargeStep
    {
        get => (int)GetValue(NumberOfStepsInLargeStepProperty);
        set => SetValue(NumberOfStepsInLargeStepProperty, value);
    }

    private static readonly DependencyProperty LabelProperty = DependencyProperty
        .Register(nameof(Label), typeof(string), typeof(Ruler));

    /// <summary>
    /// Ruler label.
    /// </summary>
    public string Label
    {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }
    
    /// <summary>
    /// Constructor.
    /// </summary>
    public Ruler()
    {
        Loaded += HandleLoaded;
    }

    private void HandleLoaded(object sender, RoutedEventArgs args)
    {
        AddRulerMarkup();
    }

    private void AddRulerMarkup()
    {
        Children.Clear();
        
        AddLabel();
        AddHashMarks();
        AddBorder();
    }

    private void AddLabel()
    {
        var label = new TextBlock
        {
            FontSize = 14,
            FontWeight = FontWeights.Bold,
            Text = Label,
            Foreground = MarkupBrush
        };
        
        SetTop(label, 5);
        SetLeft(label, 5);

        Children.Add(label);        
    }

    private void AddHashMarks()
    {
        for (int i = 1; i <= CalculateNumberOfSteps(); i++)
        {
            var isLargeHashMark = i % NumberOfStepsInLargeStep == 0;
           
            AddHashMark(i, isLargeHashMark);

            if (isLargeHashMark)
            {
                AddHashMarkLabel(i);
            }
        }
    }

    private int CalculateNumberOfSteps()
    {
        return (int)(ActualWidth / StepWidth);
    }

    private void AddHashMark(int hashMarkIndex, bool isLargeHashMark)
    {
        const int largeHashMarkHeight = 20;
        const int hashMarkHeight = 5;
        
        var hashMark = new Rectangle
        {
            Width = 1,
            Height = isLargeHashMark ? largeHashMarkHeight : hashMarkHeight,
            Stroke = MarkupBrush
        };

        SetLeft(hashMark, StepWidth * hashMarkIndex);
        SetTop(hashMark, Height - hashMark.Height);

        Children.Add(hashMark);
    }

    private void AddHashMarkLabel(int hashMarkIndex)
    {
        const int labelOffsetX = 5;
        const int labelOffsetY = -25;
        
        var label = new TextBlock
        {
            Text = hashMarkIndex.ToString(),
            FontSize = 14,
            Foreground = MarkupBrush
        };
                
        SetLeft(label, StepWidth * hashMarkIndex + labelOffsetX);
        SetTop(label, Height + labelOffsetY);
        Children.Add(label);             
    }

    private void AddBorder()
    {
        var border = new Rectangle
        {
            Height = 1,
            Width = ActualWidth,
            Stroke = MarkupBrush,
        };
        
        SetTop(border, ActualHeight - 1);
        
        Children.Add(border);
    }
}