using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HgLBrowser.security
{
    public class TaskbarManager
    {
        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static void HideTaskbar()
        {
            IntPtr taskBarHandle = FindWindow("Shell_TrayWnd", null);
            ShowWindow(taskBarHandle, SW_HIDE);
        }

        public static void ShowTaskbar()
        {
            IntPtr taskBarHandle = FindWindow("Shell_TrayWnd", null);
            ShowWindow(taskBarHandle, SW_SHOW);
        }
    }
}
