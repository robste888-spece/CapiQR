using System;
using System.Drawing;
using ZXing;

namespace CapiQR
{
    internal static class DecodeQrTest
    {
        private static int Main(string[] args)
        {
            if (args.Length != 5)
            {
                Console.Error.WriteLine("Usage: DecodeQrTest <image> <x> <y> <width> <height>");
                return 2;
            }

            using (var source = new Bitmap(args[0]))
            using (var crop = source.Clone(new Rectangle(int.Parse(args[1]), int.Parse(args[2]), int.Parse(args[3]), int.Parse(args[4])), source.PixelFormat))
            {
                Result result = QrDecoder.Decode(crop);
                if (result == null)
                {
                    Console.Error.WriteLine("QR_NOT_FOUND");
                    return 1;
                }
                Console.WriteLine(result.BarcodeFormat);
                Console.WriteLine(result.Text);
                return 0;
            }
        }
    }
}
