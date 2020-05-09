using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
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
        private ExportManager ExportManager;

        List<string> Type1Fields = new List<string>();
        private List<string> posiciones = new List<string>();
        ObservableCollection<DeclaredFormControl> listaDeclarados = new ObservableCollection<DeclaredFormControl>();
        const int DECLARED_AMOUNT_LIMIT = 500;

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
            listaDeclarados.CollectionChanged += DeclaredListChanged;
        }

        //Handles file opening button
        private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Hojas de cálculo Excel (*.xlsx)|*.xlsx";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true && posiciones.Count != 0)
            {
                ExcelManager = new ExcelManager(openFileDialog.FileName);
                backgroundWorker.RunWorkerAsync(argument:"excelImport");

                exportProgressBar = new ProgressWindow(false, "Importando desde Excel...");
                exportProgressBar.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                exportProgressBar.ShowDialog();
            }
            else
            {
                if(ExcelManager != null)
                {
                    menu_Export.IsEnabled = false;
                }
                
                if (!openFileDialog.SafeFileName.Equals(""))
                {
                    MessageBoxResult msg = MessageBox.Show("Error al intentar abrir " + openFileDialog.SafeFileName, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if(posiciones.Count == 0)
                {
                    MessageBoxResult msg = MessageBox.Show("No se han establecido parámetros de importación", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
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
        }

        //Handles text file exporting
        private void Menu_Export_Click(object sender, RoutedEventArgs e)
        {
            if (Type1Fields.Count < 5)
            {
                MessageBox.Show("Rellene primero los datos del registro de tipo 1","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }

            MessageBoxResult msg = MessageBox.Show("Se van a exportar los datos. ¿Continuar?","ATENCIÓN",MessageBoxButton.YesNo,MessageBoxImage.Question);
            if (msg != MessageBoxResult.Yes)
            {
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Archivos 347 (*.347)|*.347|Ficheros de texto (*.txt)|*txt";
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if(sfd.ShowDialog() == true)
            {
                saveLocation = sfd.FileName;
                
                backgroundWorker.RunWorkerAsync(argument:"exportAll");

                exportProgressBar = new ProgressWindow(false, "Exportando...");
                exportProgressBar.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                exportProgressBar.ShowDialog();
            }
        }

        //Handles text file importing
        private void Menu_Import_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos 347 (*.347)|*.347|Ficheros de texto (*.txt)|*txt";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                ImportManager importManager = new ImportManager(openFileDialog.FileName);
                List<string> _type1 = new List<string>();
                List<Declared> _declareds = new List<Declared>();

                if (importManager.ImportFromText(out _type1, out _declareds))
                {
                    if(_type1 != null && _declareds != null)
                    {
                        this.Type1Fields = _type1;
                        ImportedFileToForm(_declareds);
                    }
                    else
                    {
                        Debug.WriteLine("Whoops! Something went wrong when importing!!!");
                    }
                }
                /*
                backgroundWorker.RunWorkerAsync(argument: "excelImport");

                exportProgressBar = new ProgressWindow(false, "Importando desde Excel...");
                exportProgressBar.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                exportProgressBar.ShowDialog();
                */
            }
            else
            {
                if (!openFileDialog.SafeFileName.Equals(""))
                {
                    MessageBoxResult msg = MessageBox.Show("Error al intentar abrir " + openFileDialog.SafeFileName, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Handles opening the Import Settings Window
        private void Menu_ImportSettings_Click(object sender, RoutedEventArgs e)
        {
            ImportSettings importSettings = new ImportSettings();
            importSettings.Owner = this;

            if (this.posiciones != null && this.posiciones.Count > 0)
                importSettings.positions = this.posiciones;

            importSettings.ShowDialog();
            if (importSettings.DialogResult == true)
            {
                posiciones = importSettings.positions;
            }
        }

        //Handles background worker execution
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //ExcelManager.ExportData(Type1Fields, sender as BackgroundWorker, posiciones, saveLocation);

            if((e.Argument as string).Equals("exportAll")){
                List<Declared> temp = new List<Declared>();
                ExportManager = new ExportManager(saveLocation, Type1Fields);
                foreach (DeclaredFormControl dfc in listaDeclarados)
                {
                    temp.Add(dfc.declared);
                }
                ExportManager.ExportFromMain(temp, sender as BackgroundWorker);
            }
            
            if((e.Argument as string).Equals("excelImport"))
            {
                List<Declared> temp = new List<Declared>();
                temp = ExcelManager.ImportExcelData(posiciones, sender as BackgroundWorker);
                e.Result = temp;
            }
        }

        //Handles background worker completion
        private void Worker_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            List<Declared> result = e.Result as List<Declared>;
            if(result != null)
            {
                ImportedFileToForm(result);
            }
            exportProgressBar.Close();
        }

        //Adapts any imported file to forms
        private void ImportedFileToForm(List<Declared> result)
        {
            Menu_deleteAllDeclared_Click(this, null);
            foreach (Declared d in result)
            {
                if (dock_DeclaredContainer.Children.Count < DECLARED_AMOUNT_LIMIT)
                {
                    DeclaredFormControl dfc = new DeclaredFormControl();

                    dfc.mainGroupBox.Header = "Declarado " + (listaDeclarados.Count + 1);
                    dfc.DeleteButtonClick += DeclaredContainer_OnDeleteButtonClick;   //Subscribe delegate for deleting

                    dfc.declared = d;
                    dfc.txt_DeclaredNIF.Text = (dfc.declared.declaredData["DeclaredNIF"].Length >= 9) ? dfc.declared.declaredData["DeclaredNIF"].Substring(0, 9) : dfc.declared.declaredData["DeclaredNIF"];
                    dfc.txt_LegalRepNIF.Text = (dfc.declared.declaredData["LegalRepNIF"].Length >= 9) ? dfc.declared.declaredData["LegalRepNIF"].Substring(0, 9) : dfc.declared.declaredData["LegalRepNIF"];
                    dfc.txt_CommunityOpNIF.Text = (dfc.declared.declaredData["CommunityOpNIF"].Length >= 17) ? dfc.declared.declaredData["CommunityOpNIF"].Substring(0, 17) : dfc.declared.declaredData["CommunityOpNIF"];
                    dfc.txt_DeclaredName.Text = (dfc.declared.declaredData["DeclaredName"].Length >= 40) ? dfc.declared.declaredData["DeclaredName"].Substring(0, 40) : dfc.declared.declaredData["DeclaredName"];
                    dfc.txt_ProvinceCode.Text = (dfc.declared.declaredData["ProvinceCode"].Length >= 2) ? dfc.declared.declaredData["ProvinceCode"].Substring(0, 2) : dfc.declared.declaredData["ProvinceCode"];
                    dfc.txt_CountryCode.Text = (dfc.declared.declaredData["CountryCode"].Length >= 2) ? dfc.declared.declaredData["CountryCode"].Substring(0, 2) : dfc.declared.declaredData["CountryCode"];
                    dfc.txt_OpKey.Text = (dfc.declared.declaredData["OpKey"].Length >= 1) ? dfc.declared.declaredData["OpKey"].Substring(0, 1) : dfc.declared.declaredData["OpKey"];
                    dfc.txt_TotalMoney.Text = (dfc.declared.declaredData["TotalMoney"].Length >= 15) ? dfc.declared.declaredData["TotalMoney"].Substring(0, 15) : dfc.declared.declaredData["TotalMoney"];
                    dfc.txt_AnualMoney.Text = (dfc.declared.declaredData["AnualMoney"].Length >= 16) ? dfc.declared.declaredData["AnualMoney"].Substring(0, 16) : dfc.declared.declaredData["AnualMoney"];
                    dfc.txt_AnualPropertyMoney.Text = (dfc.declared.declaredData["AnualPropertyMoney"].Length >= 16) ? dfc.declared.declaredData["AnualPropertyMoney"].Substring(0, 16) : dfc.declared.declaredData["AnualPropertyMoney"];
                    dfc.txt_AnualOpIVA.Text = (dfc.declared.declaredData["AnualOpIVA"].Length >= 16) ? dfc.declared.declaredData["AnualOpIVA"].Substring(0, 16) : dfc.declared.declaredData["AnualOpIVA"];
                    dfc.txt_Exercise.Text = (dfc.declared.declaredData["Exercise"].Length >= 4) ? dfc.declared.declaredData["Exercise"].Substring(0, 4) : dfc.declared.declaredData["Exercise"];
                    dfc.txt_TrimestralOp1.Text = (dfc.declared.declaredData["TrimestralOp1"].Length >= 16) ? dfc.declared.declaredData["TrimestralOp1"].Substring(0, 16) : dfc.declared.declaredData["TrimestralOp1"];
                    dfc.txt_TrimestralOp2.Text = (dfc.declared.declaredData["TrimestralOp2"].Length >= 16) ? dfc.declared.declaredData["TrimestralOp2"].Substring(0, 16) : dfc.declared.declaredData["TrimestralOp2"]; ;
                    dfc.txt_TrimestralOp3.Text = (dfc.declared.declaredData["TrimestralOp3"].Length >= 16) ? dfc.declared.declaredData["TrimestralOp3"].Substring(0, 16) : dfc.declared.declaredData["TrimestralOp3"]; ;
                    dfc.txt_TrimestralOp4.Text = (dfc.declared.declaredData["TrimestralOp4"].Length >= 16) ? dfc.declared.declaredData["TrimestralOp4"].Substring(0, 16) : dfc.declared.declaredData["TrimestralOp4"]; ;
                    dfc.txt_AnualPropertyIVAOp1.Text = (dfc.declared.declaredData["AnualPropertyIVAOp1"].Length >= 16) ? dfc.declared.declaredData["AnualPropertyIVAOp1"].Substring(0,16) : dfc.declared.declaredData["AnualPropertyIVAOp1"];
                    dfc.txt_AnualPropertyIVAOp2.Text = (dfc.declared.declaredData["AnualPropertyIVAOp2"].Length >= 16) ? dfc.declared.declaredData["AnualPropertyIVAOp2"].Substring(0, 16) : dfc.declared.declaredData["AnualPropertyIVAOp2"];
                    dfc.txt_AnualPropertyIVAOp3.Text = (dfc.declared.declaredData["AnualPropertyIVAOp3"].Length >= 16) ? dfc.declared.declaredData["AnualPropertyIVAOp3"].Substring(0, 16) : dfc.declared.declaredData["AnualPropertyIVAOp3"];
                    dfc.txt_AnualPropertyIVAOp4.Text = (dfc.declared.declaredData["AnualPropertyIVAOp4"].Length >= 16) ? dfc.declared.declaredData["AnualPropertyIVAOp4"].Substring(0, 16) : dfc.declared.declaredData["AnualPropertyIVAOp4"];

                    if (dfc.declared.declaredData["OpInsurance"].Equals("X"))
                    {
                        dfc.chk_OpInsurance.IsChecked = true;
                    }
                    else
                        dfc.declared.declaredData["OpInsurance"] = " ";
                    if (dfc.declared.declaredData["LocalBusinessLease"].Equals("X"))
                    {
                        dfc.chk_LocalBusinessLease.IsChecked = true;
                    }
                    else
                        dfc.declared.declaredData["LocalBusinessLease"] = " ";
                    if (dfc.declared.declaredData["OpIVA"].Equals("X"))
                    {
                        dfc.chk_OpIVA.IsChecked = true;
                    }
                    else
                        dfc.declared.declaredData["OpInsurance"] = " ";
                    if (dfc.declared.declaredData["OpPassive"].Equals("X"))
                    {
                        dfc.chk_OpPassive.IsChecked = true;
                    }
                    else
                        dfc.declared.declaredData["OpPassive"] = " ";
                    if (dfc.declared.declaredData["OpCustoms"].Equals("X"))
                    {
                        dfc.chk_OpCustoms.IsChecked = true;
                    }
                    else
                        dfc.declared.declaredData["OpCustoms"] = " ";
                    DockPanel.SetDock(dfc, Dock.Top);
                    dock_DeclaredContainer.Children.Add(dfc);
                    listaDeclarados.Add(dfc);
                }
                else
                    break;
            }
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

        //When window is completely loaded, execute this
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            return;
        }

        //Handles adding declared forms
        private void Menu_addNewDeclared_Click(object sender, RoutedEventArgs e)
        {
            if (dock_DeclaredContainer.Children.Count < DECLARED_AMOUNT_LIMIT)
            {
                DeclaredFormControl d = new DeclaredFormControl();

                d.mainGroupBox.Header = "Declarado "+(listaDeclarados.Count+1);
                d.DeleteButtonClick += DeclaredContainer_OnDeleteButtonClick;   //Subscribe delegate for deleting

                DockPanel.SetDock(d, Dock.Top);
                dock_DeclaredContainer.Children.Add(d);
                listaDeclarados.Add(d);
            }
            else
            {
                MessageBoxResult msg = MessageBox.Show("No se pueden añadir más de "+DECLARED_AMOUNT_LIMIT+" declarados.", "ATENCIÓN", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }

        //DeclaredContainer is deleted
        private void DeclaredContainer_OnDeleteButtonClick(object sender, EventArgs e)
        {
            if(listaDeclarados.Contains(sender as DeclaredFormControl))
            {
                listaDeclarados.Remove(sender as DeclaredFormControl);
                dock_DeclaredContainer.Children.Remove(sender as DeclaredFormControl);

                //Reorganize items
                foreach(DeclaredFormControl dfc in listaDeclarados)
                {
                    var pos = listaDeclarados.IndexOf(dfc);
                    dfc.mainGroupBox.Header = "Declarado " + (pos+1);
                }

                if (listaDeclarados.Count == 0)
                    menu_deleteAllDeclared.IsEnabled = false;
            }
        }

        //Handles deleting all declareds
        private void Menu_deleteAllDeclared_Click(object sender, RoutedEventArgs e)
        {
            if(listaDeclarados.Count > 0)
            {
                foreach(DeclaredFormControl dfc in listaDeclarados)
                {
                    dock_DeclaredContainer.Children.Remove(dfc);
                }
                listaDeclarados.Clear();
            }
        }

        //Enable or disable export based on declared amount (fires automatically)
        public void DeclaredListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (listaDeclarados.Count > 0)
            {
                if(Type1Fields.Count >= 5)
                    menu_Export.IsEnabled = true;
                menu_deleteAllDeclared.IsEnabled = true;
            }
            else
            {
                if(Type1Fields.Count < 5)
                    menu_Export.IsEnabled = false;
                menu_deleteAllDeclared.IsEnabled = false;
            }
                
        }

        //Launch AEAT WebPage
        private void Menu_GoToAEAT_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www12.agenciatributaria.gob.es/wlpl/OVCT-CXEW/SelectorAcceso?ref=%2Fwlpl%2FPAIN-M347%2FE2019%2Findex.zul&aut=CP");
        }
    }
}
