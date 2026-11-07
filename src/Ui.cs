using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CapiQR
{
    internal static class Palette
    {
        public static readonly Color Cream = Color.FromArgb(255, 248, 232);
        public static readonly Color Paper = Color.FromArgb(255, 253, 248);
        public static readonly Color Brown = Color.FromArgb(77, 54, 46);
        public static readonly Color BrownSoft = Color.FromArgb(116, 86, 70);
        public static readonly Color Capybara = Color.FromArgb(183, 126, 83);
        public static readonly Color CapybaraLight = Color.FromArgb(222, 177, 128);
        public static readonly Color Orange = Color.FromArgb(224, 137, 70);
        public static readonly Color OrangeHover = Color.FromArgb(238, 155, 85);
        public static readonly Color Green = Color.FromArgb(76, 119, 84);
        public static readonly Color Border = Color.FromArgb(231, 215, 192);
        public static readonly Color Muted = Color.FromArgb(126, 108, 97);
    }

    internal sealed class RoundedButton : Button
    {
        public Color ButtonColor { get; set; }
        public Color HoverColor { get; set; }
        public int Radius { get; set; }
        private bool _hover;

        public RoundedButton()
        {
            ButtonColor = Palette.Orange;
            HoverColor = Palette.OrangeHover;
            Radius = 14;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            ForeColor = Color.White;
            Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            Cursor = Cursors.Hand;
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _hover = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _hover = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.Clear(Parent != null ? Parent.BackColor : Palette.Cream);
            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var path = CreateRoundedRectangle(rect, Radius))
            using (var brush = new SolidBrush(Enabled ? (_hover ? HoverColor : ButtonColor) : Color.FromArgb(190, 190, 190)))
                e.Graphics.FillPath(brush, path);

            TextRenderer.DrawText(
                e.Graphics,
                Text,
                Font,
                rect,
                Enabled ? ForeColor : Color.WhiteSmoke,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }

        internal static GraphicsPath CreateRoundedRectangle(Rectangle rect, int radius)
        {
            int diameter = Math.Max(2, radius * 2);
            var path = new GraphicsPath();
            path.AddArc(rect.Left, rect.Top, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Top, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.Left, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    internal sealed class CardPanel : Panel
    {
        public int Radius { get; set; }

        public CardPanel()
        {
            Radius = 18;
            BackColor = Palette.Paper;
            DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var rect = new Rectangle(1, 1, Width - 3, Height - 3);
            using (var path = RoundedButton.CreateRoundedRectangle(rect, Radius))
            using (var fill = new SolidBrush(BackColor))
            using (var pen = new Pen(Palette.Border, 1f))
            {
                e.Graphics.FillPath(fill, path);
                e.Graphics.DrawPath(pen, path);
            }
            base.OnPaint(e);
        }
    }

    internal sealed class CapybaraControl : Control
    {
        public CapybaraControl()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            BackColor = Color.Transparent;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawCapybara(e.Graphics, ClientRectangle);
        }

        public static void DrawCapybara(Graphics graphics, Rectangle bounds)
        {
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            float scale = Math.Min(bounds.Width / 120f, bounds.Height / 90f);
            if (scale <= 0)
                return;

            var state = graphics.Save();
            graphics.TranslateTransform(bounds.Left + (bounds.Width - 120f * scale) / 2f, bounds.Top + (bounds.Height - 90f * scale) / 2f);
            graphics.ScaleTransform(scale, scale);

            using (var body = new SolidBrush(Palette.Capybara))
            using (var light = new SolidBrush(Palette.CapybaraLight))
            using (var dark = new SolidBrush(Palette.Brown))
            using (var white = new SolidBrush(Color.White))
            using (var orange = new SolidBrush(Palette.Orange))
            {
                graphics.FillEllipse(body, 15, 32, 89, 50);
                graphics.FillEllipse(body, 53, 11, 60, 59);
                graphics.FillEllipse(body, 57, 5, 18, 19);
                graphics.FillEllipse(body, 89, 7, 17, 19);
                graphics.FillEllipse(light, 80, 44, 35, 24);
                graphics.FillEllipse(white, 75, 29, 9, 9);
                graphics.FillEllipse(dark, 78, 32, 4, 4);
                graphics.FillEllipse(dark, 106, 47, 5, 5);
                graphics.FillEllipse(dark, 34, 76, 15, 7);
                graphics.FillEllipse(dark, 75, 75, 15, 7);
                graphics.FillEllipse(orange, 10, 33, 15, 15);
                graphics.FillEllipse(orange, 6, 27, 6, 6);
            }
            graphics.Restore(state);
        }
    }

    internal static class IconFactory
    {
        public static Icon CreateAppIcon()
        {
            using (var bitmap = new Bitmap(64, 64))
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(Color.Transparent);
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var brush = new SolidBrush(Palette.Cream))
                    graphics.FillEllipse(brush, 1, 1, 62, 62);
                CapybaraControl.DrawCapybara(graphics, new Rectangle(4, 9, 56, 45));

                IntPtr handle = bitmap.GetHicon();
                try
                {
                    using (var icon = Icon.FromHandle(handle))
                        return (Icon)icon.Clone();
                }
                finally
                {
                    NativeMethods.DestroyIcon(handle);
                }
            }
        }
    }
}
