#if SystemDrawing
namespace System.Drawing.Drawing2D;
#else
namespace Shone.Drawing.Drawing2D;
#endif
using Aprillz.MewUI.Rendering;

/// <summary>
/// A brush that paints an area with a linear gradient between two points,
/// mimicking System.Drawing.Drawing2D.LinearGradientBrush using MewUI.
/// </summary>
public class LinearGradientBrush : Brush, IDisposable
{
    private bool disposed;

    private PointF startPoint;
    private PointF endPoint;
    private Color color1;
    private Color color2;

    /// <summary>
    /// Creates a LinearGradientBrush based on a rectangle, two colors, and an angle (in degrees).
    /// This approximates System.Drawing.Drawing2D.LinearGradientBrush(Rectangle, Color, Color, float).
    /// </summary>
    /// <param name="rect">The bounding rectangle for the gradient.</param>
    /// <param name="color1">The first gradient color.</param>
    /// <param name="color2">The second gradient color.</param>
    /// <param name="angle">Angle in degrees, clockwise from the x-axis.</param>
    public LinearGradientBrush(RectangleF rect, Color color1, Color color2, float angle)
        : base()
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
        endPoint = new PointF(cx - offset * dx, cy - offset * dy);
    }

    public LinearGradientBrush(Rectangle rect, Color color1, Color color2, float angle) :
        this(new RectangleF(rect.Left, rect.Top, rect.Width, rect.Height), color1, color2, angle)
    {
    }

    public LinearGradientBrush(PointF point1, PointF point2, Color color1, Color color2) :
        this(RectangleF.FromLTRB(Math.Min(point1.X, point2.X), Math.Min(point1.Y, point2.Y), Math.Max(point1.X, point2.X), Math.Max(point1.Y, point2.Y)),
            color1, color2, point1.AngleTo(point2))
    {
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

    public ColorBlend InterpolationColors { get; set; }

    #endregion

    /// <summary>
    /// Creates or returns an IBrush for filling with a linear gradient 
    /// from StartPoint to EndPoint between Color1 and Color2.
    /// </summary>
    public IBrush ToMewBrush()
    {
        var start = new Aprillz.MewUI.Point((double)startPoint.X, (double)startPoint.Y);
        var end = new Aprillz.MewUI.Point((double)endPoint.X, (double)endPoint.Y);

        var colors = new Aprillz.MewUI.Color[] { color1.ToMewColor(), color2.ToMewColor() };
        var stops = new GradientStop[]
        {
            new GradientStop(0.0f, colors[0]),
            new GradientStop(1.0f, colors[1])
        };


        return Graphics.Factory.CreateLinearGradientBrush(start, end, stops);
    }

    #region Dispose

    public void Dispose()
    {
        if (!disposed)
        {
            disposed = true;
        }
    }
    #endregion

    public override string ToString()
    {
        return $"LinearGradientBrush [ Start=PointF {{ X={startPoint.X}, Y={startPoint.Y} }}, End=PointF {{ X={endPoint.X}, Y={endPoint.Y} }}, Color1=Color [{color1}], Color2=Color [{color2}] ]";
    }

    internal static Blend TriangularBlend(float focus, float scale)
    {
        Blend blend = new Blend();
        blend.Factors = new float[3] { 0f, scale, 0f };
        blend.Positions = new float[3] { 0f, focus, 1f };
        return blend;
    }

    private static PointF scale(PointF d, float v)
    {
        return new PointF(d.X * v, d.Y * v);
    }
    private static PointF add(PointF d, PointF v)
    {
        return new PointF(d.X + v.X, d.Y + v.Y);
    }
    static PointF calc(float d, PointF v, PointF c, PointF t, PointF n)
    {
        PointF pointF = scale(v, (float)Math.Pow(1f - d, 3.0));
        PointF pointF2 = scale(c, (float)(3.0 * Math.Pow(1f - d, 2.0) * (double)d));
        PointF pointF3 = scale(t, (float)((double)(3f * (1f - d)) * Math.Pow(d, 2.0)));
        return add(v: scale(n, (float)Math.Pow(d, 3.0)), d: add(add(pointF, pointF2), pointF3));
    }
    internal static Blend SigmaBell(float focus, float scale)
    {
        var blend = new Blend(511);
        int num = blend.Positions.Length / 2;
        float num2 = 0.37f;
        float num3 = focus;
        var pointF = new PointF(0f, 0f);
        var pointF2 = new PointF(focus, scale);
        var pointF3 = new PointF(pointF.X + num3 * num2, pointF.Y);
        var pointF4 = new PointF(pointF2.X - num3 * num2, pointF2.Y);
        for (int i = 0; i <= num; i++)
        {
            PointF pointF5 = calc((float)i / (float)num, pointF, pointF3, pointF4, pointF2);
            blend.Positions[i] = pointF5.X;
            blend.Factors[i] = pointF5.Y;
        }
        num3 = 1f - focus;
        pointF = new PointF(focus, scale);
        pointF2 = new PointF(1f, 0f);
        pointF3 = new PointF(pointF.X + num3 * num2, pointF.Y);
        pointF4 = new PointF(pointF2.X - num3 * num2, pointF2.Y);
        for (int j = 1; j <= num; j++)
        {
            PointF pointF6 = calc((float)j / (float)num, pointF, pointF3, pointF4, pointF2);
            blend.Positions[j + num] = pointF6.X;
            blend.Factors[j + num] = pointF6.Y;
        }
        return blend;
    }
}
