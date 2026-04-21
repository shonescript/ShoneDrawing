using Aprillz.MewUI.Rendering;

namespace ShoneDrawing;

public class Graphics : IDisposable
{
    private IGraphicsContext graphicsContext;
    private bool disposed;

    // Example InterpolationMode property and other members...
    private InterpolationMode interpolationMode = InterpolationMode.Default;
    public InterpolationMode InterpolationMode
    {
        get => interpolationMode;
        set => interpolationMode = value;
    }

    private Graphics(IGraphicsContext context)
    {
        graphicsContext = context;
    }

    public static Graphics FromImage(Bitmap bitmap)
    {
        var renderTarget = bitmap.ToRenderTarget();
        var graphicsFactory = Aprillz.MewUI.Application.DefaultGraphicsFactory;
        var context = graphicsFactory.CreateContext(renderTarget);
        return new Graphics(context);
    }

    #region Clear and Drawing Methods

    public void Clear(Color c)
    {
        graphicsContext.Clear(c.ToMewColor());
    }

    public void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
    {
        var start = new Aprillz.MewUI.Point(x1, y1);
        var end = new Aprillz.MewUI.Point(x2, y2);
        graphicsContext.DrawLine(start, end, pen.Color.ToMewColor(), pen.Width);
    }

    public void DrawRectangle(Pen pen, float x, float y, float width, float height)
    {
        var rect = new Aprillz.MewUI.Rect(x, y, width, height);
        graphicsContext.DrawRectangle(rect, pen.Color.ToMewColor(), pen.Width);
    }

    public void FillRectangle(Brush brush, float x, float y, float width, float height)
    {
        var rect = new Aprillz.MewUI.Rect(x, y, width, height);
        graphicsContext.FillRectangle(rect, brush.ToMewBrush());
    }

    public void DrawEllipse(Pen pen, float x, float y, float width, float height)
    {
        var bounds = new Aprillz.MewUI.Rect(x, y, width, height);
        graphicsContext.DrawEllipse(bounds, pen.Color.ToMewColor(), pen.Width);
    }

    public void FillEllipse(Brush brush, float x, float y, float width, float height)
    {
        var bounds = new Aprillz.MewUI.Rect(x, y, width, height);
        graphicsContext.FillEllipse(bounds, brush.ToMewBrush());
    }

    public void DrawImage(Bitmap image, float x, float y)
    {
        var location = new Aprillz.MewUI.Point(x, y);
        graphicsContext.DrawImage(image.ToMewImage(), location);
    }

    /// <summary>
    /// Draws text with a simple color and textSize.
    /// </summary>
    public void DrawString(string text, float x, float y, Color color, float textSize = 16)
    {
        var bounds = new Aprillz.MewUI.Rect(x, y, float.MaxValue, textSize * 1.5f);
        var graphicsFactory = Aprillz.MewUI.Application.DefaultGraphicsFactory;
        var font = graphicsFactory.CreateFont("Arial", textSize);
        graphicsContext.DrawText(text.AsSpan(), bounds, font, color.ToMewColor());
    }

    /// <summary>
    /// Draws text with a Font and Brush, at the given location.
    /// </summary>
    public void DrawString(string text, Font font, Brush brush, float x, float y)
    {
        var bounds = new Aprillz.MewUI.Rect(x, y, float.MaxValue, font.Size * 1.5f);
        graphicsContext.DrawText(text.AsSpan(), bounds, font.ToMewFont(), brush.Color.ToMewColor());
    }

    #endregion

    #region MeasureString

    /// <summary>
    /// Measures the width and height of the given string using the specified Font.
    /// Returns a SizeF representing the bounding box (in local drawing units).
    /// </summary>
    public SizeF MeasureString(string numerics, Font f)
    {
        var size = graphicsContext.MeasureText(numerics.AsSpan(), f.ToMewFont());
        return new SizeF((float)size.Width, (float)size.Height);
    }

    #endregion

    #region Transforms

    public void TranslateTransform(float dx, float dy)
    {
        graphicsContext.Translate(dx, dy);
    }

    public void ScaleTransform(float sx, float sy)
    {
        graphicsContext.Scale(sx, sy);
    }

    public void RotateTransform(float degrees)
    {
        graphicsContext.Rotate(degrees * Math.PI / 180);
    }

    public void ResetTransform()
    {
        graphicsContext.ResetTransform();
    }

    #endregion

    #region VisibleClipBounds

    public RectangleF VisibleClipBounds
    {
        get
        {
            // MewUI doesn't expose device clip bounds directly, return a default value
            return new RectangleF(0, 0, 10000, 10000);
        }
    }

    #endregion

    #region Disposal

    public void Dispose()
    {
        if (!disposed)
        {
            graphicsContext.Dispose();
            graphicsContext = null;
            disposed = true;
        }
    }

    #endregion

    public override string ToString()
    {
        return $"Graphics [VisibleClipBounds={VisibleClipBounds}]";
    }
}
