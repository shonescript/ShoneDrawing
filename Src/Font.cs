using Aprillz.MewUI;
using Aprillz.MewUI.Rendering;

#if SystemDrawing
namespace System.Drawing;
#else
namespace Shone.Drawing;
#endif
/// <summary>
/// A simplified class that mimics System.Drawing.Font
/// using MewUI to store typeface and style information.
/// </summary>
public class Font : IDisposable
{
    private bool disposed;

    private IFont mewFont;
    private float size;  // in "points" or user-chosen units
    private FontStyle style;
    private string familyName;

    /// <summary>
    /// Initializes a new Font with the specified family name, size, and style.
    /// </summary>
    /// <param name="familyName">The font family name (e.g. "Arial").</param>
    /// <param name="emSize">The font size in points (or float units).</param>
    /// <param name="style">The font style (regular, bold, italic, etc.).</param>
    public Font(string familyName, float emSize, FontStyle style = FontStyle.Regular)
    {
        this.familyName = familyName;
        this.size = emSize;
        this.style = style;

        // Create MewUI font
        var weight = (style & FontStyle.Bold) != 0 ? FontWeight.Bold : FontWeight.Normal;
        var isItalic = (style & FontStyle.Italic) != 0;
        var isUnderline = (style & FontStyle.Underline) != 0;
        var isStrikethrough = (style & FontStyle.Strikeout) != 0;

        // Use graphics factory to create font
        
        mewFont = Graphics.Factory.CreateFont(familyName, emSize, weight, isItalic, isUnderline, isStrikethrough);
    }

    /// <summary>
    /// The name of the font family (e.g. "Arial", "Times New Roman").
    /// </summary>
    public string FontFamily
    {
        get
        {
            return familyName;
        }
    }

    /// <summary>
    /// The em-size (in points or user-chosen float) of this font.
    /// </summary>
    public float Size
    {
        get
        {
            return size;
        }
    }

    /// <summary>
    /// The style (bold, italic, etc.) of this font.
    /// </summary>
    public FontStyle Style
    {
        get
        {
            return style;
        }
    }

    /// <summary>
    /// Returns true if this font is bold.
    /// </summary>
    public bool Bold => (style & FontStyle.Bold) != 0;

    /// <summary>
    /// Returns true if this font is italic.
    /// </summary>
    public bool Italic => (style & FontStyle.Italic) != 0;

    /// <summary>
    /// Returns true if this font is underlined.
    /// (In System.Drawing, you'd typically handle the underline in text drawing.)
    /// </summary>
    public bool Underline => (style & FontStyle.Underline) != 0;

    /// <summary>
    /// Returns true if this font is strikeout.
    /// (Similarly, handle the strike line in text drawing if needed.)
    /// </summary>
    public bool Strikeout => (style & FontStyle.Strikeout) != 0;

    #region MewUI Interop

    /// <summary>
    /// Converts this Font to MewUI IFont.
    /// </summary>
    public IFont ToMewFont()
    {
        return mewFont;
    }

    #endregion

    #region Dispose

    public void Dispose()
    {
        if (!disposed)
        {
            disposed = true;
            mewFont?.Dispose();
            mewFont = null;
        }
    }

    #endregion

    public override string ToString()
    {
        return $"Font [Family={familyName}, Size={size}, Style={style}]";
    }
}
