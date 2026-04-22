#if SystemDrawing
namespace System.Drawing.Drawing2D;
#else
using System.Runtime.CompilerServices;

namespace Shone.Drawing.Drawing2D;
#endif

public class PathGradientBrush : Brush
{
    private Matrix matrix = new Matrix();

    private Color color;

    private Color[] colors;

    private PointF point1;

    private Blend n;

    private ColorBlend colorBlend;

    private PointF point2;

    private WrapMode mode;

    private readonly GraphicsPath path;

    public Color CenterColor
    {

        get
        {
            return color;
        }

        set
        {
            color = value;
        }
    }

    public Color[] SurroundColors
    {

        get
        {
            return colors;
        }

        set
        {
            colors = value;
        }
    }

    public PointF CenterPoint
    {

        get
        {
            return point1;
        }

        set
        {
            point1 = value;
        }
    }

    public RectangleF Rectangle => path.GetBounds();

    public Blend Blend
    {

        get
        {
            return n;
        }

        set
        {
            n = value;
        }
    }

    public ColorBlend InterpolationColors
    {

        get
        {
            return colorBlend;
        }

        set
        {
            colorBlend = value;
        }
    }

    public Matrix Transform
    {
        get
        {
            return this.matrix.Clone();
        }
        set
        {
            this.matrix.Dispose();
            this.matrix = value.Clone();
        }
    }

    public PointF FocusScales
    {

        get
        {
            return point2;
        }

        set
        {
            point2 = value;
        }
    }

    public WrapMode WrapMode
    {

        get
        {
            return mode;
        }

        set
        {
            mode = value;
        }
    }

    public PathGradientBrush(PointF[] points)
        : this(points, WrapMode.Clamp)
    {
    }

    public PathGradientBrush(PointF[] points, WrapMode wrapMode)
        : this(d(points))
    {
        WrapMode = wrapMode;
    }

    public PathGradientBrush(Point[] points)
        : this(points, WrapMode.Clamp)
    {
    }

    public PathGradientBrush(Point[] points, WrapMode wrapMode)
        : this(points.Select(p => new PointF(p.X, p.Y)).ToArray(), wrapMode)
    {
    }

    public PathGradientBrush(GraphicsPath path)
    {
        RectangleF bounds = path.GetBounds();
        if (bounds.Height == 0f || bounds.Width == 0f)
        {
            throw new OutOfMemoryException();
        }
        this.path = (GraphicsPath)path.Clone();
        WrapMode = WrapMode.Clamp;
        Initialize();
    }

    private PathGradientBrush(PathGradientBrush d)
    {
        this.matrix = d.matrix?.Clone();
        this.path = (GraphicsPath)d.path.Clone();
        CenterColor = d.CenterColor;
        SurroundColors = (Color[])d.SurroundColors?.Clone();
        CenterPoint = d.CenterPoint;
        if (d.Blend != null)
        {
            Blend = new Blend
            {
                Factors = (float[])d.Blend.Factors?.Clone(),
                Positions = (float[])d.Blend.Positions?.Clone()
            };
        }
        if (d.InterpolationColors != null)
        {
            InterpolationColors = new ColorBlend
            {
                Colors = (Color[])d.InterpolationColors.Colors?.Clone(),
                Positions = (float[])d.InterpolationColors.Positions?.Clone()
            };
        }
        FocusScales = d.FocusScales;
        WrapMode = d.WrapMode;
    }


    private void Initialize()
    {
        if (path.PointCount != 0)
        {
            float num = 0f;
            float num2 = 0f;
            PointF[] pathPoints = path.PathPoints;
            for (int i = 0; i < pathPoints.Length; i++)
            {
                PointF pointF = pathPoints[i];
                num += pointF.X;
                num2 += pointF.Y;
            }
            CenterPoint = new PointF(num / (float)path.PointCount, num2 / (float)path.PointCount);
        }
    }

    public void SetSigmaBellShape(float focus)
    {
        SetSigmaBellShape(focus, 1f);
    }

    public void SetSigmaBellShape(float focus, float scale)
    {
        Blend = LinearGradientBrush.SigmaBell(focus, scale);
    }

    public void SetBlendTriangularShape(float focus)
    {
        SetBlendTriangularShape(focus, 1f);
    }

    public void SetBlendTriangularShape(float focus, float scale)
    {
        Blend = LinearGradientBrush.TriangularBlend(focus, scale);
    }

    public void ResetTransform()
    {
        this.matrix.Reset();
    }

    public void MultiplyTransform(Matrix matrix)
    {
        MultiplyTransform(matrix, MatrixOrder.Prepend);
    }

    public void MultiplyTransform(Matrix matrix, MatrixOrder order)
    {
        this.matrix.Multiply(matrix, order);
    }

    public void TranslateTransform(float dx, float dy)
    {
        TranslateTransform(dx, dy, MatrixOrder.Prepend);
    }

    public void TranslateTransform(float dx, float dy, MatrixOrder order)
    {
        this.matrix.Translate(dx, dy, order);
    }

    public void ScaleTransform(float sx, float sy)
    {
        ScaleTransform(sx, sy, MatrixOrder.Prepend);
    }

    public void ScaleTransform(float sx, float sy, MatrixOrder order)
    {
        this.matrix.Scale(sx, sy, order);
    }

    public void RotateTransform(float angle)
    {
        RotateTransform(angle, MatrixOrder.Prepend);
    }

    public void RotateTransform(float angle, MatrixOrder order)
    {
        this.matrix.Rotate(angle, order);
    }

    private static GraphicsPath d(PointF[] d)
    {
        GraphicsPath graphicsPath = new GraphicsPath();
        graphicsPath.AddLines(d);
        return graphicsPath;
    }
}