using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace CapiQR
{
    internal static class AppSettings
    {
        private const string RunKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private const string RunValueName = "CapiQR";

        public static bool StartsWithWindows
        {
            get
            {
                using (var key = Registry.CurrentUser.OpenSubKey(RunKeyPath, false))
                    return key != null && key.GetValue(RunValueName) != null;
            }
            set
            {
                using (var key = Registry.CurrentUser.OpenSubKey(RunKeyPath, true))
                {
                    if (key == null)
                        throw new InvalidOperationException("ไม่สามารถเปิด Windows startup settings ได้");

                    if (value)
                        key.SetValue(RunValueName, "\"" + Process.GetCurrentProcess().MainModule.FileName + "\" --startup");
                    else
                        key.DeleteValue(RunValueName, false);
                }
            }
        }

        public static string FindChrome()
        {
            var paths = new[]
            {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Google", "Chrome", "Application", "chrome.exe"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Google", "Chrome", "Application", "chrome.exe"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Google", "Chrome", "Application", "chrome.exe")
            };

            foreach (var path in paths)
            {
                if (File.Exists(path))
                    return path;
            }
            return null;
        }
    }
}
