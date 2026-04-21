#if SystemDrawing
namespace System.Drawing.Text;
#else
namespace Shone.Drawing.Text;
#endif
/// <summary>
/// Specifies how textures or images are tiled or clamped 
/// when they are smaller or larger than the destination area.
/// Mimics System.Drawing.Drawing2D.WrapMode.
/// </summary>
public enum TextRenderingHint
{
    SystemDefault,
    SingleBitPerPixelGridFit,
    SingleBitPerPixel,
    AntiAliasGridFit,
    AntiAlias,
    ClearTypeGridFit
}