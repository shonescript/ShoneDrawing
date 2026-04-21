#if SystemDrawing
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
#else
using Shone.Drawing.Drawing2D;
using Shone.Drawing.Imaging;
#endif
using Aprillz.MewUI.Rendering;

#if SystemDrawing
namespace System.Drawing;
#else
namespace Shone.Drawing;
#endif
public static class GraphicsExtensions
{
    public static void DrawImage(this Graphics g, Bitmap image, Point p)
    {
        g.DrawImage(image, (float)p.X, (float)p.Y);
    }

    public static void DrawImage(this Graphics g, Bitmap b, PointF p)
    {
        g.DrawImage(b, p.X, p.Y);
    }

    public static void DrawImage(this Graphics g, Bitmap b, Rectangle destRect, Rectangle srcRect, GraphicsUnit unit)
    {
        float dpiX = b.HorizontalResolution;
        float dpiY = b.VerticalResolution;

        var mewSrc = ConvertRect(srcRect, unit, dpiX, dpiY);
        var mewDest = ConvertRect(destRect, unit, dpiX, dpiY);

        var context = GetGraphicsContext(g);

        context.DrawImage(b.ToMewImage(), mewDest, mewSrc);
    }

    public static void DrawImage(this Graphics g, Bitmap b, Rectangle destRect)
    {
        Rectangle srcRect = new Rectangle(0, 0, b.Width, b.Height);

        g.DrawImage(b, destRect, srcRect, GraphicsUnit.Pixel);
    }

    public static void DrawImage(this Graphics g, Bitmap b, int x, int y, int width, int height)
    {
        Rectangle destRect = new Rectangle(x, y, width, height);

        g.DrawImage(b, destRect);
    }

    public static void DrawImage(
        this Graphics g,
        Bitmap b,
        Rectangle destRect,
        int srcX,
        int srcY,
        int srcWidth,
        int srcHeight,
        GraphicsUnit unit,
        ImageAttributes attributes)
    {
        var srcRect = new Rectangle(srcX, srcY, srcWidth, srcHeight);

        float dpiX = b.HorizontalResolution;
        float dpiY = b.VerticalResolution;

        var mewSrc = ConvertRect(srcRect, unit, dpiX, dpiY);
        var mewDest = ConvertRect(destRect, unit, dpiX, dpiY);

        var context = GetGraphicsContext(g);

        if (attributes != null)
        {
            var colorFilterArray = attributes.GetColorFilterArray();
            if (colorFilterArray != null)
            {
            }
        }

        context.DrawImage(b.ToMewImage(), mewDest, mewSrc);
    }

    private static Aprillz.MewUI.Rect ConvertRect(Rectangle r, GraphicsUnit unit, float dpiX, float dpiY)
    {
        float x = r.X;
        float y = r.Y;
        float w = r.Width;
        float h = r.Height;

        switch (unit)
        {
            case GraphicsUnit.Pixel:
            case GraphicsUnit.Display:
                break;

            case GraphicsUnit.Inch:
                x *= dpiX;
                y *= dpiY;
                w *= dpiX;
                h *= dpiY;
                break;

            case GraphicsUnit.Millimeter:
                float scaleX_mm = dpiX / 25.4f;
                float scaleY_mm = dpiY / 25.4f;
                x *= scaleX_mm;
                y *= scaleY_mm;
                w *= scaleX_mm;
                h *= scaleY_mm;
                break;

            case GraphicsUnit.Point:
                float scaleX_pt = dpiX / 72f;
                float scaleY_pt = dpiY / 72f;
                x *= scaleX_pt;
                y *= scaleY_pt;
                w *= scaleX_pt;
                h *= scaleY_pt;
                break;

            case GraphicsUnit.Document:
                float scaleX_doc = dpiX / 300f;
                float scaleY_doc = dpiY / 300f;
                x *= scaleX_doc;
                y *= scaleY_doc;
                w *= scaleX_doc;
                h *= scaleY_doc;
                break;

            default:
                break;
        }

        return new Aprillz.MewUI.Rect(x, y, w, h);
    }

    private static IGraphicsContext GetGraphicsContext(Graphics g)
    {
        var contextField = typeof(Graphics).GetField("graphicsContext", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (contextField == null)
            throw new Exception("Could not find internal field 'graphicsContext' in Graphics.");
        var contextObj = contextField.GetValue(g);
        if (contextObj == null)
            throw new Exception("The internal 'graphicsContext' is null in Graphics.");
        return (IGraphicsContext)contextObj;
    }

    public static void FillRectangle(this Graphics g, LinearGradientBrush b, Rectangle r)
    {
        var context = GetGraphicsContext(g);

        var rect = new Aprillz.MewUI.Rect(r.X, r.Y, r.Width, r.Height);

        context.FillRectangle(rect, b.ToMewBrush());
    }

    public static void FillRectangle(this Graphics g, SolidBrush b, Rectangle r)
    {
        var context = GetGraphicsContext(g);

        var rect = new Aprillz.MewUI.Rect(r.X, r.Y, r.Width, r.Height);

        context.FillRectangle(rect, b.ToMewBrush());
    }

    public static void CopyFromScreen(
        this Graphics g,
        int x,
        int y,
        int zero1,
        int zero2,
        Size size,
        CopyPixelOperation copyPixelOp
    )
    {
        if (copyPixelOp != CopyPixelOperation.SourceCopy)
        {
            throw new NotImplementedException("Only SourceCopy is supported in this example.");
        }

        throw new NotImplementedException(
            "CopyFromScreen is not implemented cross-platform. " +
            "Use platform-specific APIs to capture the screen."
        );
    }

    public static void DrawLines(this Graphics g, Pen p, Point[] points)
    {
        if (points.Length < 2)
            return;

        for (int i = 0; i < points.Length - 1; i++)
        {
            float x1 = points[i].X;
            float y1 = points[i].Y;
            float x2 = points[i + 1].X;
            float y2 = points[i + 1].Y;

            g.DrawLine(p, x1, y1, x2, y2);
        }
    }

    public static void DrawString(this Graphics g, string text, Font font, SolidBrush brush, PointF point, StringFormat format)
    {

        var context = GetGraphicsContext(g);

        var bounds = new Aprillz.MewUI.Rect(point.X, point.Y, float.MaxValue, font.Size * 1.5f);

        context.DrawText(text.AsSpan(), bounds, font.ToMewFont(), brush.Color.ToMewColor());
    }

    public static void DrawRectangle(this Graphics g, Pen p, Rectangle r)
    {

        var context = GetGraphicsContext(g);

        var rect = new Aprillz.MewUI.Rect(r.X, r.Y, r.Width, r.Height);

        context.DrawRectangle(rect, p.Color.ToMewColor(), p.Width);
    }

    public static void FillRectangle(this Graphics g, SolidBrush b, RectangleF r)
    {

        var context = GetGraphicsContext(g);

        var rect = new Aprillz.MewUI.Rect(r.X, r.Y, r.Width, r.Height);

        context.FillRectangle(rect, b.ToMewBrush());
    }

}