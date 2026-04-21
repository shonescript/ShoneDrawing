using System;
using Aprillz.MewUI;

namespace ShoneDrawing
{
    /// <summary>
    /// A simplified version of System.Drawing.Imaging.ImageFormat, mapped to MewUI's ImageFormat.
    /// </summary>
    public enum ImageFormat
    {
        Bmp,
        Jpeg,
        Png
        // You can add others if supported by MewUI
    }

    /// <summary>
    /// Extension methods to convert between the custom ImageFormat and MewUI's ImageFormat.
    /// </summary>
    public static class ImageFormatExtensions
    {
        /// <summary>
        /// Converts the custom ImageFormat enum to the corresponding MewUI ImageFormat.
        /// </summary>
        /// <param name="format">The custom ImageFormat.</param>
        /// <returns>The corresponding MewUI ImageFormat.</returns>
        /// <exception cref="System.NotSupportedException">Thrown if the given format is not supported by this mapping.</exception>
        public static Aprillz.MewUI.ImageFormat ToMewImageFormat(this ImageFormat format)
        {
            switch (format)
            {
                case ImageFormat.Bmp:
                    return Aprillz.MewUI.ImageFormat.Bmp;
                case ImageFormat.Jpeg:
                    return Aprillz.MewUI.ImageFormat.Jpeg;
                case ImageFormat.Png:
                    return Aprillz.MewUI.ImageFormat.Png;
                default:
                    throw new System.NotSupportedException($"The image format '{format}' is not supported.");
            }
        }
    }
}
