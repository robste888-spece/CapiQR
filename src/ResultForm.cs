using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace CapiQR
{
    internal sealed class ResultForm : Form
    {
        private readonly string _rawText;
        private readonly Uri _safeUri;
        private readonly TextBox _resultBox;
        private readonly RoundedButton _openButton;

        public ResultForm(string text)
        {
            _rawText = text ?? string.Empty;
            Uri parsed;
            if (Uri.TryCreate(_rawText.Trim(), UriKind.Absolute, out parsed) &&
                (parsed.Scheme == Uri.UriSchemeHttp || parsed.Scheme == Uri.UriSchemeHttps))
                _safeUri = parsed;

            Text = "CapiQR — พบ QR แล้ว";
            Icon = IconFactory.CreateAppIcon();
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(530, 330);
            BackColor = Palette.Cream;
            Font = new Font("Segoe UI", 9.5f);
            AutoScaleMode = AutoScaleMode.Dpi;

            var mascot = new CapybaraControl { Location = new Point(24, 17), Size = new Size(82, 64) };
            Controls.Add(mascot);

            var title = new Label
            {
                Text = _safeUri != null ? "เจอลิงก์แล้ว!" : "อ่าน QR สำเร็จ",
                Font = new Font("Segoe UI Semibold", 18f, FontStyle.Bold),
                ForeColor = Palette.Brown,
                AutoSize = true,
                Location = new Point(112, 21)
            };
            Controls.Add(title);

            var subtitle = new Label
            {
                Text = _safeUri != null ? "ตรวจสอบปลายทางก่อนเปิดทุกครั้ง" : "QR นี้ไม่ใช่เว็บลิงก์ จึงเปิด Chrome ไม่ได้",
                ForeColor = Palette.Muted,
                AutoSize = true,
                Location = new Point(115, 57)
            };
            Controls.Add(subtitle);

            _resultBox = new TextBox
            {
                Text = _rawText,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                ForeColor = Palette.Brown,
                Font = new Font("Segoe UI", 10.5f),
                Location = new Point(25, 95),
                Size = new Size(480, 122)
            };
            Controls.Add(_resultBox);

            var security = new Label
            {
                Text = _safeUri != null ? "✓ อนุญาตเฉพาะ http / https" : "⚠ ปิดการเปิดอัตโนมัติเพื่อความปลอดภัย",
                ForeColor = _safeUri != null ? Palette.Green : Palette.Orange,
                Font = new Font("Segoe UI Semibold", 9f),
                AutoSize = true,
                Location = new Point(25, 229)
            };
            Controls.Add(security);

            var copyButton = new RoundedButton
            {
                Text = "คัดลอก",
                ButtonColor = Palette.BrownSoft,
                HoverColor = Palette.Brown,
                Location = new Point(25, 266),
                Size = new Size(130, 43)
            };
            copyButton.Click += CopyClicked;
            Controls.Add(copyButton);

            _openButton = new RoundedButton
            {
                Text = "เปิดใน Chrome",
                ButtonColor = Palette.Green,
                HoverColor = Color.FromArgb(91, 139, 99),
                Location = new Point(165, 266),
                Size = new Size(210, 43),
                Enabled = _safeUri != null
            };
            _openButton.Click += OpenClicked;
            Controls.Add(_openButton);

            var closeButton = new RoundedButton
            {
                Text = "ปิด",
                ButtonColor = Palette.CapybaraLight,
                HoverColor = Palette.Capybara,
                ForeColor = Palette.Brown,
                Location = new Point(385, 266),
                Size = new Size(120, 43)
            };
            closeButton.Click += delegate { Close(); };
            Controls.Add(closeButton);

            AcceptButton = _openButton.Enabled ? _openButton : copyButton;
            CancelButton = closeButton;
        }

        private void CopyClicked(object sender, EventArgs e)
        {
            Clipboard.SetText(_rawText);
            ((Button)sender).Text = "คัดลอกแล้ว ✓";
        }

        private void OpenClicked(object sender, EventArgs e)
        {
            if (_safeUri == null)
                return;

            var chrome = AppSettings.FindChrome();
            if (chrome == null)
            {
                MessageBox.Show("ไม่พบ Google Chrome ในเครื่อง\nลิงก์ถูกคัดลอกไว้ให้แล้ว", "CapiQR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Clipboard.SetText(_safeUri.AbsoluteUri);
                return;
            }

            var info = new ProcessStartInfo(chrome, "\"" + _safeUri.AbsoluteUri.Replace("\"", string.Empty) + "\"");
            info.UseShellExecute = true;
            Process.Start(info);
            Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && Icon != null)
                Icon.Dispose();
            base.Dispose(disposing);
        }
    }
}
