using Aprillz.MewUI.Rendering;

#if SystemDrawing
using System.Drawing.Drawing2D;
namespace System.Drawing;
#else
using Shone.Drawing.Drawing2D;
namespace Shone.Drawing;
#endif
/// <summary>
/// A minimal stand-in for System.Drawing.Pen, used for drawing lines, rectangles, etc.
/// </summary>
public class Pen: IDisposable
{
    public Color Color { get; set; }
    public float Width { get; set; }
    public LineCap StartCap { get; set; }
    public LineCap EndCap { get; set; }
    public DashCap DashCap { get; set; }
    public LineJoin LineJoin { get; set; }
    public DashStyle DashStyle { get; set; }
    public float DashOffset { get; set; }
    public float[] DashPattern { get; set; }
    public float[] CompoundArray { get; set; }

    public float MiterLimit { get; set; }

    public Pen(Color color, float width = 1.0f)
    {
        Color = color;
        Width = width;
    }

    public Pen(Brush brush, float width = 1.0f)
    {
        Color = brush.Color;
        Width = width;
    }

    /// <summary>
    /// Converts this Pen to MewUI IPen.
    /// </summary>
    public IPen ToMewPen()
    {
        
        return Graphics.Factory.CreatePen(Color.ToMewColor(), Width);
    }

    public void Dispose()
    {
    }
}