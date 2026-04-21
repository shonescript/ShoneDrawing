using Aprillz.MewUI.Rendering;



/// <summary>
/// A simple Icon class that mimics some of the functionality of System.Drawing.Icon
/// but is implemented using MewUI.
/// </summary>
public class Icon : IDisposable
{
    private IImage image;

    /// <summary>
    /// Initializes a new instance of the Icon class from an IImage.
    /// </summary>
    /// <param name="image">The IImage to wrap as an icon.</param>
    public Icon(IImage image)
    {
        this.image = image;
    }

    public Icon(MemoryStream memoryStream)
    {
        var img = Image.FromStream(memoryStream);
        if (img == null)
            throw new Exception("Unable to decode the icon from the memory stream.");
        image = img.ToMewImage();
    }

    /// <summary>
    /// Gets the width of the icon.
    /// </summary>
    public int Width => image?.PixelWidth ?? 0;

    /// <summary>
    /// Gets the height of the icon.
    /// </summary>
    public int Height => image?.PixelHeight ?? 0;

    /// <summary>
    /// Loads an icon from a file.
    /// </summary>
    /// <param name="fileName">The file path of the icon.</param>
    /// <returns>An Icon instance.</returns>
    public static Icon FromFile(string fileName)
    {
        var img = Image.FromFile(fileName);
        if (img == null)
            throw new Exception("Unable to decode the icon from file.");
        return new Icon(img.ToMewImage());
    }

    public static Icon FromStream(Stream stream)
    {
        var img = Image.FromStream(stream);
        if (img == null)
            throw new Exception("Unable to decode the icon from the stream.");
        return new Icon(img.ToMewImage());
    }

    /// <summary>
    /// Converts the icon to a Bitmap.
    /// </summary>
    /// <returns>A Bitmap representing the icon.</returns>
    public Bitmap ToBitmap()
    {
        var bitmap = new Bitmap(image.PixelWidth, image.PixelHeight);
        var graphicsFactory = Aprillz.MewUI.Application.DefaultGraphicsFactory;
        using (var context = graphicsFactory.CreateContext(bitmap.ToRenderTarget()))
        {
            context.DrawImage(image, new Aprillz.MewUI.Point(0, 0));
        }
        return bitmap;
    }

    public void Save(Stream stream, ImageFormat format, int quality = 100)
    {

        throw new NotImplementedException("Save is not implemented for MewUI Icon");
    }

    /// <summary>
    /// Releases all resources used by the Icon.
    /// </summary>
    public void Dispose()
    {
        image?.Dispose();
        image = null;
    }

    /// <summary>
    /// Returns a string that represents the current Icon.
    /// </summary>
    public override string ToString()
    {
        return $"Icon: {Width} x {Height}";
    }
}
