using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using ZXing;
using ZXing.Common;

namespace CapiQR
{
    internal static class QrDecoder
    {
        public static Result Decode(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            var reader = new BarcodeReader();
            reader.AutoRotate = true;
            reader.Options = new DecodingOptions
            {
                TryHarder = true,
                TryInverted = true,
                PossibleFormats = new List<BarcodeFormat> { BarcodeFormat.QR_CODE }
            };

            var result = reader.Decode(bitmap);
            if (result != null)
                return result;

            int scale = Math.Min(bitmap.Width, bitmap.Height) < 350 ? 3 : 2;
            using (var enlarged = Resize(bitmap, bitmap.Width * scale, bitmap.Height * scale))
            {
                result = reader.Decode(enlarged);
                if (result != null)
                    return result;

                using (var highContrast = ToHighContrast(enlarged))
                    return reader.Decode(highContrast);
            }
        }

        private static Bitmap Resize(Bitmap source, int width, int height)
        {
            var output = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            output.SetResolution(96, 96);
            using (var graphics = Graphics.FromImage(output))
            {
                graphics.Clear(Color.White);
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphics.PixelOffsetMode = PixelOffsetMode.Half;
                graphics.DrawImage(source, new Rectangle(0, 0, width, height), 0, 0, source.Width, source.Height, GraphicsUnit.Pixel);
            }
            return output;
        }

        private static Bitmap ToHighContrast(Bitmap source)
        {
            var output = new Bitmap(source.Width, source.Height, PixelFormat.Format24bppRgb);
            using (var graphics = Graphics.FromImage(output))
            using (var attributes = new ImageAttributes())
            {
                var matrix = new ColorMatrix(new[]
                {
                    new[] { 1.7f, 0f, 0f, 0f, 0f },
                    new[] { 0f, 1.7f, 0f, 0f, 0f },
                    new[] { 0f, 0f, 1.7f, 0f, 0f },
                    new[] { 0f, 0f, 0f, 1f, 0f },
                    new[] { -0.35f, -0.35f, -0.35f, 0f, 1f }
                });
                attributes.SetColorMatrix(matrix);
                graphics.DrawImage(source, new Rectangle(0, 0, output.Width, output.Height), 0, 0, source.Width, source.Height, GraphicsUnit.Pixel, attributes);
            }
            return output;
        }
    }
}
