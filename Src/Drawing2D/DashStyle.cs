#if SystemDrawing
namespace System.Drawing.Drawing2D;
#else
namespace Shone.Drawing.Drawing2D;
#endif
public enum DashStyle
{
    Solid,
    Dash,
    Dot,
    DashDot,
    DashDotDot,
    Custom
}
