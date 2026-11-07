using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;

internal static class GenerateIcon
{
    [DllImport("user32.dll")]
    private static extern bool DestroyIcon(IntPtr handle);

    private static void Main(string[] args)
    {
        if (args.Length != 1)
            throw new ArgumentException("Output .ico path required");

        using (var bitmap = new Bitmap(64, 64))
        using (var graphics = Graphics.FromImage(bitmap))
        {
            graphics.Clear(Color.Transparent);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var cream = new SolidBrush(Color.FromArgb(255, 248, 232)))
            using (var body = new SolidBrush(Color.FromArgb(183, 126, 83)))
            using (var muzzle = new SolidBrush(Color.FromArgb(222, 177, 128)))
            using (var dark = new SolidBrush(Color.FromArgb(77, 54, 46)))
            using (var orange = new SolidBrush(Color.FromArgb(224, 137, 70)))
            using (var white = new SolidBrush(Color.White))
            {
                graphics.FillEllipse(cream, 1, 1, 62, 62);
                graphics.FillEllipse(body, 9, 26, 43, 26);
                graphics.FillEllipse(body, 27, 13, 31, 32);
                graphics.FillEllipse(body, 29, 9, 10, 11);
                graphics.FillEllipse(body, 46, 10, 9, 11);
                graphics.FillEllipse(muzzle, 42, 31, 18, 13);
                graphics.FillEllipse(white, 39, 23, 5, 5);
                graphics.FillEllipse(dark, 41, 25, 2, 2);
                graphics.FillEllipse(dark, 55, 33, 3, 3);
                graphics.FillEllipse(orange, 6, 26, 9, 9);
                graphics.FillEllipse(dark, 18, 48, 8, 4);
                graphics.FillEllipse(dark, 39, 48, 8, 4);
            }

            IntPtr handle = bitmap.GetHicon();
            try
            {
                using (var icon = Icon.FromHandle(handle))
                using (var output = File.Create(args[0]))
                    icon.Save(output);
            }
            finally
            {
                DestroyIcon(handle);
            }
        }
    }
}
