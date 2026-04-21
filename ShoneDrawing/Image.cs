using System;
using System.IO;
using Aprillz.MewUI;
using Aprillz.MewUI.Rendering;

namespace ShoneDrawing
{
    /// <summary>
    /// A simplified class that mimics System.Drawing.Image using MewUI for pixel data.
    /// </summary>
    public class Image : IDisposable
    {
        protected IImage mewImage;

        protected float horizontalResolution = 96.0f;
        protected float verticalResolution = 96.0f;

        private bool disposed;
        
        public PixelFormat PixelFormat { get; set; }

        #region Constructors

        protected Image()
        {
            // Empty or internal usage.
            PixelFormat = PixelFormat.Format32bppArgb;
        }

        /// <summary>
        /// Constructs an Image from an existing IImage.
        /// </summary>
        public Image(IImage image)
        {
            mewImage = image;
        }

        #endregion

        #region Static Creation Methods

        public static Image FromFile(string filename)
        {
            var bytes = File.ReadAllBytes(filename);
            var graphicsFactory = Aprillz.MewUI.Application.DefaultGraphicsFactory;
            IImage img = graphicsFactory.CreateImageFromBytes(bytes);
            if (img == null)
                throw new Exception($"Failed to decode image from file: {filename}");
            Image result = new Image(img);

            return result;
        }

        public static Image FromStream(Stream stream)
        {
            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            var bytes = ms.ToArray();
            var graphicsFactory = Aprillz.MewUI.Application.DefaultGraphicsFactory;
            IImage img = graphicsFactory.CreateImageFromBytes(bytes);
            if (img == null)
                throw new Exception("Failed to decode image from stream.");
            Image result = new Image(img);

            return result;
        }

        #endregion

        #region Properties

        public virtual int Width
        {
            get
            {
                return mewImage?.PixelWidth ?? 0;
            }
        }

        public virtual int Height
        {
            get
            {
                return mewImage?.PixelHeight ?? 0;
            }
        }

        public virtual float HorizontalResolution
        {
            get => horizontalResolution;
            set => horizontalResolution = value;
        }

        public virtual float VerticalResolution
        {
            get => verticalResolution;
            set => verticalResolution = value;
        }

        #endregion

        #region Save Methods

        public virtual void Save(string filename, ImageFormat format, int quality = 100)
        {
            throw new NotImplementedException("Save is not implemented for MewUI Image");
        }

        public virtual void Save(Stream stream, ImageFormat format, int quality = 100)
        {
            throw new NotImplementedException("Save is not implemented for MewUI Image");
        }

        #endregion

        #region Pixel Format Size

        /// <summary>
        /// Returns the bit depth (bits per pixel) for the specified PixelFormat.
        /// You can expand or modify as needed for additional formats.
        /// </summary>
        public static int GetPixelFormatSize(PixelFormat p)
        {
            switch (p)
            {
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    return 32;

                case PixelFormat.Format24bppRgb:
                    return 24;

                case PixelFormat.Format16bppRgb565:
                    return 16;

                case PixelFormat.Format8bppGray:
                    return 8;

                // Add or adjust any additional pixel formats you support:
                // case PixelFormat.Format48bppRgb: return 48; etc.

                default:
                    throw new NotSupportedException($"Unsupported or unknown pixel format {p}.");
            }
        }

        #endregion

        #region Disposal

        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
                if (mewImage != null)
                {
                    mewImage.Dispose();
                    mewImage = null;
                }
            }
        }

        #endregion

        #region Utilities

        public virtual IImage ToMewImage()
        {
            return mewImage;
        }

        public override string ToString()
        {
            if (disposed) return "Image [Disposed]";
            return $"Image [Width={Width}, Height={Height}, DPI={HorizontalResolution}x{VerticalResolution}]";
        }

        #endregion
    }
}
