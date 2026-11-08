using System;
using System.Drawing;
using System.Windows.Forms;

namespace CapiQR
{
    internal sealed class UpdateAvailableForm : Form
    {
        private readonly string _releaseUrl;

        public UpdateAvailableForm(string currentVersion, string latestVersion, string releaseUrl)
        {
            _releaseUrl = releaseUrl;
            Text = "CapiQR — มีอัปเดตใหม่";
            Icon = IconFactory.CreateAppIcon();
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(500, 315);
            BackColor = Palette.Cream;
            Font = new Font("Segoe UI", 9.5f);
            AutoScaleMode = AutoScaleMode.Dpi;

            var mascot = new CapybaraControl { Location = new Point(25, 18), Size = new Size(90, 70) };
            Controls.Add(mascot);

            var title = new Label
            {
                Text = "มี CapiQR รุ่นใหม่!",
                Font = new Font("Segoe UI Semibold", 19f, FontStyle.Bold),
                ForeColor = Palette.Brown,
                AutoSize = true,
                Location = new Point(124, 23)
            };
            Controls.Add(title);

            var subtitle = new Label
            {
                Text = "อัปเดตเพื่อรับ Function และการแก้ไขล่าสุด",
                ForeColor = Palette.Muted,
                AutoSize = true,
                Location = new Point(127, 63)
            };
            Controls.Add(subtitle);

            var versionCard = new CardPanel { Location = new Point(25, 103), Size = new Size(450, 96) };
            Controls.Add(versionCard);

            var currentLabel = new Label
            {
                Text = "รุ่นในเครื่อง\n" + currentVersion,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Palette.Muted,
                Font = new Font("Segoe UI Semibold", 10f),
                Location = new Point(25, 17),
                Size = new Size(150, 62)
            };
            versionCard.Controls.Add(currentLabel);

            var arrow = new Label
            {
                Text = "→",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI Semibold", 20f, FontStyle.Bold),
                ForeColor = Palette.Orange,
                Location = new Point(185, 22),
                Size = new Size(70, 50)
            };
            versionCard.Controls.Add(arrow);

            var latestLabel = new Label
            {
                Text = "รุ่นล่าสุด\n" + latestVersion,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Palette.Green,
                Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold),
                Location = new Point(265, 17),
                Size = new Size(155, 62)
            };
            versionCard.Controls.Add(latestLabel);

            var security = new Label
            {
                Text = "ปลอดภัย: เปิดหน้า GitHub Release ให้ตรวจสอบและดาวน์โหลดด้วยตัวเอง",
                ForeColor = Palette.Green,
                AutoSize = true,
                Location = new Point(31, 215)
            };
            Controls.Add(security);

            var downloadButton = new RoundedButton
            {
                Text = "เปิดหน้าดาวน์โหลด",
                ButtonColor = Palette.Green,
                HoverColor = Color.FromArgb(91, 139, 99),
                Location = new Point(25, 253),
                Size = new Size(285, 43)
            };
            downloadButton.Click += OpenDownloadPage;
            Controls.Add(downloadButton);

            var laterButton = new RoundedButton
            {
                Text = "ไว้ภายหลัง",
                ButtonColor = Palette.CapybaraLight,
                HoverColor = Palette.Capybara,
                ForeColor = Palette.Brown,
                Location = new Point(320, 253),
                Size = new Size(155, 43)
            };
            laterButton.Click += delegate { Close(); };
            Controls.Add(laterButton);

            AcceptButton = downloadButton;
            CancelButton = laterButton;
        }

        private void OpenDownloadPage(object sender, EventArgs e)
        {
            string error;
            if (BrowserLauncher.TryOpenHttps(_releaseUrl, out error))
            {
                Close();
                return;
            }

            try { Clipboard.SetText(_releaseUrl); }
            catch { }
            MessageBox.Show(
                "เปิด Browser ไม่สำเร็จ แต่คัดลอกลิงก์ไว้ให้แล้ว\n\n" + error,
                "CapiQR",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && Icon != null)
                Icon.Dispose();
            base.Dispose(disposing);
        }
    }
}
