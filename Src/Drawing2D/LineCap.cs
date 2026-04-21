#if SystemDrawing
namespace System.Drawing.Drawing2D;
#else
namespace Shone.Drawing.Drawing2D;
#endif
public enum LineCap
{
    Flat = 0,
    Square = 1,
    Round = 2,
    Triangle = 3,
    NoAnchor = 16,
    SquareAnchor = 17,
    RoundAnchor = 18,
    DiamondAnchor = 19,
    ArrowAnchor = 20,
    Custom = 255,
    AnchorMask = 240
}