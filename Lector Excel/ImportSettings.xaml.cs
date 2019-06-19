using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ComponentModel;
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

        // Handles manual window closing, without saving changes
        private void Menu_CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        // Handles manual window closing and saves changes
        private void Menu_ConfirmAndClose_Click(object sender, RoutedEventArgs e)
        {
            positions.Clear();
            foreach (TextBox t in stack_text.Children.OfType<TextBox>())
            {
                if (t.IsEnabled)
                {
                    positions.Add(t.Text.ToUpper());
                    Debug.WriteLine("Added " + t.Text);
                }
            }

            if (positions.Count() != positions.Distinct().Count())   // Check if there are duplicates
            {
                MessageBox.Show("Parece que ha introducido valores duplicados. Por favor, revise los campos e inténtelo de nuevo", "Valores repetidos", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (!positions.All(s => !s.Equals("")))  // Check if any field was empty
            {
                MessageBox.Show("Parece que ha dejado algún campo vacío. Por favor, revise los campos e inténtelo de nuevo", "Campo vacío", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            this.DialogResult = true;
            this.Close();
        }

        // Called after window is fully loaded
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(positions != null && positions.Count > 0)
            {
                int i = 0;
                foreach (TextBox t in stack_text.Children.OfType<TextBox>())
                {
                    if (t.IsEnabled)
                    {
                        t.Text = positions.ElementAt(i++);
                    }
                }

                positions.Clear();
            }
        }
    }
}
