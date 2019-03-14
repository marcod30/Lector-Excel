using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Lector_Excel
{
    /// <summary>
    /// Lógica de interacción para ProgressWindow.xaml
    /// </summary>
    public partial class ProgressWindow : Window
    {
        bool isIndeterminate;
        string title = "Exportando...";
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
        }
    }
}
