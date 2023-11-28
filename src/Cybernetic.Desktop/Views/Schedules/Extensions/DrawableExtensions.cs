using System.Windows.Media;

namespace Cybernetic.Desktop.Views.Schedules.Extensions;

public static class DrawableExtensions
{
    /// <summary>
    /// Draw object with transformation.
    /// </summary>
    /// <param name="drawable">Object to draw.</param>
    /// <param name="drawingContext">Drawing context.</param>
    /// <param name="transform">Transformation.</param>
    public static void Draw(this IDrawable drawable, DrawingContext drawingContext, Transform transform)
    {
        drawingContext.PushTransform(transform);
        drawable.Draw(drawingContext);
        // drawingContext.PushTransform(transform.Inverse.?);
    }
}