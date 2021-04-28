using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Lector_Excel
{
    /// <summary>
    /// Ventana con una barra de progreso.
    /// </summary>
    public partial class ProgressWindow : Window
    {
        bool isIndeterminate;
        string title = "Exportando...";

        // Required for removing the close button
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        /// <summary>
        /// Inicializa una nueva instancia de <c>ProgressWindow</c>.
        /// </summary>
        /// <param name="isIndeterminate">True si la barra de progreso debe ser indeterminada.</param>
        /// <param name="title">Título de la ventana.</param>
        public ProgressWindow(bool isIndeterminate, string title)
        {
            InitializeComponent();
            this.isIndeterminate = isIndeterminate;
            this.title = title;
        }

        // Property that has progress
        /// <value>Obtiene o cambia el valor de la barra de progreso.</value>
        public int Amount
        {
            get { return Amount; }
            set { Amount = value; }
        }

        /// <summary>
        /// Función de evento de carga de la ventana.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Export_Progressbar.IsIndeterminate = isIndeterminate;
            if (isIndeterminate)
                txt_percentage.Visibility = Visibility.Hidden;
            this.Title = title;
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }
    }
}
