using Microsoft.Win32;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System;
using System.IO;

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
                    //Debug.WriteLine("Added " + t.Text);
                }
            }

            if (CheckForEmptyAndDuplicates())
                return;

            this.DialogResult = true;
            this.Close();
        }

        // Checks if there is any duplicate or empty fields. If there were any, returns true, otherwise returns false.
        private bool CheckForEmptyAndDuplicates()
        {
            if (positions.Count() != positions.Distinct().Count())   // Check if there are duplicates
            {
                MessageBox.Show("Parece que ha introducido valores duplicados. Por favor, revise los campos e inténtelo de nuevo", "Valores repetidos", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            else if (!positions.All(s => !s.Equals("")) || !positions.All(s => !s.Contains(" ")))  // Check if any field was empty or contains blank spaces
            {
                MessageBox.Show("Parece que ha dejado algún campo vacío. Por favor, revise los campos e inténtelo de nuevo", "Campo vacío", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }

            return false;
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

        // When called, resets all TextBoxes to their default values
        private void Menu_ResetDefault_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            foreach (TextBox t in stack_text.Children.OfType<TextBox>())
            {
                if (t.IsEnabled)
                {
                    t.Text = ((char)('A' + i++)).ToString().ToUpper();
                }
            }
        }

        // Handles opening file and writing in textboxes
        private void Menu_LoadFromFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "LectorExcel files (*.lectorexcel)|*.lectorexcel";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if(openFileDialog.ShowDialog() == true)
            {
                int i = 0;
                string[] temp;
                temp = File.ReadAllLines(openFileDialog.FileName);
                if(temp.Count() != stack_text.Children.OfType<TextBox>().Count() - 1)
                {
                    MessageBox.Show("El archivo no contiene una estructura de datos adecuada. Asegúrese de que se trata del archivo correcto.", "Error al importar", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                foreach (TextBox t in stack_text.Children.OfType<TextBox>())
                {
                    if (t.IsEnabled)
                    {
                        t.Text = temp[i++];
                    }
                }
            }
        }

        // Handles writing to a file the current content of the textboxes
        private void Menu_SaveToFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "LectorExcel files (*.lectorexcel)|*.lectorexcel";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            positions.Clear();
            foreach (TextBox t in stack_text.Children.OfType<TextBox>())
            {
                if (t.IsEnabled)
                {
                    positions.Add(t.Text.ToUpper());
                    Debug.WriteLine("Added " + t.Text);
                }
            }

            if (CheckForEmptyAndDuplicates())
                return;

            if (saveFileDialog.ShowDialog() == true)
            {
                //The using statement automatically flushes AND CLOSES the stream and calls IDisposable.Dispose on the stream object.
                using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
                {
                    foreach (string s in positions)
                    {
                        sw.WriteLine(s);
                    }
                }
            }
        }
    }
}
