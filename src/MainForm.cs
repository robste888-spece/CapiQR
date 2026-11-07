using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CapiQR
{
    internal sealed class MainForm : Form
    {
        private const int HotkeyId = 0x4341;
        private NotifyIcon _trayIcon;
        private Icon _appIcon;
        private Label _hotkeyLabel;
        private bool _hotkeyRegistered;
        private bool _isScanning;
        private bool _exiting;
        private string _hotkeyText = "Ctrl + Shift + Q";

        public MainForm()
        {
            Text = "CapiQR — QR Link Scanner";
            ClientSize = new Size(510, 545);
            MinimumSize = new Size(526, 584);
            MaximumSize = new Size(526, 584);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Palette.Cream;
            Font = new Font("Segoe UI", 9.5f);
            AutoScaleMode = AutoScaleMode.Dpi;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            _appIcon = IconFactory.CreateAppIcon();
            Icon = _appIcon;
            BuildInterface();
            BuildTrayIcon();
        }

        private void BuildInterface()
        {
            var mascot = new CapybaraControl { Location = new Point(29, 17), Size = new Size(105, 82) };
            Controls.Add(mascot);

            var brand = new Label
            {
                Text = "CapiQR",
                Font = new Font("Segoe UI Semibold", 25f, FontStyle.Bold),
                ForeColor = Palette.Brown,
                AutoSize = true,
                Location = new Point(145, 25)
            };
            Controls.Add(brand);

            var tagline = new Label
            {
                Text = "กด ลาก เปิด — QR บนจอ ไม่ต้องใช้มือถือ",
                ForeColor = Palette.Muted,
                AutoSize = true,
                Location = new Point(149, 69)
            };
            Controls.Add(tagline);

            var scanCard = new CardPanel { Location = new Point(24, 112), Size = new Size(462, 202) };
            Controls.Add(scanCard);

            var scanTitle = new Label
            {
                Text = "พร้อมสแกน QR บนหน้าจอ",
                Font = new Font("Segoe UI Semibold", 15f, FontStyle.Bold),
                ForeColor = Palette.Brown,
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(28, 22)
            };
            scanCard.Controls.Add(scanTitle);

            var scanHelp = new Label
            {
                Text = "เลื่อนเมาส์ไปจอที่มี QR แล้วกด Hotkey\nจากนั้นลากกรอบให้ครอบ QR Code",
                ForeColor = Palette.Muted,
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(30, 59)
            };
            scanCard.Controls.Add(scanHelp);

            var scanButton = new RoundedButton
            {
                Text = "สแกน QR ตอนนี้",
                Location = new Point(28, 116),
                Size = new Size(250, 58)
            };
            scanButton.Click += delegate { StartScan(); };
            scanCard.Controls.Add(scanButton);

            _hotkeyLabel = new Label
            {
                Text = "Hotkey\n" + _hotkeyText,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI Semibold", 10f),
                ForeColor = Palette.Brown,
                BackColor = Color.FromArgb(246, 231, 210),
                Location = new Point(292, 116),
                Size = new Size(140, 58)
            };
            scanCard.Controls.Add(_hotkeyLabel);

            var settingsCard = new CardPanel { Location = new Point(24, 329), Size = new Size(462, 142) };
            Controls.Add(settingsCard);

            var settingsTitle = new Label
            {
                Text = "ตั้งค่าการทำงาน",
                Font = new Font("Segoe UI Semibold", 12f, FontStyle.Bold),
                ForeColor = Palette.Brown,
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(28, 18)
            };
            settingsCard.Controls.Add(settingsTitle);

            var startupCheck = new CheckBox
            {
                Text = "เปิด CapiQR พร้อม Windows",
                Checked = AppSettings.StartsWithWindows,
                AutoSize = true,
                ForeColor = Palette.Brown,
                BackColor = Color.Transparent,
                Location = new Point(31, 55)
            };
            startupCheck.CheckedChanged += StartupChanged;
            settingsCard.Controls.Add(startupCheck);

            var privacy = new Label
            {
                Text = "🔒 ภาพและผล QR ประมวลผลในเครื่องเท่านั้น ไม่อัปโหลดออกอินเทอร์เน็ต",
                ForeColor = Palette.Green,
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(30, 91)
            };
            settingsCard.Controls.Add(privacy);

            var footer = new Label
            {
                Text = "ปิดหน้าต่าง = ซ่อนไว้ที่ System tray  •  คลิกขวาไอคอนเพื่อ Exit",
                ForeColor = Palette.Muted,
                AutoSize = true,
                Location = new Point(59, 499)
            };
            Controls.Add(footer);
        }

        private void BuildTrayIcon()
        {
            var menu = new ContextMenuStrip();
            var scanItem = new ToolStripMenuItem("สแกน QR   " + _hotkeyText);
            scanItem.Click += delegate { StartScan(); };
            menu.Items.Add(scanItem);

            var showItem = new ToolStripMenuItem("เปิด CapiQR");
            showItem.Click += delegate { ShowMainWindow(); };
            menu.Items.Add(showItem);
            menu.Items.Add(new ToolStripSeparator());

            var exitItem = new ToolStripMenuItem("Exit");
            exitItem.Click += delegate
            {
                _exiting = true;
                Close();
            };
            menu.Items.Add(exitItem);

            _trayIcon = new NotifyIcon
            {
                Text = "CapiQR — กด Ctrl + Shift + Q เพื่อสแกน",
                Icon = _appIcon,
                ContextMenuStrip = menu,
                Visible = true
            };
            _trayIcon.DoubleClick += delegate { ShowMainWindow(); };
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            RegisterHotkey();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (Environment.GetCommandLineArgs().Any(delegate(string arg) { return string.Equals(arg, "--startup", StringComparison.OrdinalIgnoreCase); }))
            {
                Hide();
                _trayIcon.ShowBalloonTip(2500, "CapiQR พร้อมแล้ว", "กด " + _hotkeyText + " เพื่อสแกน QR บนจอ", ToolTipIcon.Info);
            }
        }

        private void RegisterHotkey()
        {
            if (_hotkeyRegistered)
                return;

            _hotkeyText = "Ctrl + Shift + Q";
            _hotkeyRegistered = NativeMethods.RegisterHotKey(Handle, HotkeyId, NativeMethods.MOD_CONTROL | NativeMethods.MOD_SHIFT | NativeMethods.MOD_NOREPEAT, (uint)Keys.Q);
            if (!_hotkeyRegistered)
            {
                _hotkeyText = "Ctrl + Alt + Q";
                _hotkeyRegistered = NativeMethods.RegisterHotKey(Handle, HotkeyId, NativeMethods.MOD_CONTROL | NativeMethods.MOD_ALT | NativeMethods.MOD_NOREPEAT, (uint)Keys.Q);
            }

            if (_hotkeyLabel != null)
            {
                _hotkeyLabel.Text = _hotkeyRegistered ? "Hotkey\n" + _hotkeyText : "Hotkey ถูกใช้งาน\nกดปุ่ม Scan แทน";
                _hotkeyLabel.ForeColor = _hotkeyRegistered ? Palette.Brown : Color.Firebrick;
            }
            if (_trayIcon != null)
                _trayIcon.Text = _hotkeyRegistered ? "CapiQR — กด " + _hotkeyText + " เพื่อสแกน" : "CapiQR — Hotkey ไม่พร้อม";
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeMethods.WM_HOTKEY && m.WParam.ToInt32() == HotkeyId)
                BeginInvoke(new Action(StartScan));
            base.WndProc(ref m);
        }

        private void StartScan()
        {
            if (_isScanning)
                return;

            _isScanning = true;
            bool restoreMainWindow = Visible;
            Hide();

            var timer = new Timer { Interval = 180 };
            timer.Tick += delegate
            {
                timer.Stop();
                timer.Dispose();
                RunCapture(restoreMainWindow);
            };
            timer.Start();
        }

        private void RunCapture(bool restoreMainWindow)
        {
            try
            {
                using (var capture = new ScreenCaptureForm())
                {
                    if (capture.ShowDialog() == DialogResult.OK && capture.SelectedImage != null)
                    {
                        using (var selected = capture.SelectedImage)
                        {
                            var result = QrDecoder.Decode(selected);
                            if (result == null)
                            {
                                MessageBox.Show(
                                    "ไม่พบ QR Code ในพื้นที่ที่เลือก\n\nลองลากกรอบให้มีขอบขาวรอบ QR และไม่ติดข้อความมากเกินไป",
                                    "CapiQR",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                            }
                            else
                            {
                                using (var resultForm = new ResultForm(result.Text))
                                    resultForm.ShowDialog();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("สแกน QR ไม่สำเร็จ\n\n" + ex.Message, "CapiQR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isScanning = false;
                if (restoreMainWindow)
                    ShowMainWindow();
            }
        }

        private void StartupChanged(object sender, EventArgs e)
        {
            var checkbox = (CheckBox)sender;
            try
            {
                AppSettings.StartsWithWindows = checkbox.Checked;
            }
            catch (Exception ex)
            {
                checkbox.CheckedChanged -= StartupChanged;
                checkbox.Checked = !checkbox.Checked;
                checkbox.CheckedChanged += StartupChanged;
                MessageBox.Show(ex.Message, "CapiQR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ShowMainWindow()
        {
            Show();
            WindowState = FormWindowState.Normal;
            Activate();
            BringToFront();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!_exiting && e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
                _trayIcon.ShowBalloonTip(2000, "CapiQR ยังทำงานอยู่", "กด " + _hotkeyText + " เพื่อสแกน หรือคลิกขวาไอคอนเพื่อ Exit", ToolTipIcon.Info);
                return;
            }
            base.OnFormClosing(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_hotkeyRegistered && IsHandleCreated)
                    NativeMethods.UnregisterHotKey(Handle, HotkeyId);
                if (_trayIcon != null)
                {
                    _trayIcon.Visible = false;
                    _trayIcon.Dispose();
                }
                if (_appIcon != null)
                    _appIcon.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
