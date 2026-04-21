using Aprillz.MewUI.Rendering;

#if SystemDrawing
namespace System.Drawing;
#else
namespace Shone.Drawing;
#endif
/// <summary>
/// A minimal stand-in for System.Drawing.Brush, used for filling shapes.
/// </summary>
public class Brush
{
    public Color Color { get; set; }

    public Brush()
    {
        Color = Color.Black;
    }
    
    public Brush(Color color)
    {
        Color = color;
    }

    /// <summary>
    /// Converts this Brush to MewUI IBrush.
    /// </summary>
    public ISolidColorBrush ToMewBrush()
    {
        
        return Graphics.Factory.CreateSolidColorBrush(Color.ToMewColor());
    }
}