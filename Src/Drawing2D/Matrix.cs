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
            // to do
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
        // to do
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
        // to do
    }

    public void Translate(float offsetX, float offsetY)
    {
        Translate(offsetX, offsetY, MatrixOrder.Prepend);
    }

    public void Translate(float offsetX, float offsetY, MatrixOrder order)
    {
        // to do
    }

    public void Scale(float scaleX, float scaleY)
    {
        Scale(scaleX, scaleY, MatrixOrder.Prepend);
    }

    public void Scale(float scaleX, float scaleY, MatrixOrder order)
    {
        // to do
    }

    public void Shear(float shearX, float shearY)
    {
        Shear(shearX, shearY, MatrixOrder.Prepend);
    }

    public void Shear(float shearX, float shearY, MatrixOrder order)
    {
        // to do
    }

    public void RotateAt(float angle, PointF point)
    {
        RotateAt(angle, point, MatrixOrder.Prepend);
    }

    public void RotateAt(float angle, PointF point, MatrixOrder order)
    {
        // to do
    }

    public void TransformPoints(PointF[] pts)
    {
        TransformPoints(pts.AsSpan());
    }
    public void TransformPoints(params ReadOnlySpan<PointF> pts)
    {
        // to do
    }

    public void TransformPoints(Point[] pts)
    {
        TransformPoints(pts.AsSpan());
    }
    public void TransformPoints(params ReadOnlySpan<Point> pts)
    {
        // to do
    }

    public void TransformVectors(PointF[] pts)
    {
        TransformVectors(pts.AsSpan());
    }
    public void TransformVectors(params ReadOnlySpan<PointF> pts)
    {
        // to do
    }

    public void Multiply(Matrix matrix)
    {
        Multiply(matrix, MatrixOrder.Prepend);
    }

    public void Multiply(Matrix matrix, MatrixOrder order)
    {
        // to do
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
        // to do
    }
}
