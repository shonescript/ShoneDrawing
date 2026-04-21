using System;

namespace ShoneDrawing
{
    public static class PixelFormatExtensions
    {
        /// <summary>
        /// Converts our custom PixelFormat to MewUI compatible value, if possible.
        /// Throws NotSupportedException if there is no direct mapping.
        /// </summary>
        public static int ToMewPixelFormat(this PixelFormat format)
        {
            switch (format)
            {
                case PixelFormat.Format16bppRgb565:
                    return 1; // 16bpp RGB565

                case PixelFormat.Format24bppRgb:
                    // MewUI treats this as 32bpp with alpha = opaque
                    return 2; // 32bpp RGB

                case PixelFormat.Format32bppRgb:
                    // GDI+ "32bppRgb" is actually 0x22009, alpha is unused but present
                    return 2; // 32bpp RGB

                case PixelFormat.Format32bppArgb:
                    return 3; // 32bpp ARGB

                case PixelFormat.Format32bppPArgb:
                    // MewUI doesn't differentiate "premultiplied" color type separately
                    return 3; // 32bpp ARGB

                case PixelFormat.Format8bppGray:
                    return 0; // 8bpp Grayscale
                
                case PixelFormat.Format8bppIndexed:
                    return 0; // 8bpp Grayscale

                case PixelFormat.Undefined:
                default:
                    throw new NotSupportedException(
                        $"The pixel format '{format}' is not supported or not mapped to a MewUI pixel format.");
            }
        }
    }
}
