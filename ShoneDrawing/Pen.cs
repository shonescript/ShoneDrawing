using Aprillz.MewUI.Rendering;

namespace Shone.Drawing;

/// <summary>
/// A minimal stand-in for System.Drawing.Pen, used for drawing lines, rectangles, etc.
/// </summary>
public class Pen: IDisposable
{
    public Color Color { get; set; }
    public float Width { get; set; }

    // You could add more properties: DashStyle, LineJoin, etc.

    public Pen(Color color, float width = 1.0f)
    {
        Color = color;
        Width = width;
    }

    /// <summary>
    /// Converts this Pen to MewUI IPen.
    /// </summary>
    public IPen ToMewPen()
    {
        var graphicsFactory = Aprillz.MewUI.Application.DefaultGraphicsFactory;
        return graphicsFactory.CreatePen(Color.ToMewColor(), Width);
    }

    public void Dispose()
    {
        // No unmanaged resources to release.
    }
}