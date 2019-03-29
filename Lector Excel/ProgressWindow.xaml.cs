using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Lector_Excel
{
    /// <summary>
    /// Lógica de interacción para ProgressWindow.xaml
    /// </summary>
    public partial class ProgressWindow : Window
    {
        bool isIndeterminate;
        string title = "Exportando...";

        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        public ProgressWindow(bool isIndeterminate, string title)
        {
            InitializeComponent();
            this.isIndeterminate = isIndeterminate;
            this.title = title;
        }

        // Property that has progress
        public int Amount
        {
            get { return Amount; }
            set { Amount = value; }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Export_Progressbar.IsIndeterminate = isIndeterminate;
            this.Title = title;
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }
    }
}
