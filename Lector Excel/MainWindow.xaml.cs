using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Lector_Excel
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ExcelManager ExcelManager;
        List<string> Type1Fields = new List<string>();
        ProgressWindow exportProgressBar;
        private readonly BackgroundWorker backgroundWorker = new BackgroundWorker();
        private string saveLocation = "";
        public MainWindow()
        {
            InitializeComponent();
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.DoWork += Worker_DoWork;
            backgroundWorker.ProgressChanged += Worker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += Worker_Completed;
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
                btn_Export.IsEnabled = true;
                lbl_fileOpenStatus.Foreground = Brushes.Green;
                lbl_fileOpenStatus.Content =openFileDialog.SafeFileName + " abierto con éxito.";
            }
            else
            {
                menu_Export.IsEnabled = false;
                btn_Export.IsEnabled = false;
                if (!openFileDialog.SafeFileName.Equals(""))
                {
                    lbl_fileOpenStatus.Foreground = Brushes.Red;
                    lbl_fileOpenStatus.Content = "Error al intentar abrir " + openFileDialog.SafeFileName;
                }
                else
                {
                    lbl_fileOpenStatus.Content = "";
                }

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
                //MessageBox.Show("Cambios confirmados","cambios",MessageBoxButton.OK,MessageBoxImage.Information);
                Type1Fields = type1Window.Lista;
            }
            if (Type1Fields.Count > 0 && ExcelManager != null)
            {
                menu_Export.IsEnabled = true;
                btn_Export.IsEnabled = true;
            }
            else
            {
                menu_Export.IsEnabled = false;
                btn_Export.IsEnabled = false;
            }
        }

        //Handles text file exporting
        private void Menu_Export_Click(object sender, RoutedEventArgs e)
        {
            if (Type1Fields.Count < 5)
            {
                MessageBox.Show("Rellene primero todos los datos de tipo 1","Error");
                return;
            }
            MessageBoxResult msg = MessageBox.Show("Se van a exportart los datos. ¿Continuar?","OJOCUIDAO",MessageBoxButton.YesNo,MessageBoxImage.Question);
            
            if (msg != MessageBoxResult.Yes)
            {
                
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text files (*.txt)|*.txt";
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if(sfd.ShowDialog() == true)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                saveLocation = sfd.FileName;
                
                backgroundWorker.RunWorkerAsync();

                exportProgressBar = new ProgressWindow(false, "Exportando...");
                exportProgressBar.ShowDialog();

                Mouse.OverrideCursor = Cursors.Arrow;

                menu_Export.IsEnabled = false;
                btn_Export.IsEnabled = false;
                lbl_fileOpenStatus.Content = "";
            }
        }

        //Handles background worker execution
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            ExcelManager.ExportData(Type1Fields, sender as BackgroundWorker, saveLocation);
        }

        //Handles background worker completion
        private void Worker_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            exportProgressBar.Close();
        }

        //Handles background worker progress
        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            exportProgressBar.Export_Progressbar.Value = e.ProgressPercentage;
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
