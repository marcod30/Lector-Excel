using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Lógica de interacción para Type1Window.xaml
    /// </summary>
    public partial class Type1Window : Window
    {
        
        public Type1Window()
        {
            InitializeComponent();
            Lista = new List<string>();
        }
        public List<string> Lista { get; set; }
        // Handles changes confirmation
        private void Btn_OK_Click(object sender, RoutedEventArgs e)
        {
            Lista.Clear();
            Lista.Add(txt_Ejercicio.Text);
            Lista.Add(txt_Name.Text);
            Lista.Add(txt_NIF.Text);
            Lista.Add(txt_Entities.Text);
            Lista.Add(txt_TotalMoney.Text);
            this.DialogResult = true;
            this.Close();
        }

        // Handles window force close
        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Menu_importData_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "LectorExcel files (*.lectorexcel)|*.lectorexcel";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                string[] temp;
                temp = File.ReadAllLines(openFileDialog.FileName);
                IEnumerable<TextBox> collection = main_dockpanel.Children.OfType<TextBox>();
                int index = 0;
                Lista.Clear();
                foreach (TextBox t in collection)
                {
                    t.Text = temp[index];
                    Lista.Add(temp[index]);
                    index++;
                }
            }
        }

        private void Menu_exportData_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "LectorExcel files (*.lectorexcel)|*.lectorexcel";
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (sfd.ShowDialog() == true)
            {
                if (Lista.Count > 0)
                    //The using statement automatically flushes AND CLOSES the stream and calls IDisposable.Dispose on the stream object.
                    using (StreamWriter sw = new StreamWriter(sfd.FileName))
                    {
                        foreach (string s in Lista)
                        {
                            sw.WriteLine(s);
                        }
                    }
            }
        }

        // When Window is completely loaded, execute this
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Lista != null && Lista.Count > 0)
            {
                menu_exportData.IsEnabled = true;
                txt_Ejercicio.Text = Lista[0];
                txt_Name.Text = Lista[1];
                txt_NIF.Text = Lista[2];
                txt_Entities.Text = Lista[3];
                txt_TotalMoney.Text = Lista[4];
            }
        }
    }
}
