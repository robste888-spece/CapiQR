using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace CapiQR
{
    internal sealed class ScreenCaptureForm : Form
    {
        private readonly Bitmap _screenshot;
        private Point _start;
        private Point _current;
        private bool _selecting;

        public Bitmap SelectedImage { get; private set; }

        public ScreenCaptureForm()
        {
            var targetScreen = Screen.FromPoint(Cursor.Position).Bounds;
            _screenshot = new Bitmap(targetScreen.Width, targetScreen.Height, PixelFormat.Format32bppArgb);
            using (var graphics = Graphics.FromImage(_screenshot))
                graphics.CopyFromScreen(targetScreen.Left, targetScreen.Top, 0, 0, targetScreen.Size, CopyPixelOperation.SourceCopy);

            FormBorderStyle = FormBorderStyle.None;
            Text = "CapiQR — เลือกพื้นที่ QR";
            StartPosition = FormStartPosition.Manual;
            Bounds = targetScreen;
            ShowInTaskbar = true;
            TopMost = true;
            DoubleBuffered = true;
            KeyPreview = true;
            Cursor = Cursors.Cross;
            BackColor = Color.Black;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                _start = e.Location;
                _current = e.Location;
                _selecting = true;
                Capture = true;
                Invalidate();
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_selecting)
            {
                _current = ClampPoint(e.Location);
                Invalidate();
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (_selecting && e.Button == MouseButtons.Left)
            {
                Capture = false;
                _selecting = false;
                _current = ClampPoint(e.Location);
                var selection = Normalize(_start, _current);

                if (selection.Width >= 20 && selection.Height >= 20)
                {
                    SelectedImage = _screenshot.Clone(selection, PixelFormat.Format32bppArgb);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    System.Media.SystemSounds.Beep.Play();
                    Invalidate();
                }
            }
            base.OnMouseUp(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawImageUnscaled(_screenshot, 0, 0);
            using (var shade = new SolidBrush(Color.FromArgb(145, 31, 25, 22)))
                e.Graphics.FillRectangle(shade, ClientRectangle);

            if (_selecting)
            {
                var selection = Normalize(_start, _current);
                if (selection.Width > 0 && selection.Height > 0)
                {
                    var state = e.Graphics.Save();
                    e.Graphics.SetClip(selection);
                    e.Graphics.DrawImageUnscaled(_screenshot, 0, 0);
                    e.Graphics.Restore(state);
                    using (var pen = new Pen(Palette.Orange, 4f))
                    {
                        pen.Alignment = PenAlignment.Inset;
                        e.Graphics.DrawRectangle(pen, selection);
                    }
                }
            }

            DrawInstruction(e.Graphics);
        }

        private void DrawInstruction(Graphics graphics)
        {
            const int width = 430;
            const int height = 58;
            var rect = new Rectangle((ClientSize.Width - width) / 2, 24, width, height);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var path = RoundedButton.CreateRoundedRectangle(rect, 18))
            using (var brush = new SolidBrush(Color.FromArgb(238, 255, 248, 232)))
            using (var border = new Pen(Palette.Orange, 2f))
            {
                graphics.FillPath(brush, path);
                graphics.DrawPath(border, path);
            }
            TextRenderer.DrawText(graphics, "ลากกรอบครอบ QR Code  •  ESC / คลิกขวาเพื่อยกเลิก", new Font("Segoe UI Semibold", 11f), rect, Palette.Brown, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private Point ClampPoint(Point point)
        {
            return new Point(
                Math.Max(0, Math.Min(ClientSize.Width - 1, point.X)),
                Math.Max(0, Math.Min(ClientSize.Height - 1, point.Y)));
        }

        private static Rectangle Normalize(Point first, Point second)
        {
            int left = Math.Min(first.X, second.X);
            int top = Math.Min(first.Y, second.Y);
            int right = Math.Max(first.X, second.X);
            int bottom = Math.Max(first.Y, second.Y);
            return Rectangle.FromLTRB(left, top, right, bottom);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _screenshot.Dispose();
                if (SelectedImage != null && DialogResult != DialogResult.OK)
                    SelectedImage.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
