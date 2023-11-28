using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Cybernetic.Desktop.Views.Schedules;

public class RulerControl : Canvas
{
    private const int LargeHashMarkHeight = 20;
    private const int HashMarkHeight = 5;
    private const int HashMarkWidth = 1;
    
    private static readonly DependencyProperty LabelProperty = DependencyProperty
        .Register(nameof(Label), typeof(string), typeof(RulerControl));

    private static readonly DependencyProperty MarkupBrushProperty = DependencyProperty
        .Register(nameof(MarkupBrush), typeof(Brush), 
            typeof(RulerControl), new PropertyMetadata(Brushes.Black));
    
    private static readonly DependencyProperty StepWidthProperty = DependencyProperty
        .Register(nameof(StepWidth), typeof(double), typeof(RulerControl), new PropertyMetadata(Handle));

    private static void Handle(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
        var a = (RulerControl)obj;
        a.InvalidateVisual();
    }
    
    private static readonly DependencyProperty NumberOfStepsInLargeStepProperty = DependencyProperty
        .Register(nameof(NumberOfStepsInLargeStep), typeof(int),
            typeof(RulerControl), new PropertyMetadata(5));
    
    /// <summary>
    /// Ruler label.
    /// </summary>
    public string Label
    {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    /// <summary>
    /// Ruler markup brush.
    /// </summary>
    public Brush MarkupBrush
    {
        get => (Brush)GetValue(MarkupBrushProperty);
        set => SetValue(MarkupBrushProperty, value);
    }
    
    /// <summary>
    /// Width of one ruler step.
    /// </summary>
    public double StepWidth
    {
        get => (double)GetValue(StepWidthProperty);
        set => SetValue(StepWidthProperty, value);
    }
    
    /// <summary>
    /// Number of steps in ruler large step.
    /// </summary>
    public int NumberOfStepsInLargeStep
    {
        get => (int)GetValue(NumberOfStepsInLargeStepProperty);
        set => SetValue(NumberOfStepsInLargeStepProperty, value);
    }
    
    /// <inheritdoc />
    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);
        
        DrawLabel(drawingContext);
        DrawHashMarks(drawingContext);
    }
    
    private void DrawLabel(DrawingContext drawingContext)
    {
        if (string.IsNullOrEmpty(Label))
        {
            return;
        }
        
        var labelPosition = new Point(5, 5);
        var labelText = CreateMarkupText(Label, 14);
        labelText.SetFontWeight(FontWeights.DemiBold);
        
        drawingContext.DrawText(labelText, labelPosition);
    }
    
    private void DrawHashMarks(DrawingContext drawingContext)
    {
        for (int i = 1; i <= CalculateNumberOfSteps(); i++)
        {
            var isLargeHashMark = i % NumberOfStepsInLargeStep == 0;
           
            DrawHashMark(drawingContext, i, isLargeHashMark);
            
            if (isLargeHashMark)
            {
                DrawHashMarkLabel(drawingContext, i);
            }
        }
    }
    
    private int CalculateNumberOfSteps()
    {
        return (int)(Width / StepWidth);
    }
    
    private void DrawHashMark(DrawingContext drawingContext, int hashMarkIndex, bool isLargeHashMark)
    {
        var hashMarkHeight = isLargeHashMark ? LargeHashMarkHeight : HashMarkHeight;
        var hashMark = new Rect
        {
            Width = HashMarkWidth,
            Height = hashMarkHeight,
            X = StepWidth * hashMarkIndex,
            Y = ActualHeight - hashMarkHeight
        };

        drawingContext.DrawRectangle(MarkupBrush, null, hashMark);
    }

    private void DrawHashMarkLabel(DrawingContext drawingConte, int hashMarkIndex)
    {
        const int labelOffsetX = 5;
        
        var label = CreateMarkupText(hashMarkIndex.ToString(), 12);
        var labelPosition = new Point(StepWidth * hashMarkIndex + labelOffsetX, ActualHeight - LargeHashMarkHeight);
        
        drawingConte.DrawText(label, labelPosition);
    }

    private FormattedText CreateMarkupText(string text, int size)
    {
        return new FormattedText(text, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight,
            new Typeface("Verdana"), size, MarkupBrush);
    }
}