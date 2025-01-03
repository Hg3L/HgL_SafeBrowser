using HgLBrowser.security;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HgL_SafeBrowser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TaskbarManager.HideTaskbar();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            KeyboardBlocker.Start(); // Kích hoạt chặn phím
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            KeyboardBlocker.Stop(); // Tắt chặn phím
            TaskbarManager.ShowTaskbar(); // Hiển thị lại Taskbar
        }

        // kiểm tra share màn hình
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            ScreenSharingDetector.ShowWarningIfSuspicious();
        }

        // Chặn các phím tắt
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // 1. Chặn phím chức năng F1 - F12
            if (e.Key >= Key.F1 && e.Key <= Key.F12)
            {
                e.Handled = true;
            }

            // 2. Chặn tổ hợp phím Ctrl + ...
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                switch (e.Key)
                {
                    case Key.C: // Copy
                    case Key.V: // Paste
                    case Key.X: // Cut
                    case Key.A: // Select All
                    case Key.P: // Print
                    case Key.S: // Save
                    case Key.Escape: // ESC
                        e.Handled = true;
                        break;
                }
            }

            // 3. Chặn Ctrl + Shift + Esc (Mở Task Manager)
            if (Keyboard.Modifiers == (ModifierKeys.Control | ModifierKeys.Shift) && e.Key == Key.Escape)
            {
                e.Handled = true;
            }

            // 4. Chặn chuột phải (phím menu)
            if (e.Key == Key.Apps)
            {
                e.Handled = true;
            }
        }
    }
}