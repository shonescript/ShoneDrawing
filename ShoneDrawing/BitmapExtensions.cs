using System;
using Aprillz.MewUI;
using Aprillz.MewUI.Rendering;

namespace ShoneDrawing
{
    public static class BitmapExtensions
    {
        public static void RotateFlip(this Bitmap bmp, RotateFlipType rft)
        {
            if (bmp == null)
                throw new ArgumentNullException(nameof(bmp));

            // Extract rotation (0..3) from the lower 2 bits
            // and flip (0..3) from the upper bits
            int rotateIndex = (int)rft % 4;  // 0=none, 1=90, 2=180, 3=270
            int flipIndex = (int)rft / 4;    // 0=none, 1=flipX, 2=flipY, 3=flipXY

            // Original width/height
            int oldW = bmp.Width;
            int oldH = bmp.Height;

            if (oldW <= 0 || oldH <= 0)
                return; // Nothing to do

            // Determine final width/height after rotation
            int newW = oldW;
            int newH = oldH;

            // If rotating 90 or 270, we swap width/height
            if (rotateIndex == 1 || rotateIndex == 3)
            {
                newW = oldH;
                newH = oldW;
            }

            // Create a new bitmap with the final dimensions
            var newBitmap = new Bitmap(newW, newH);
            var graphicsFactory = Aprillz.MewUI.Application.DefaultGraphicsFactory;
            using (var context = graphicsFactory.CreateContext(newBitmap.ToRenderTarget()))
            {
                // Apply transformations
                // 1) Rotation
                switch (rotateIndex)
                {
                    case 0: // none
                        break;
                    case 1: // 90
                        context.Translate(newW, 0); // shift right
                        context.Rotate(Math.PI / 2);
                        break;
                    case 2: // 180
                        context.Translate(newW, newH);
                        context.Rotate(Math.PI);
                        break;
                    case 3: // 270
                        context.Translate(0, newH);
                        context.Rotate(3 * Math.PI / 2);
                        break;
                }

                // 2) Flip
                switch (flipIndex)
                {
                    case 0: // none
                        break;
                    case 1: // flip X (horizontal)
                        context.Scale(-1, 1);
                        context.Translate(-newW, 0);
                        break;
                    case 2: // flip Y (vertical)
                        context.Scale(1, -1);
                        context.Translate(0, -newH);
                        break;
                    case 3: // flip XY
                        context.Scale(-1, -1);
                        context.Translate(-newW, -newH);
                        break;
                }

                // Draw the old bitmap at (0,0) after the transformations
                context.DrawImage(bmp.ToMewImage(), new Aprillz.MewUI.Point(0, 0));
            }

            // Replace the old image in 'bmp' with the new one
            bmp.Dispose(); // Dispose old references
            // Now re-construct 'bmp' from the new bitmap
            bmp.ReplaceInternalImage(newBitmap.ToMewImage(), newBitmap.ToRenderTarget());
        }

        /// <summary>
        /// Helper method to replace the internal image in the existing Bitmap with a new one.
        /// </summary>
        private static void ReplaceInternalImage(this Bitmap bmp, IImage newImage, IRenderTarget newRenderTarget)
        {
            // We'll reassign the internal fields using reflection
            var imageField = typeof(Bitmap).GetField("image", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var renderTargetField = typeof(Bitmap).GetField("renderTarget", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (imageField == null || renderTargetField == null)
                throw new Exception("Could not find internal fields 'image' or 'renderTarget'.");

            imageField.SetValue(bmp, newImage);
            renderTargetField.SetValue(bmp, newRenderTarget);
        }
    }
}
