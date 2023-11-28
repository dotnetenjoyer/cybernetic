using System.Windows.Media;

namespace Cybernetic.Desktop.Views.Schedules;

/// <summary>
/// Represents an object that can draw itself in a drawing context.
/// </summary>
public interface IDrawable
{
    /// <summary>
    /// Draw the object with using drawing context.
    /// </summary>
    /// <param name="context">Drawing context.</param>
    void Draw(DrawingContext context);
}