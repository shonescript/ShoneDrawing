#if SystemDrawing
namespace System.Drawing;
#else
namespace Shone.Drawing;
#endif
public static class RectangleExtensions
{
    public static RectangleF ToRectangleF(this Rectangle rect)
    {
        return new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
    }
}