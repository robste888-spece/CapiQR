using System;
using System.Diagnostics;

namespace CapiQR
{
    internal static class BrowserLauncher
    {
        public static bool TryOpenHttps(string url, out string errorMessage)
        {
            errorMessage = null;
            Uri uri;
            if (!Uri.TryCreate(url, UriKind.Absolute, out uri) || uri.Scheme != Uri.UriSchemeHttps)
            {
                errorMessage = "ลิงก์ดาวน์โหลดไม่ปลอดภัยหรือไม่ถูกต้อง";
                return false;
            }

            try
            {
                string chrome = AppSettings.FindChrome();
                var info = chrome == null
                    ? new ProcessStartInfo(uri.AbsoluteUri)
                    : new ProcessStartInfo(chrome, "\"" + uri.AbsoluteUri.Replace("\"", string.Empty) + "\"");
                info.UseShellExecute = true;
                Process.Start(info);
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }
    }
}
