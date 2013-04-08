using System;
using System.IO;
using System.Runtime.InteropServices;

namespace GridPatternLibrary.Support
{
    public static class Functions
    {
        public const string DataFileName = "ea.xml";

        public static string GetAppDataFileName(string appName)
        {
            return Path.Combine(GetAppDataDirectory(appName), DataFileName);
        }

        public static string GetAppDataDirectory(string appName)
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                    appName);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }


        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint RegisterWindowMessage(string lpString);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(HandleRef hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
    }
}