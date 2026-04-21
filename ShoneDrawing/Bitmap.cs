using System;
using System.IO;
using System.Runtime.InteropServices;
using Aprillz.MewUI;
using Aprillz.MewUI.Rendering;

namespace ShoneDrawing
{
    public class Bitmap : Image
    {
        private IImage image;
        private IBitmapRenderTarget renderTarget;

        private float horizontalResolution = 96.0f;
        private float verticalResolution   = 96.0f;

        #region Constructors

        public Bitmap(int width, int height)
        {
            var graphicsFactory = Aprillz.MewUI.Application.DefaultGraphicsFactory;
            renderTarget = graphicsFactory.CreateBitmapRenderTarget(width, height);
            image = graphicsFactory.CreateImageFromPixelSource(renderTarget);
            PixelFormat = PixelFormat.Format32bppArgb;
        }

        public Bitmap(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            var bytes = File.ReadAllBytes(fileName);
            var graphicsFactory = Aprillz.MewUI.Application.DefaultGraphicsFactory;
            image = graphicsFactory.CreateImageFromBytes(bytes);
            PixelFormat = PixelFormat.Format32bppArgb;
        }

        public Bitmap(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            var bytes = ms.ToArray();
            var graphicsFactory = Aprillz.MewUI.Application.DefaultGraphicsFactory;
            image = graphicsFactory.CreateImageFromBytes(bytes);
            PixelFormat = PixelFormat.Format32bppArgb;
        }

        public Bitmap(Bitmap b, Size s)
        {
            if (b == null)
                throw new ArgumentNullException(nameof(b));

            var graphicsFactory = Aprillz.MewUI.Application.DefaultGraphicsFactory;
            renderTarget = graphicsFactory.CreateBitmapRenderTarget(s.Width, s.Height);
            using (var context = graphicsFactory.CreateContext(renderTarget))
            {
                var destRect = new Aprillz.MewUI.Rect(0, 0, s.Width, s.Height);
                context.DrawImage(b.image, destRect);
            }
            image = graphicsFactory.CreateImageFromPixelSource(renderTarget);

            // Copy over resolution from original
            horizontalResolution = b.horizontalResolution;
            verticalResolution   = b.verticalResolution;
            
            PixelFormat = b.PixelFormat;
        }
        
        /// <summary>
        /// Creates a Bitmap of the given width/height in the specified PixelFormat.
        /// In this simplified example, we primarily handle PixelFormat.Format32bppArgb 
        /// by creating a 32-bit RGBA (premultiplied) bitmap.
        /// </summary>
        public Bitmap(int width, int height, PixelFormat format)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentOutOfRangeException("Width and height must be positive.");

            // Example: handle only Format32bppArgb.
            // If you need more formats, add logic or throw for unsupported.
            if (format != PixelFormat.Format32bppArgb)
                throw new NotSupportedException(
                    $"Only Format32bppArgb is supported in this constructor, but got {format}."
                );

            var graphicsFactory = Aprillz.MewUI.Application.DefaultGraphicsFactory;
            renderTarget = graphicsFactory.CreateBitmapRenderTarget(width, height);
            image = graphicsFactory.CreateImageFromPixelSource(renderTarget);
            PixelFormat = format;
        }

        /// <summary>
        /// Creates a Bitmap from raw pixel data, using the provided width, height, stride, 
        /// PixelFormat (p), and pointer to the pixel data (scan0).
        /// The raw data is copied into this bitmap’s internal buffer.
        /// </summary>
        public Bitmap(int width, int height, int stride, PixelFormat p, IntPtr scan0)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentOutOfRangeException("Width and height must be positive.");
            if (scan0 == IntPtr.Zero)
                throw new ArgumentNullException(nameof(scan0), "scan0 cannot be IntPtr.Zero.");

            // TODO: Implement raw pixel data initialization for MewUI
            var graphicsFactory = Aprillz.MewUI.Application.DefaultGraphicsFactory;
            renderTarget = graphicsFactory.CreateBitmapRenderTarget(width, height);
            image = graphicsFactory.CreateImageFromPixelSource(renderTarget);
            PixelFormat = p;
        }

        /// <summary>
        /// Creates a new Bitmap from an existing one, scaled to the specified width and height.
        /// </summary>
        public Bitmap(Bitmap b, int width, int height)
        {
            if (b == null)
                throw new ArgumentNullException(nameof(b));
            if (width <= 0 || height <= 0)
                throw new ArgumentOutOfRangeException("Width and height must be positive.");

            var graphicsFactory = Aprillz.MewUI.Application.DefaultGraphicsFactory;
            renderTarget = graphicsFactory.CreateBitmapRenderTarget(width, height);
            using (var context = graphicsFactory.CreateContext(renderTarget))
            {
                var destRect = new Aprillz.MewUI.Rect(0, 0, width, height);
                context.DrawImage(b.image, destRect);
            }
            image = graphicsFactory.CreateImageFromPixelSource(renderTarget);

            // Copy resolution from original
            horizontalResolution = b.horizontalResolution;
            verticalResolution   = b.verticalResolution;
            
            PixelFormat = b.PixelFormat;
        }

        #endregion

        #region Properties

        public new int Width
        {
            get
            {
                if (image == null)
                    throw new ObjectDisposedException(nameof(Bitmap));
                return image.PixelWidth;
            }
        }

        public new int Height
        {
            get
            {
                if (image == null)
                    throw new ObjectDisposedException(nameof(Bitmap));
                return image.PixelHeight;
            }
        }

        public float HorizontalResolution
        {
            get => horizontalResolution;
            set => horizontalResolution = value;
        }

        public float VerticalResolution
        {
            get => verticalResolution;
            set => verticalResolution = value;
        }

        public IntPtr Scan0
        {
            get
            {
                if (image == null)
                    throw new ObjectDisposedException(nameof(Bitmap));
                // TODO: Implement Scan0 for MewUI
                throw new NotImplementedException("Scan0 is not implemented for MewUI Bitmap");
            }
        }

        public int Stride
        {
            get
            {
                if (image == null)
                    throw new ObjectDisposedException(nameof(Bitmap));
                // TODO: Implement Stride for MewUI
                return Width * 4; // Assuming 32bpp
            }
        }

        #endregion

        #region Pixel Access

        public Color GetPixel(int x, int y)
        {
            if (image == null)
                throw new ObjectDisposedException(nameof(Bitmap));
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                throw new ArgumentOutOfRangeException();

            // TODO: Implement GetPixel for MewUI
            return Color.Black;
        }

        public void SetPixel(int x, int y, Color color)
        {
            if (image == null)
                throw new ObjectDisposedException(nameof(Bitmap));
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                throw new ArgumentOutOfRangeException();

            // TODO: Implement SetPixel for MewUI
        }

        #endregion

        #region Clone

        /// <summary>
        /// Creates an exact copy of this entire bitmap.
        /// </summary>
        public Bitmap Clone()
        {
            if (image == null)
                throw new ObjectDisposedException(nameof(Bitmap));

            var newBitmap = new Bitmap(Width, Height);
            var graphicsFactory = Aprillz.MewUI.Application.DefaultGraphicsFactory;
            using (var context = graphicsFactory.CreateContext(newBitmap.renderTarget))
            {
                context.DrawImage(image, new Aprillz.MewUI.Point(0, 0));
            }
            newBitmap.HorizontalResolution = this.horizontalResolution;
            newBitmap.VerticalResolution = this.verticalResolution;
            newBitmap.PixelFormat = this.PixelFormat;

            return newBitmap;
        }

        /// <summary>
        /// Clones a portion of this bitmap into a new one, possibly converting pixel format.
        /// </summary>
        public Bitmap Clone(Rectangle r, PixelFormat f)
        {
            if (image == null)
                throw new ObjectDisposedException(nameof(Bitmap));

            if (r.X < 0 || r.Y < 0 || r.Width < 0 || r.Height < 0 ||
                r.X + r.Width > Width || r.Y + r.Height > Height)
            {
                throw new ArgumentException("The specified rectangle is out of bounds.");
            }

            var newBitmap = new Bitmap(r.Width, r.Height, f);
            var graphicsFactory = Aprillz.MewUI.Application.DefaultGraphicsFactory;
            using (var context = graphicsFactory.CreateContext(newBitmap.renderTarget))
            {
                var srcRect = new Aprillz.MewUI.Rect(r.X, r.Y, r.Width, r.Height);
                var dstRect = new Aprillz.MewUI.Rect(0, 0, r.Width, r.Height);
                context.DrawImage(image, dstRect, srcRect);
            }
            newBitmap.HorizontalResolution = this.horizontalResolution;
            newBitmap.VerticalResolution = this.verticalResolution;

            return newBitmap;
        }

        #endregion

        #region LockBits / UnlockBits

        public BitmapData LockBits(Rectangle r, ImageLockMode m, PixelFormat f)
        {
            if (image == null)
                throw new ObjectDisposedException(nameof(Bitmap));

            if (r.X < 0 || r.Y < 0 || r.Width < 0 || r.Height < 0 ||
                r.X + r.Width > Width || r.Y + r.Height > Height)
            {
                throw new ArgumentException("The specified rectangle is out of the bitmap bounds.");
            }

            // TODO: Implement LockBits for MewUI
            throw new NotImplementedException("LockBits is not implemented for MewUI Bitmap");
        }

        public void UnlockBits(BitmapData bmData)
        {
            if (bmData == null)
                throw new ArgumentNullException(nameof(bmData));
            // No copy-back in this simplified approach
        }

        #endregion

        #region Save Methods

        public void Save(string fileName, ImageFormat format, int quality = 100)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            // TODO: Implement Save for MewUI
            throw new NotImplementedException("Save is not implemented for MewUI Bitmap");
        }
        
        public void Save(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            Save(fileName, ImageFormat.Png);
        }

        public void Save(Stream stream, ImageFormat format, int quality = 100)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            // TODO: Implement Save for MewUI
            throw new NotImplementedException("Save is not implemented for MewUI Bitmap");
        }

        public void Save(MemoryStream s, ImageFormat f)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));

            // TODO: Implement Save for MewUI
            throw new NotImplementedException("Save is not implemented for MewUI Bitmap");
        }

        #endregion

        #region Conversion

        public IImage ToMewImage()
        {
            if (image == null)
                throw new ObjectDisposedException(nameof(Bitmap));
            return image;
        }

        public IRenderTarget ToRenderTarget()
        {
            if (renderTarget == null)
                throw new ObjectDisposedException(nameof(Bitmap));
            return renderTarget;
        }

        #endregion

        #region DPI Helpers

        public void SetResolution(float horizontal, float vertical)
        {
            if (horizontal <= 0 || vertical <= 0)
                throw new ArgumentOutOfRangeException("Resolution must be positive.");
            horizontalResolution = horizontal;
            verticalResolution   = vertical;
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            if (image != null)
            {
                image.Dispose();
                image = null;
            }
            if (renderTarget != null)
            {
                renderTarget.Dispose();
                renderTarget = null;
            }
        }

        #endregion

        public override string ToString()
        {
            if (image == null)
                return "Bitmap: Disposed";

            return $"Bitmap: {Width} x {Height}, Stride: {Stride} bytes, DPI: {horizontalResolution}x{verticalResolution}";
        }

        /// <summary>
        /// Estimate bytes/pixel for the chosen pixel format.
        /// </summary>
        private int EstimateBytesPerPixel(PixelFormat fmt)
        {
            switch (fmt)
            {
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    return 4;
                case PixelFormat.Format24bppRgb:
                    return 3;
                case PixelFormat.Format16bppRgb565:
                    return 2;
                case PixelFormat.Format8bppGray:
                    return 1;
                default:
                    return 4;
            }
        }
    }
}
