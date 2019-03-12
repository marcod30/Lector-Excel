using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lector_Excel
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ExcelManager ExcelManager;
        List<string> Type1Fields = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
        }

        //Handles file opening button
        private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                ExcelManager = new ExcelManager(openFileDialog.FileName);
                menu_Export.IsEnabled = true; // enable on correct file read
            }
            else
            {
                menu_Export.IsEnabled = false;
            }
        }

        //Handles data filling of Type 1
        private void Menu_FillType1_Click(object sender, RoutedEventArgs e)
        {
            Type1Window type1Window = new Type1Window();
            type1Window.Owner = this;
            if (Type1Fields != null && Type1Fields.Count > 0)
                type1Window.Lista = Type1Fields;
            type1Window.ShowDialog();
            if(type1Window.DialogResult == true)
            {
                MessageBox.Show("Cambios confirmados","cambios",MessageBoxButton.OK,MessageBoxImage.Information);
                Type1Fields = type1Window.Lista;


                MessageBox.Show(Type1Fields[0]);
            }
            else
            {
                MessageBox.Show("Cambios descartados","cambios", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (Type1Fields.Count > 0 /* && Excel opened*/)
            {
                menu_Export.IsEnabled = true;
            }
            else
            {
                menu_Export.IsEnabled = false;
            }
        }

        //Handles text file exporting
        private void Menu_Export_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult msg = MessageBox.Show("Se van a exportart los datos. ¿Continuar?","OJOCUIDAO",MessageBoxButton.YesNo,MessageBoxImage.Question);
            if(msg != MessageBoxResult.Yes)
            {
                return;
            }

            ExcelManager.ExportData(Type1Fields[0],Type1Fields[1]);
            // Mostrar ventana con ProgressBar
            // Exportar a fichero de texto
            //   |- Quizás desde ExcelManager?
        }

        //Handles program exiting
        private void Menu_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //Handles showing about info
        private void Menu_About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Owner = this;
            aboutWindow.ShowDialog();
        }
    }
}
