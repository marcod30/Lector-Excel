using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Lógica de interacción para ImportSettings.xaml
    /// </summary>
    public partial class ImportSettings : Window
    {
        public List<string> positions { get; set; }

        // Constructor
        public ImportSettings()
        {
            InitializeComponent();
            positions = new List<string>();
        }

        // Called after triggering this window's close event
        private void Window_Closed(object sender, EventArgs e)
        {
            positions.Clear();
            foreach(TextBox t in stack_text.Children.OfType<TextBox>())
            {
                if (t.IsEnabled)
                {
                    positions.Add(t.Text);
                    Debug.WriteLine("Added " + t.Text);
                }
            }
            //this.DialogResult = true;
        }

        // Handles manual window closing
        private void Menu_CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
