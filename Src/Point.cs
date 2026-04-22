#if SystemDrawing
namespace System.Drawing;
#else
using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Shone.Drawing;
#endif
public struct Point
{
    public int X;
    public int Y;
    
    public bool IsEmpty => X == 0 && Y == 0;

    public Vector2 Vector2 => new Vector2(X, Y);

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    // Example overrides, operators, etc.
    public override string ToString() => $"{{X={X},Y={Y}}}";

    public static Point Empty => new Point(0, 0);

    public static explicit operator Point(Vector2 v)
        => new Point((int)MathF.Round(v.X), (int)MathF.Round(v.Y));
}
