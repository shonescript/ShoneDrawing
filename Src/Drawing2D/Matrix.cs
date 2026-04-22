#if SystemDrawing
namespace System.Drawing.Drawing2D;
#else
namespace Shone.Drawing.Drawing2D;

using System.Diagnostics.CodeAnalysis;
#endif
using System.Numerics;

public enum MatrixOrder
{
    Prepend,
    Append
}


public class Matrix : IDisposable
{
    Matrix3x2 matrix;

    public float[] Elements => [matrix.M11, matrix.M12, matrix.M21, matrix.M22, matrix.M31, matrix.M32];

    public bool IsIdentity => matrix.IsIdentity;

    public bool IsInvertible
    {
        get
        {
            return Matrix3x2.Invert(matrix, out _);
        }
    }

    public float OffsetX => matrix.M31;

    public float OffsetY => matrix.M32;

    public Matrix3x2 MatrixElements => matrix;

    public Matrix()
    {
        matrix = Matrix3x2.Identity;
    }

    public Matrix(float m11, float m12, float m21, float m22, float dx, float dy)
    {
        matrix = new Matrix3x2(m11, m12, m21, m22, dx, dy);
    }

    public void Dispose()
    {
    }

    public override int GetHashCode() => matrix.GetHashCode();

    public override bool Equals(object? obj)
    {
        return obj is Matrix m ? matrix == m.matrix : false;
    }

    public Matrix Clone()
    {
        var m = new Matrix();
        m.matrix = matrix;
        return m;
    }

    public void Invert()
    {
        if (!Matrix3x2.Invert(matrix, out var inverted))
            throw new InvalidOperationException("The matrix is not invertible.");

        matrix = inverted;
    }

    public void Translate(float offsetX, float offsetY)
    {
        Translate(offsetX, offsetY, MatrixOrder.Prepend);
    }

    public void Translate(float offsetX, float offsetY, MatrixOrder order)
    {
        var t = Matrix3x2.CreateTranslation(offsetX, offsetY);
        matrix = order == MatrixOrder.Prepend
            ? t * matrix
            : matrix * t;
    }

    public void Scale(float scaleX, float scaleY)
    {
        Scale(scaleX, scaleY, MatrixOrder.Prepend);
    }

    public void Scale(float scaleX, float scaleY, MatrixOrder order)
    {
        var s = Matrix3x2.CreateScale(scaleX, scaleY);
        matrix = order == MatrixOrder.Prepend
            ? s * matrix
            : matrix * s;
    }

    public void Shear(float shearX, float shearY)
    {
        Shear(shearX, shearY, MatrixOrder.Prepend);
    }

    public void Shear(float shearX, float shearY, MatrixOrder order)
    {
        var s = new Matrix3x2(
            1f, shearY,
            shearX, 1f,
            0f, 0f);

        matrix = order == MatrixOrder.Prepend
            ? s * matrix
            : matrix * s;
    }

    public void RotateAt(float angle, PointF point)
    {
        RotateAt(angle, point, MatrixOrder.Prepend);
    }

    public void RotateAt(float angle, PointF point, MatrixOrder order)
    {
        var toOrigin = Matrix3x2.CreateTranslation(-point.X, -point.Y);
        var rotate = Matrix3x2.CreateRotation(angle * (MathF.PI / 180f));
        var back = Matrix3x2.CreateTranslation(point.X, point.Y);

        var r = toOrigin * rotate * back;

        matrix = order == MatrixOrder.Prepend
            ? r * matrix
            : matrix * r;
    }

    public void TransformPoints(PointF[] pts)
    {
        TransformPoints(pts.AsSpan());
    }

    public void TransformPoints(params Span<PointF> pts)
    {
        for (int i = 0; i < pts.Length; i++)
        {
            var p = pts[i];
            pts[i] = Vector2.Transform(p.Vector2, matrix);
        }
    }

    public void TransformPoints(Point[] pts)
    {
        TransformPoints(pts.AsSpan());
    }

    public void TransformPoints(params Span<Point> pts)
    {
        for (int i = 0; i < pts.Length; i++)
        {
            var p = pts[i];
            var x = p.X * matrix.M11 + p.Y * matrix.M21 + matrix.M31;
            var y = p.X * matrix.M12 + p.Y * matrix.M22 + matrix.M32;
            pts[i] = new Point((int)MathF.Round(x), (int)MathF.Round(y));
        }
    }

    public void TransformVectors(PointF[] pts)
    {
        TransformVectors(pts.AsSpan());
    }

    public void TransformVectors(params Span<PointF> pts)
    {
        for (int i = 0; i < pts.Length; i++)
        {
            var p = pts[i];
            pts[i] = Vector2.TransformNormal(p.Vector2, matrix);
        }
    }

    public void Multiply(Matrix matrix)
    {
        Multiply(matrix, MatrixOrder.Prepend);
    }

    public void Multiply(Matrix matrix, MatrixOrder order)
    {
        this.matrix = order == MatrixOrder.Prepend
            ? matrix.matrix * this.matrix
            : this.matrix * matrix.matrix;
    }

    public void Rotate(float angle)
    {
        Rotate(angle, MatrixOrder.Prepend);
    }

    public void Rotate(float angle, MatrixOrder order)
    {
        RotateAt(angle, new PointF(0f, 0f), order);
    }

    public void Reset()
    {
        matrix = Matrix3x2.Identity;
    }
}