using System.Windows.Media;

namespace Cybernetic.Desktop.Views.Schedules;

/// <summary>
/// Indicates object that can be drawn.
/// </summary>
public interface IDrawable
{
    /// <summary>
    /// Draw the object with using drawing context.
    /// </summary>
    /// <param name="drawingContext">Drawing context.</param>
    void Draw(DrawingContext drawingContext);
}