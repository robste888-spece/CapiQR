using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace CapiQR
{
    internal static class Program
    {
        private const string MutexName = "Local\\CapiQR.SingleInstance.4B820A0D";

        [STAThread]
        private static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += ResolveEmbeddedAssembly;

            bool createdNew;
            using (var mutex = new Mutex(true, MutexName, out createdNew))
            {
                if (!createdNew)
                {
                    MessageBox.Show(
                        "CapiQR กำลังทำงานอยู่แล้ว ให้กด Ctrl + Shift + Q เพื่อสแกน QR",
                        "CapiQR",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                NativeMethods.SetProcessDPIAware();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.ThreadException += OnThreadException;
                try
                {
                    Application.Run(new MainForm());
                }
                catch (Exception ex)
                {
                    WriteErrorLog(ex);
                    MessageBox.Show(
                        "CapiQR เปิดไม่สำเร็จ\n\n" + ex.Message,
                        "CapiQR",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                GC.KeepAlive(mutex);
            }
        }

        private static Assembly ResolveEmbeddedAssembly(object sender, ResolveEventArgs args)
        {
            var requested = new AssemblyName(args.Name);
            if (!string.Equals(requested.Name, "zxing", StringComparison.OrdinalIgnoreCase))
                return null;

            var current = Assembly.GetExecutingAssembly();
            using (var stream = current.GetManifestResourceStream("CapiQR.Dependencies.zxing.dll"))
            {
                if (stream == null)
                    return null;

                var bytes = new byte[stream.Length];
                int offset = 0;
                while (offset < bytes.Length)
                {
                    int read = stream.Read(bytes, offset, bytes.Length - offset);
                    if (read == 0)
                        break;
                    offset += read;
                }
                return Assembly.Load(bytes);
            }
        }

        private static void OnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            string logPath = WriteErrorLog(e.Exception);
            MessageBox.Show(
                "CapiQR พบข้อผิดพลาด\n\n" + e.Exception.Message + "\n\nLog: " + logPath,
                "CapiQR",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        private static string WriteErrorLog(Exception exception)
        {
            var logPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "CapiQR",
                "error.log");

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(logPath));
                File.AppendAllText(logPath, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + Environment.NewLine + exception + Environment.NewLine);
            }
            catch
            {
                // Logging must never hide the original error dialog.
            }
            return logPath;
        }
    }
}
