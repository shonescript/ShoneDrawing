using System;
using System.IO;
using Aprillz.MewUI;
using Aprillz.MewUI.Rendering;

namespace ShoneDrawing
{
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
            this.image = image ?? throw new ArgumentNullException(nameof(image));
        }

        /// <summary>
        /// Initializes a new instance of the Icon class by decoding the image data from a memory stream.
        /// </summary>
        /// <param name="memoryStream">A memory stream containing icon data.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception">Thrown if the icon cannot be decoded.</exception>
        public Icon(MemoryStream memoryStream)
        {
            if (memoryStream == null)
                throw new ArgumentNullException(nameof(memoryStream));

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
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            var img = Image.FromFile(fileName);
            if (img == null)
                throw new Exception("Unable to decode the icon from file.");
            return new Icon(img.ToMewImage());
        }

        /// <summary>
        /// Loads an icon from a stream.
        /// </summary>
        /// <param name="stream">A stream containing the encoded icon data.</param>
        /// <returns>An Icon instance.</returns>
        public static Icon FromStream(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

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
            if (image == null)
                throw new ObjectDisposedException(nameof(Icon));
            var bitmap = new Bitmap(image.PixelWidth, image.PixelHeight);
            var graphicsFactory = Aprillz.MewUI.Application.DefaultGraphicsFactory;
            using (var context = graphicsFactory.CreateContext(bitmap.ToRenderTarget()))
            {
                context.DrawImage(image, new Aprillz.MewUI.Point(0, 0));
            }
            return bitmap;
        }

        /// <summary>
        /// Saves the icon to the specified stream in the given image format.
        /// </summary>
        /// <param name="stream">The output stream.</param>
        /// <param name="format">The desired image format.</param>
        /// <param name="quality">The quality (0-100) for encoding.</param>
        public void Save(Stream stream, ImageFormat format, int quality = 100)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (image == null)
                throw new ObjectDisposedException(nameof(Icon));

            // TODO: Implement Save for MewUI
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
}
