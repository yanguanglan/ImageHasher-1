namespace ImageHasher.Common
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;

    public static class ImageUtils
    {
        /// <summary>
        /// Quickly reduce the size of an image
        /// </summary>
        public static Bitmap ReduceImage(Image image, int width, int height)
        {
            // A standard RGB image in jpeg format has 24-bits per pixel
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            using (Graphics canvas = Graphics.FromImage(bmp))
            {
                // Optimize rendering
                canvas.InterpolationMode = InterpolationMode.Low;
                canvas.SmoothingMode = SmoothingMode.HighSpeed;
                canvas.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                canvas.CompositingQuality = CompositingQuality.HighSpeed;

                canvas.DrawImage(image, 0, 0, width, height);
            }

            return bmp;
        }

        /// <summary>
        /// Grayscale an image
        /// </summary>
        public static Bitmap GrayscaleImage(Bitmap bmp)
        {
            Bitmap result = new Bitmap(
                bmp.Width, bmp.Height, PixelFormat.Format24bppRgb);

            byte[] bytes = GetGrayscaleBytes(bmp);

            int curr = 0;

            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    byte grayscale = bytes[curr];

                    Color gray = Color.FromArgb(grayscale, grayscale, grayscale);

                    result.SetPixel(x, y, gray);

                    curr++;
                }
            }

            return result;
        }

        /// <summary>
        /// Get the grayscale pixel values for an image
        /// </summary>
        public static byte[] GetGrayscaleBytes(Bitmap bmp)
        {
            byte[] bytes = new byte[bmp.Height * bmp.Width];

            int pos = 0;

            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    Color pixel = bmp.GetPixel(x, y);

                    byte grayscale = (byte)((pixel.R * 0.3)
                                          + (pixel.G * 0.59)
                                          + (pixel.B * 0.11));

                    bytes[pos] = grayscale;

                    pos++;
                }
            }

            return bytes;
        }
    }
}