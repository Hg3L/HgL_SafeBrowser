using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HgLBrowser.security
{
    public class ScreenSharingDetector
    {
        private static readonly string[] SuspiciousProcesses =
        {
            "Teams", "Zoom", "Skype", "Discord", "obs64", "obs32", "CamtasiaStudio",
            "AnyDesk", "TeamViewer", "Webex", "GoogleMeet", "Slack", "BlueJeans"
        };

        public static bool IsSuspiciousProcessRunning()
        {
            try
            {
                var runningProcesses = Process.GetProcesses();

                foreach (var process in runningProcesses)
                {
                    string processName = process.ProcessName.ToLower();

                    if (SuspiciousProcesses.Any(name => processName.Equals(name.ToLower())))
                    {
                        process.Dispose(); 
                        return true; 
                    }
                    process.Dispose(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error detecting processes: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return false; 
        }

        public static void ShowWarningIfSuspicious()
        {
            if (IsSuspiciousProcessRunning())
            {
                MessageBox.Show("Phát hiện ứng dụng chia sẻ màn hình hoặc video call đang chạy. Hãy đóng tất cả các ứng dụng đó trước khi tiếp tục!",
                                "Cảnh báo bảo mật", MessageBoxButton.OK, MessageBoxImage.Warning);

                Application.Current.Shutdown();
            }
        }
    }
}
