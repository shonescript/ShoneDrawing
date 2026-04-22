using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
#if SystemDrawing
namespace System.Drawing;
#else
namespace Shone.Drawing;
#endif

public struct PointF : IEquatable<PointF>
{
    public static readonly PointF Empty;

    private float x;

    private float y;

    [Browsable(false)]
    public readonly bool IsEmpty
    {
        get
        {
            if (x == 0f)
            {
                return y == 0f;
            }
            return false;
        }
    }

    public float X
    {
        readonly get
        {
            return x;
        }
        set
        {
            x = value;
        }
    }

    public float Y
    {
        readonly get
        {
            return y;
        }
        set
        {
            y = value;
        }
    }

    public Vector2 Vector2 => new Vector2(X, Y);

    public PointF(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public PointF(Vector2 vector)
    {
        x = vector.X;
        y = vector.Y;
    }

    public Vector2 ToVector2()
    {
        return new Vector2(x, y);
    }


    public static implicit operator PointF(Vector2 v) => new PointF(v.X, v.Y);

    public static explicit operator Vector2(PointF point)
    {
        return point.ToVector2();
    }

    public static PointF operator +(PointF pt, Size sz)
    {
        return Add(pt, sz);
    }

    public static PointF operator -(PointF pt, Size sz)
    {
        return Subtract(pt, sz);
    }

    public static PointF operator +(PointF pt, SizeF sz)
    {
        return Add(pt, sz);
    }

    public static PointF operator -(PointF pt, SizeF sz)
    {
        return Subtract(pt, sz);
    }

    public static bool operator ==(PointF left, PointF right)
    {
        if (left.X == right.X)
        {
            return left.Y == right.Y;
        }
        return false;
    }

    public static bool operator !=(PointF left, PointF right)
    {
        return !(left == right);
    }

    public static PointF Add(PointF pt, Size sz)
    {
        return new PointF(pt.X + (float)sz.Width, pt.Y + (float)sz.Height);
    }

    public static PointF Subtract(PointF pt, Size sz)
    {
        return new PointF(pt.X - (float)sz.Width, pt.Y - (float)sz.Height);
    }

    public static PointF Add(PointF pt, SizeF sz)
    {
        return new PointF(pt.X + sz.Width, pt.Y + sz.Height);
    }

    public static PointF Subtract(PointF pt, SizeF sz)
    {
        return new PointF(pt.X - sz.Width, pt.Y - sz.Height);
    }

    public override readonly bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is PointF)
        {
            return Equals((PointF)obj);
        }
        return false;
    }

    public readonly bool Equals(PointF other)
    {
        return this == other;
    }

    public override readonly int GetHashCode()
    {
        return HashCode.Combine(X.GetHashCode(), Y.GetHashCode());
    }

    public override readonly string ToString()
    {
        return $"{{X={x}, Y={y}}}";
    }

    public static PointF Transform(PointF p, Matrix3x2 matrix)
    {
        return Vector2.Transform(p.Vector2, matrix);
    }
}

