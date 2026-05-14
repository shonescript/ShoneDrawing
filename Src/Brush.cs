using Aprillz.MewUI.Rendering;

#if SystemDrawing
namespace System.Drawing;
#else
namespace Shone.Drawing;
#endif
/// <summary>
/// A minimal stand-in for System.Drawing.Brush, used for filling shapes.
/// </summary>
public class Brush : IDisposable
{
    public Color Color { get; set; }

    /// <summary>
    /// Converts this Brush to MewUI IBrush.
    /// </summary>
    public ISolidColorBrush MewBrush =>
        Graphics.Factory.CreateSolidColorBrush(Color.MewColor);

    public Brush()
    {
        Color = Color.Black;
    }
    
    public Brush(Color color)
    {
        Color = color;
    }

    public void Dispose()
    {
    }
}