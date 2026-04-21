#if SystemDrawing
namespace System.Drawing.Drawing2D;
#else
namespace Shone.Drawing.Drawing2D;
#endif

public static class InterpolationModeExtensions
{
    /// <summary>
    /// Converts an InterpolationMode to a MewUI compatible value.
    /// </summary>
    /// <param name="mode">The InterpolationMode to convert.</param>
    /// <returns>A corresponding value for MewUI.</returns>
    public static int ToMewInterpolationMode(this InterpolationMode mode)
    {
        switch (mode)
        {
            case InterpolationMode.Low:
            case InterpolationMode.NearestNeighbor:
                // Possibly no smoothing
                return 0;

            case InterpolationMode.Bilinear:
            case InterpolationMode.Default:
                // Basic smoothing
                return 1;

            case InterpolationMode.Bicubic:
                // Medium quality
                return 2;

            case InterpolationMode.High:
            case InterpolationMode.HighQualityBilinear:
            case InterpolationMode.HighQualityBicubic:
                // Highest quality smoothing
                return 3;

            // If not recognized, default to Low
            default:
                return 1;
        }
    }
}