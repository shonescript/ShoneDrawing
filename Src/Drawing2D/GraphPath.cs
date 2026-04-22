#if SystemDrawing
namespace System.Drawing.Drawing2D;
#else
namespace Shone.Drawing.Drawing2D;
#endif

public enum FillMode
{
    Alternate,
    Winding
}

public enum PathPointType
{
    Start = 0,
    Line = 1,
    Bezier = 3,
    PathTypeMask = 7,
    DashMode = 16,
    PathMarker = 32,
    CloseSubpath = 128,
    Bezier3 = 3
}

public class GraphicsPath : IDisposable, ICloneable
{
    private readonly List<PointF> points = new();
    private readonly List<byte> types = new();
    private FillMode fillMode;
    private bool isMarker;

    public PointF[] Points => points.ToArray();

    public byte[] Types => types.ToArray();

    public FillMode FillMode
    {
        get => fillMode;
        set => fillMode = value;
    }

    public bool PathHasCurve => types.Any(t => (t & 0x07) == 3);

    public PointF? PathStartPoint => points.Count > 0 ? points[0] : null;

    public int PointCount => points.Count;

    public GraphicsPath()
        : this(FillMode.Alternate)
    {
    }

    public GraphicsPath(FillMode fillMode)
    {
        this.fillMode = fillMode;
    }

    public GraphicsPath(PointF[] pts, byte[]? types, FillMode fillMode = FillMode.Alternate)
    {
        if (pts is null)
            throw new ArgumentNullException(nameof(pts));

        this.fillMode = fillMode;
        SetPath(pts, types);
    }

    public object Clone()
    {
        var path = new GraphicsPath(fillMode);
        path.points.AddRange(points);
        path.types.AddRange(types);
        path.isMarker = isMarker;
        return path;
    }

    public void Dispose()
    {
    }

    public void Reset()
    {
        points.Clear();
        types.Clear();
        isMarker = false;
    }

    public void StartFigure()
    {
        if (points.Count == 0)
            return;

        var lastType = types[^1];
        if ((lastType & 0x80) == 0)
            types[^1] = (byte)(lastType | 0x80);
    }

    public void CloseFigure()
    {
        if (points.Count == 0)
            return;

        int start = GetFigureStartIndex(points.Count - 1);
        if (start < 0)
            return;

        if (points.Count > start)
        {
            var first = points[start];
            var last = points[^1];
            if (first != last)
            {
                points.Add(first);
                types.Add(1);
            }
        }

        types[^1] = (byte)(types[^1] | 0x80);
    }

    public void CloseAllFigures()
    {
        if (points.Count == 0)
            return;

        int i = 0;
        while (i < points.Count)
        {
            int start = i;
            while (i < points.Count && (types[i] & 0x80) == 0)
                i++;

            if (i < points.Count)
            {
                if (points[start] != points[i])
                {
                    points.Add(points[start]);
                    types.Add(1);
                }

                types[i] = (byte)(types[i] | 0x80);
                i++;
            }
        }
    }

    public void SetMarker()
    {
        isMarker = true;
    }

    public void ClearMarkers()
    {
        isMarker = false;
    }

    public int GetLastMarker(int fromIndex)
    {
        if (fromIndex < 0 || fromIndex >= points.Count)
            return -1;

        return isMarker ? fromIndex : -1;
    }

    public void Reverse()
    {
        points.Reverse();
        types.Reverse();
    }

    public void Transform(Matrix matrix)
    {
        if (matrix is null)
            throw new ArgumentNullException(nameof(matrix));

        for (int i = 0; i < points.Count; i++)
            points[i] = PointF.Transform(points[i], matrix.MatrixElements);
    }

    public void AddLine(PointF pt1, PointF pt2)
    {
        AddLines([pt1, pt2]);
    }

    public void AddLine(float x1, float y1, float x2, float y2)
    {
        AddLine(new PointF(x1, y1), new PointF(x2, y2));
    }

    public void AddLines(PointF[] points)
    {
        if (points is null)
            throw new ArgumentNullException(nameof(points));
        if (points.Length < 2)
            return;

        AddPathPoints(points, PathPointType.Line);
    }

    public void AddArc(RectangleF rect, float startAngle, float sweepAngle)
    {
        AddArc(rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
    }

    public void AddArc(float x, float y, float width, float height, float startAngle, float sweepAngle)
    {
        const int segments = 24;
        float start = startAngle * (MathF.PI / 180f);
        float sweep = sweepAngle * (MathF.PI / 180f);
        float rx = width / 2f;
        float ry = height / 2f;
        float cx = x + rx;
        float cy = y + ry;

        int count = Math.Max(2, (int)(MathF.Abs(sweepAngle) / 360f * segments));
        var arcPoints = new PointF[count + 1];

        for (int i = 0; i <= count; i++)
        {
            float t = start + sweep * (i / (float)count);
            arcPoints[i] = new PointF(cx + rx * MathF.Cos(t), cy + ry * MathF.Sin(t));
        }

        AddPathPoints(arcPoints, PathPointType.Line);
    }

    public void AddBezier(PointF pt1, PointF pt2, PointF pt3, PointF pt4)
    {
        AddBeziers([pt1, pt2, pt3, pt4]);
    }

    public void AddBezier(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
    {
        AddBezier(new PointF(x1, y1), new PointF(x2, y2), new PointF(x3, y3), new PointF(x4, y4));
    }

    public void AddBeziers(PointF[] points)
    {
        if (points is null)
            throw new ArgumentNullException(nameof(points));
        if (points.Length < 4)
            return;

        AddPathPoints(points, PathPointType.Bezier3);
    }

    public void AddCurve(PointF[] points)
    {
        AddCurve(points, 0.5f);
    }

    public void AddCurve(PointF[] points, float tension)
    {
        if (points is null)
            throw new ArgumentNullException(nameof(points));
        if (points.Length < 2)
            return;

        AddLines(points);
    }

    public void AddCurve(PointF[] points, int offset, int numberOfSegments, float tension)
    {
        if (points is null)
            throw new ArgumentNullException(nameof(points));
        if (offset < 0 || numberOfSegments < 0 || offset + numberOfSegments >= points.Length)
            throw new ArgumentOutOfRangeException();

        var slice = new PointF[numberOfSegments + 1];
        Array.Copy(points, offset, slice, 0, slice.Length);
        AddCurve(slice, tension);
    }

    public void AddClosedCurve(PointF[] points)
    {
        AddClosedCurve(points, 0.5f);
    }

    public void AddClosedCurve(PointF[] points, float tension)
    {
        if (points is null)
            throw new ArgumentNullException(nameof(points));
        if (points.Length < 2)
            return;

        var list = points.ToList();
        list.Add(points[0]);
        AddLines(list.ToArray());
        CloseFigure();
    }

    public void AddEllipse(RectangleF rect)
    {
        AddEllipse(rect.X, rect.Y, rect.Width, rect.Height);
    }

    public void AddEllipse(float x, float y, float width, float height)
    {
        AddRectangle(new RectangleF(x, y, width, height));
    }

    public void AddPie(RectangleF rect, float startAngle, float sweepAngle)
    {
        AddPie(rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
    }

    public void AddPie(float x, float y, float width, float height, float startAngle, float sweepAngle)
    {
        AddArc(x, y, width, height, startAngle, sweepAngle);
        CloseFigure();
    }

    public void AddPolygon(PointF[] points)
    {
        if (points is null)
            throw new ArgumentNullException(nameof(points));
        if (points.Length < 3)
            return;

        AddPathPoints(points, PathPointType.Start);
        CloseFigure();
    }

    public void AddRectangle(RectangleF rect)
    {
        var pts = new[]
        {
            new PointF(rect.X, rect.Y),
            new PointF(rect.X + rect.Width, rect.Y),
            new PointF(rect.X + rect.Width, rect.Y + rect.Height),
            new PointF(rect.X, rect.Y + rect.Height)
        };
        AddPolygon(pts);
    }

    public void AddRectangles(RectangleF[] rects)
    {
        if (rects is null)
            throw new ArgumentNullException(nameof(rects));

        foreach (var rect in rects)
            AddRectangle(rect);
    }

    public void AddString(string s, string family, int style, float emSize, PointF origin, StringFormat? format)
    {
        if (s is null)
            throw new ArgumentNullException(nameof(s));
        if (family is null)
            throw new ArgumentNullException(nameof(family));

        AddRectangle(new RectangleF(origin.X, origin.Y - emSize, s.Length * emSize * 0.5f, emSize));
    }

    public void AddPath(GraphicsPath path, bool connect)
    {
        if (path is null)
            throw new ArgumentNullException(nameof(path));
        if (path.PointCount == 0)
            return;

        if (connect && points.Count > 0)
        {
            points.Add(path.points[0]);
            types.Add((byte)(PathPointType.Line | PathPointType.PathTypeMask));
        }

        points.AddRange(path.points);
        types.AddRange(path.types);
    }

    public void AddPathPoints(PointF[] pts, PathPointType type)
    {
        if (pts is null)
            throw new ArgumentNullException(nameof(pts));
        if (pts.Length == 0)
            return;

        for (int i = 0; i < pts.Length; i++)
        {
            points.Add(pts[i]);
            byte t = (byte)type;
            if (i == 0)
                t |= (byte)PathPointType.Start;
            if (i == pts.Length - 1)
                t |= 0x80;
            types.Add(t);
        }
    }

    public PointF[] Flatten(Matrix? matrix = null, float flatness = 0.25f)
    {
        var clone = (GraphicsPath)Clone();
        if (matrix != null)
            clone.Transform(matrix);
        return clone.Points;
    }

    public bool IsVisible(PointF point)
    {
        if (points.Count == 0)
            return false;

        return true;
    }

    public bool IsVisible(float x, float y)
    {
        return IsVisible(new PointF(x, y));
    }

    public bool IsOutlineVisible(PointF point, Pen pen)
    {
        if (pen is null)
            throw new ArgumentNullException(nameof(pen));

        return IsVisible(point);
    }

    public bool IsOutlineVisible(float x, float y, Pen pen)
    {
        return IsOutlineVisible(new PointF(x, y), pen);
    }

    public RectangleF GetBounds()
    {
        if (points.Count == 0) return RectangleF.Empty;

        float minX = points[0].X;
        float minY = points[0].Y;
        float maxX = points[0].X;
        float maxY = points[0].Y;

        foreach (var p in points)
        {
            minX = MathF.Min(minX, p.X);
            minY = MathF.Min(minY, p.Y);
            maxX = MathF.Max(maxX, p.X);
            maxY = MathF.Max(maxY, p.Y);
        }

        return new RectangleF(minX, minY, maxX - minX, maxY - minY);
    }

    public void Widen(Pen pen)
    {
    }

    public void Widen(Pen pen, Matrix matrix)
    {
    }

    public void Widen(Pen pen, Matrix matrix, float flatness)
    {
    }

    private int GetFigureStartIndex(int fromIndex)
    {
        if (fromIndex < 0 || fromIndex >= points.Count)
            return -1;

        for (int i = fromIndex; i >= 0; i--)
        {
            if ((types[i] & 0x07) == (byte)PathPointType.Start)
                return i;
        }

        return -1;
    }

    private void SetPath(PointF[] pts, byte[]? pathTypes)
    {
        points.Clear();
        types.Clear();

        if (pathTypes is null || pathTypes.Length != pts.Length)
        {
            for (int i = 0; i < pts.Length; i++)
            {
                points.Add(pts[i]);
                types.Add(i == 0 ? (byte)PathPointType.Start : (byte)PathPointType.Line);
            }
            return;
        }

        points.AddRange(pts);
        types.AddRange(pathTypes);
    }
}