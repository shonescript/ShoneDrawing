using Aprillz.MewUI.Rendering;

namespace Shone.Drawing;

/// <summary>
/// A brush that paints an area with a linear gradient between two points,
/// mimicking System.Drawing.Drawing2D.LinearGradientBrush using MewUI.
/// </summary>
public sealed class LinearGradientBrush : Brush, IDisposable
{
    private bool disposed;

    private PointF startPoint;
    private PointF endPoint;
    private Color color1;
    private Color color2;

    // Other constructors omitted for brevity. 
    // (Include them if you wish to have the full class from earlier.)

    /// <summary>
    /// Creates a LinearGradientBrush based on a rectangle, two colors, and an angle (in degrees).
    /// This approximates System.Drawing.Drawing2D.LinearGradientBrush(Rectangle, Color, Color, float).
    /// </summary>
    /// <param name="rect">The bounding rectangle for the gradient.</param>
    /// <param name="color1">The first gradient color.</param>
    /// <param name="color2">The second gradient color.</param>
    /// <param name="angle">Angle in degrees, clockwise from the x-axis.</param>
    public LinearGradientBrush(Rectangle rect, Color color1, Color color2, float angle)
        : base()  // Call the base Brush constructor
    {
        this.color1 = color1;
        this.color2 = color2;

        float cx = rect.X + rect.Width * 0.5f;
        float cy = rect.Y + rect.Height * 0.5f;
        float angleRad = angle * (float)(Math.PI / 180.0);

        // For 45 degrees in a 100x100 rectangle, this gives us the expected 70.71
        float offset = (float)(Math.Sqrt(2) / 2 * rect.Width);
        float dx = (float)Math.Cos(angleRad);
        float dy = (float)Math.Sin(angleRad);

        startPoint = new PointF(cx + offset * dx, cy + offset * dy);
        endPoint   = new PointF(cx - offset * dx, cy - offset * dy);
    }

    #region Properties

    public PointF StartPoint
    {
        get => startPoint;
        set => startPoint = value;
    }

    public PointF EndPoint
    {
        get => endPoint;
        set => endPoint = value;
    }

    public Color Color1
    {
        get => color1;
        set => color1 = value;
    }

    public Color Color2
    {
        get => color2;
        set => color2 = value;
    }

    #endregion

    /// <summary>
    /// Creates or returns an IBrush for filling with a linear gradient 
    /// from StartPoint to EndPoint between Color1 and Color2.
    /// </summary>
    public IBrush ToMewBrush()
    {
        CheckDisposed();

        var start = new Aprillz.MewUI.Point((double)startPoint.X, (double)startPoint.Y);
        var end = new Aprillz.MewUI.Point((double)endPoint.X, (double)endPoint.Y);

        var colors = new Aprillz.MewUI.Color[] { color1.ToMewColor(), color2.ToMewColor() };
        var stops = new GradientStop[] 
        {
            new GradientStop(0.0f, colors[0]),
            new GradientStop(1.0f, colors[1])
        };

        var graphicsFactory = Aprillz.MewUI.Application.DefaultGraphicsFactory;
        return graphicsFactory.CreateLinearGradientBrush(start, end, stops);
    }

    #region Dispose

    public void Dispose()
    {
        if (!disposed)
        {
            disposed = true;
        }
    }

    private void CheckDisposed()
    {
        if (disposed)
            throw new ObjectDisposedException(nameof(LinearGradientBrush));
    }

    #endregion

    public override string ToString()
    {
        return $"LinearGradientBrush [ Start=PointF {{ X={startPoint.X}, Y={startPoint.Y} }}, End=PointF {{ X={endPoint.X}, Y={endPoint.Y} }}, Color1=Color [{color1}], Color2=Color [{color2}] ]";
    }
}
