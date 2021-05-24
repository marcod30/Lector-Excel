using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Win32;
using Reader_347;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace Lector_Excel
{
    
    /// <summary>
    /// La clase para la ventana principal de la aplicación.
    /// </summary>
    public partial class MainWindow : Window
    {
        ///<value> La referencia al gestor de Excel.</value>
        private ExcelManager ExcelManager;
        ///<value> La referencia al gestor de exportación.</value>
        private ExportManager ExportManager;
        ///<value> La referencia al visor de gráficas.</value>
        private ChartWindow ChartWindow;
        ///<value> La referencia a la ventana de progreso.</value>
        ProgressWindow exportProgressBar;

        ///<value> Contiene los valores del registro de tipo 1.</value>
        List<string> Type1Fields = new List<string>();
        ///<value> Contiene los valores de la configuración de Excel.</value>
        private List<string> posiciones = new List<string>();
        ///<value> Contiene los registros de declarados.</value>
        ObservableCollection<DeclaredFormControl> listaDeclarados;
        ///<value> Límite de declarados.</value>
        readonly int DECLARED_AMOUNT_LIMIT = 1000;

        ///<value> La referencia al BackgroundWorker.</value>
        private readonly BackgroundWorker backgroundWorker = new BackgroundWorker();
        private string saveLocation = "";
        
        /// <summary>
        /// Inicializa una nueva instancia de la clase <c>MainWindow</c>.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.DoWork += Worker_DoWork;
            backgroundWorker.ProgressChanged += Worker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += Worker_Completed;
            listaDeclarados = new ObservableCollection<DeclaredFormControl>();
            listaDeclarados.CollectionChanged += DeclaredListChanged;
        }

        //Handles file opening button
        /// <summary>
        /// Función de evento de click izquierdo asociado a "Abrir archivo Excel".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Hojas de cálculo Excel (*.xlsx)|*.xlsx";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                if (posiciones.Count == 0)
                {
                    MessageBox.Show("No se ha establecido la configuración de encolumnado de Excel.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                ExcelManager = new ExcelManager(openFileDialog.FileName);
                backgroundWorker.RunWorkerAsync(argument:"excelImport");

                exportProgressBar = new ProgressWindow(false, "Importando desde Excel...");
                exportProgressBar.Owner = this;
                exportProgressBar.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                exportProgressBar.ShowDialog();
            }
            else
            {
                if (!openFileDialog.SafeFileName.Equals(""))
                {
                    MessageBox.Show("Error al intentar abrir " + openFileDialog.SafeFileName, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

        /// <summary>
        /// Función de evento de click izquierdo asociado a "Exportar datos a Excel".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_SaveExcel_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Hojas de cálculo Excel (*.xlsx)|*.xlsx";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (saveFileDialog.ShowDialog() == true)
            {
                if (posiciones.Count == 0)
                {
                    MessageBox.Show("No se ha establecido la configuración de encolumnado de Excel.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                ExcelManager = new ExcelManager(saveFileDialog.FileName);
                backgroundWorker.RunWorkerAsync(argument: "excelExport");

                exportProgressBar = new ProgressWindow(false, "Exportando a Excel...");
                exportProgressBar.Owner = this;
                exportProgressBar.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                exportProgressBar.ShowDialog();
            }
        }

        //Handles data filling of Type 1
        /// <summary>
        /// Función de evento de click izquierdo asociado a "Abrir registro de tipo 1".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_FillType1_Click(object sender, RoutedEventArgs e)
        {
            Type1Window type1Window = new Type1Window
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

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
        /// <summary>
        /// Función de evento de click izquierdo asociado a "Exportar a archivo BOE".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_Export_Click(object sender, RoutedEventArgs e)
        {
            if (Type1Fields.Count < 5)
            {
                MessageBox.Show("Rellene la información esencial del registro de tipo 1","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }
            bool containsErrors = false;
            foreach(DeclaredFormControl dfc in listaDeclarados)
            {
                
                if (!dfc.IsAllDataValid())
                {
                    containsErrors = true;
                    break;
                }
            }
            if (containsErrors)
            {
                MessageBoxResult msg = MessageBox.Show("Hay errores en los campos de los declarados. Los campos con errores se exportarán como campos en blanco. ¿Continuar?", "ATENCIÓN", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (msg != MessageBoxResult.Yes)
                {
                    return;
                } 
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Archivos 347 (*.347)|*.347|Ficheros de texto (*.txt)|*txt";
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if(sfd.ShowDialog() == true)
            {
                saveLocation = sfd.FileName;
                
                backgroundWorker.RunWorkerAsync(argument:"exportAll");

                exportProgressBar = new ProgressWindow(false, "Exportando...");
                exportProgressBar.Owner = this;
                exportProgressBar.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                exportProgressBar.ShowDialog();
            }
        }

        //Handles text file importing
        /// <summary>
        /// Función de evento de click izquierdo asociado a "Abrir archivo BOE".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                //FIXME: We need to show a progressbar!
                bool importResult = importManager.ImportFromText(out _type1, out _declareds);
                if (importResult)
                {
                    if (_type1 != null && _declareds != null)
                    {
                        this.Type1Fields = _type1;
                        ImportedFileToForm(_declareds);
                    }
                    else
                    {
                        Debug.WriteLine("Whoops! Something went wrong when importing!!!");
                    }
                }
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
        /// <summary>
        /// Función de evento de click izquierdo asociado a "Abrir configuración Excel".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_ImportSettings_Click(object sender, RoutedEventArgs e)
        {
            ImportSettings importSettings = new ImportSettings
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            if (this.posiciones != null && this.posiciones.Count > 0)
                importSettings.positions = this.posiciones;

            importSettings.ShowDialog();
            if (importSettings.DialogResult == true)
            {
                posiciones = importSettings.positions;
            }
        }

        //Handles background worker execution
        /// <summary>
        /// Inicia la actividad del <c>BackgroundWorker</c>.
        /// </summary>
        /// <remarks>Se encarga de registrar el progreso de importación/exportación.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            if ((e.Argument as string).Equals("excelExport"))
            {
                List<Declared> temp = new List<Declared>();
                foreach (DeclaredFormControl dfc in listaDeclarados)
                {
                    temp.Add(dfc.declared);
                }

                ExcelManager.ExportToExcel(posiciones, temp, sender as BackgroundWorker);
            }
        }

        //Handles background worker completion
        /// <summary>
        /// Se lanza automáticamente cuando el <c>BackgroundWorker</c> finaliza.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Adapta los datos recibidos en bruto a estructuras <c>DeclaredFormControl</c>.
        /// </summary>
        /// <param name="result"> Una lista de <c>Declared</c> </param>
        /// <seealso cref="MainWindow.Menu_Import_Click(object, RoutedEventArgs)"/>
        /// <seealso cref="MainWindow.Worker_Completed(object, RunWorkerCompletedEventArgs)"/>
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
                    dfc.PropertyChanged += DeclaredChanged;

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
        /// <summary>
        /// Se lanza automáticamente cada vez que el progreso del <c>BackgroundWorker</c> cambia.
        /// </summary>
        /// <remarks>Actualiza la barra de progreso con el progreso del <c>BackgroundWorker</c>.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            exportProgressBar.Export_Progressbar.Value = e.ProgressPercentage;
        }

        //Handles program exiting
        /// <summary>
        /// Función de evento de click izquierdo asociado a "Salir de la aplicación".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //Handles showing about info
        /// <summary>
        /// Función de evento de click izquierdo asociado a "Acerca de".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_About_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/marcod30/Lector-Excel");
        }

        //When window is completely loaded, execute this
        /// <summary>
        /// Función de evento de carga de <c>MainWindow</c>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Menu_addNewDeclared_Click(sender, e);
        }

        //Handles adding declared forms
        /// <summary>
        /// Función de evento de click izquierdo asociado a "Añadir declarado".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                scrl_MainScrollViewer.ScrollToBottom(); //Scroll to bottom, where new declared was added
            }
            else
            {
                MessageBoxResult msg = MessageBox.Show("No se pueden añadir más de "+DECLARED_AMOUNT_LIMIT+" declarados.", "ATENCIÓN", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }

        //DeclaredContainer is deleted
        /// <summary>
        /// Función de evento de click izquierdo asociado a "Eliminar" en un declarado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeclaredContainer_OnDeleteButtonClick(object sender, EventArgs e)
        {
            MessageBoxResult msg = MessageBox.Show("¿Eliminar el registro? Los datos borrados no se pueden recuperar.", "ATENCIÓN", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (msg == MessageBoxResult.No)
                return;

            if (listaDeclarados.Contains(sender as DeclaredFormControl))
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
        /// <summary>
        /// Función de evento de click izquierdo asociado a "Eliminar todos los declarados".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_deleteAllDeclared_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult msg;
            if (sender is Fluent.Button)
                msg = MessageBox.Show("¿Está completamente seguro de querer eliminar TODOS los registros?", "ATENCIÓN", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            else
                msg = MessageBoxResult.Yes;

            if (msg == MessageBoxResult.No)
                return;

            if(listaDeclarados.Count > 0)
            {
                foreach(DeclaredFormControl dfc in listaDeclarados)
                {
                    dock_DeclaredContainer.Children.Remove(dfc);
                }
                listaDeclarados.Clear();
            }
        }

        //Enable or disable buttons based on declared amount (fires automatically)
        /// <summary>
        /// Evento de delegado que activa o desactiva botones del menú.
        /// </summary>
        /// <remarks>
        /// El estado de los botones cambia según la cantidad de declarados actual.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DeclaredListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (listaDeclarados.Count > 0)
            {
                menu_deleteAllDeclared.IsEnabled = true;
                menu_ScrollSpinner.IsEnabled = true;
                menu_ScrollSpinner.Maximum = listaDeclarados.Count;
                menu_ScrollSpinner.Minimum = 1;
                menu_ScrollSpinner.Value = listaDeclarados.Count;
                menu_ScrollToTop.IsEnabled = true;
                menu_ScrollToBottom.IsEnabled = true;
            }
            else
            {
                menu_deleteAllDeclared.IsEnabled = false;
                menu_ScrollSpinner.IsEnabled = false;
                menu_ScrollSpinner.Maximum = 0;
                menu_ScrollSpinner.Minimum = 0;
                menu_ScrollSpinner.Value = 0;
                menu_ScrollToTop.IsEnabled = false;
                menu_ScrollToBottom.IsEnabled = false;
            }
            
            //Set PropertyChanged method for every item
            foreach(DeclaredFormControl dfc in listaDeclarados)
            {
                dfc.PropertyChanged -= DeclaredChanged;
                dfc.PropertyChanged += DeclaredChanged;
            }
        }

        //When a Declared property changes, trigger this
        /// <summary>
        /// Evento de delegado que actualiza los gráficos cuando un declarado cambia.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <seealso cref="Reader_347.ChartWindow"/>
        public void DeclaredChanged(object sender, PropertyChangedEventArgs e)
        {
            //FIXME - Remove this condition so deleting a declared also updates chart
            if (e.PropertyName.Split('_')[0].Equals("txt"))
            {
                //A textbox changed
                Debug.WriteLine(e.PropertyName + " : " + (sender as DeclaredFormControl).declared.declaredData[e.PropertyName.Substring(4)]);
                if(ChartWindow != null && ChartWindow.IsVisible)
                {
                    switch (ChartWindow.ChartType)
                    {
                        case "VerticalBar_RegistryPerOpKey":
                        case "HorizontalBar_RegistryPerOpKey":
                            ChartWindow.SeriesCollection[0].Values = GetRegistriesPerOpKey();
                            break;
                        case "VerticalBar_BuySellPerTrimester":
                        case "HorizontalBar_BuySellPerTrimester":
                        case "Line_BuySellPerTrimester":
                            ChartWindow.SeriesCollection[0].Values = GetBuySellsPerTrimester(false);
                            ChartWindow.SeriesCollection[1].Values = GetBuySellsPerTrimester(true);
                            break;
                            //FIXME Pie charts are not updating correctly!
                        case "Pie_BuyTotal":
                            ChartWindow.SeriesCollection = GetBuySellsPerRegionAsPie(false);
                            break;
                        case "Pie_SellTotal":
                            ChartWindow.SeriesCollection = GetBuySellsPerRegionAsPie(true);
                            break;
                        case "Map_BuyTotal":
                            ChartWindow.GeoMap.HeatMap = GetBuySellsPerRegion(false);
                            break;
                        case "Map_SellTotal":
                            ChartWindow.GeoMap.HeatMap = GetBuySellsPerRegion(true);
                            break;
                    }
                }
            }

            if (e.PropertyName.Split('_')[0].Equals("chk"))
            {
                //A checkbox changed
                Debug.WriteLine(e.PropertyName + " : " + (sender as DeclaredFormControl).declared.declaredData[e.PropertyName.Substring(4)]);
            }
        }

        //Launch AEAT WebPage
        /// <summary>
        /// Función de evento de click izquierdo asociado a "Abrir página de la AEAT".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_GoToAEAT_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.agenciatributaria.gob.es/AEAT.sede/tramitacion/GI27.shtml");
        }

        //Scroll to top
        /// <summary>
        /// Mueve la barra de desplazamiento hasta arriba.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_ScrollToTop_Click(object sender, RoutedEventArgs e)
        {
            scrl_MainScrollViewer.ScrollToTop();
        }

        //Scroll to bottom
        /// <summary>
        /// Mueve la barra de desplazamiento hasta abajo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_ScrollToBottom_Click(object sender, RoutedEventArgs e)
        {
            scrl_MainScrollViewer.ScrollToBottom();
        }

        //On Main Window Closing, close every child window that's still open
        /// <summary>
        /// Cuando <c>MainWindow</c> se cierra, cierra todas las ventanas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (ChartWindow != null && ChartWindow.IsVisible)
            {
                ChartWindow.Close();
            }
        }

        //Automatically scroll to declared when Spinner changes
        /// <summary>
        /// Evento de cambio del valor del Spinner, que mueve la barra de desplazamiento.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menu_ScrollSpinner_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int value = (int)e.NewValue;
            if(value > 0)
            {
                listaDeclarados[value - 1].BringIntoView();
            }
        }

        //Handles PDF Export
        /// <summary>
        /// Función de evento de click izquierdo asociado a "Exportar a PDF".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <seealso cref="PDFManager"/>
        private void Menu_ExportPDFDraft_Click(object sender, RoutedEventArgs e)
        {
            if(listaDeclarados.Count > 6)
            {
                MessageBoxResult msg = MessageBox.Show(string.Format("La exportación a PDF solo está disponible para 6 o menos registros de declarados. Actualmente hay {0}. ¿Desea continuar y exportar solo los 6 primeros?", listaDeclarados.Count), "ATENCIÓN", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if(msg != MessageBoxResult.Yes)
                {
                    return;
                }
            }
            

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Documentos PDF (*.pdf)|*.pdf";
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (sfd.ShowDialog() == true)
            {
                PDFManager pdfManager = new PDFManager(sfd.FileName);
                List<Declared> temp = new List<Declared>();
                foreach (DeclaredFormControl dfc in listaDeclarados)
                {
                    temp.Add(dfc.declared);
                }

                pdfManager.ExportToPDFDraft(this.Type1Fields, temp);
            }
        }

        //Check Updates and see how everything blows up
        /// <summary>
        /// Función de evento de click izquierdo asociado a "Buscar actualizaciones".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_Updates_Click(object sender, RoutedEventArgs e)
        {
            UpdateChecker updateChecker = new UpdateChecker();

            ProgressWindow pw = new ProgressWindow(true, "Buscando actualizaciones...")
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            pw.Show();

            bool end = updateChecker.GetReleases();
            if (end || !end)
            {
                pw.Close();
            }
        }

        /// <summary>
        /// Función de evento de click izquierdo asociado a "Abrir visor de gráficas".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_ChartVisor_Click(object sender, RoutedEventArgs e)
        {
            //Only instantiate one window at a time
            if(this.ChartWindow == null || !ChartWindow.IsVisible)
            {
                ChartWindow = new ChartWindow();
                ChartWindow.ChartDelegate += UpdateChartInfo;
                ChartWindow.Show();
            }
        }

        //Gets chart request and sends back data to display
        /// <summary>
        /// Evento de delegado que recibe datos de un gráfico y los actualiza.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// See <see cref="ChartWindow"/>
        private void UpdateChartInfo(object sender, ChartEventArgs e)
        {
            if(ChartWindow != null && ChartWindow.IsVisible)
            {
                SeriesCollection series;
                switch (e.chartType)
                {
                    
                    case "VerticalBar_RegistryPerOpKey":
                        
                        series = new SeriesCollection
                        {
                            new ColumnSeries
                            {
                                Title = "Registros",
                                Values = GetRegistriesPerOpKey()
                            }
                        };
                        ChartWindow.SeriesCollection = series;
                        break;
                    case "VerticalBar_BuySellPerTrimester":
                        series = new SeriesCollection
                        {
                            new ColumnSeries
                            {
                                Title = "Compras",
                                Values = GetBuySellsPerTrimester(false)
                            },

                            new ColumnSeries
                            {
                                Title = "Ventas",
                                Values = GetBuySellsPerTrimester(true)
                            }
                        };
                        ChartWindow.SeriesCollection = series;
                        break;
                    case "HorizontalBar_RegistryPerOpKey":

                        series = new SeriesCollection
                        {
                            new RowSeries
                            {
                                Title = "Registros",
                                Values = GetRegistriesPerOpKey()
                            }
                        };
                        ChartWindow.SeriesCollection = series;
                        break;
                    case "HorizontalBar_BuySellPerTrimester":
                        series = new SeriesCollection
                        {
                            new RowSeries
                            {
                                Title = "Compras",
                                Values = GetBuySellsPerTrimester(false)
                            },

                            new RowSeries
                            {
                                Title = "Ventas",
                                Values = GetBuySellsPerTrimester(true)
                            }
                        };
                        ChartWindow.SeriesCollection = series;
                        break;
                    case "Line_BuySellPerTrimester":
                        series = new SeriesCollection
                        {
                            new LineSeries
                            {
                                Title = "Compras",
                                Values = GetBuySellsPerTrimester(false)
                            },

                            new LineSeries
                            {
                                Title = "Ventas",
                                Values = GetBuySellsPerTrimester(true)
                            }
                        };
                        ChartWindow.SeriesCollection = series;
                        break;
                    case "Pie_BuyTotal":
                        ChartWindow.SeriesCollection = GetBuySellsPerRegionAsPie(false);
                        break;
                    case "Pie_SellTotal":
                        ChartWindow.SeriesCollection = GetBuySellsPerRegionAsPie(true);
                        break;
                    case "Map_BuyTotal":
                        ChartWindow.MapValues = GetBuySellsPerRegion(false);
                        break;
                    case "Map_SellTotal":
                        ChartWindow.MapValues = GetBuySellsPerRegion(true);
                        break;
                    default:
                        return;
                }
            }
        }

        //Counts the registries with the same OpKey and returns them as ChartValues
        /// <summary>
        /// Función que cuenta los registros con la misma clave de operación y los devuelve en una estructura adaptada para un gráfico.
        /// </summary>
        /// <returns> Una lista de valores de gráfico.</returns>
        private ChartValues<int> GetRegistriesPerOpKey()
        {
            ChartValues<int> values = new ChartValues<int> { 0, 0, 0, 0, 0, 0, 0 };
            foreach (DeclaredFormControl dfc in listaDeclarados)
            {
                switch (dfc.declared.declaredData["OpKey"])
                {
                    case "A":
                        values[0] += 1;
                        break;
                    case "B":
                        values[1] += 1;
                        break;
                    case "C":
                        values[2] += 1;
                        break;
                    case "D":
                        values[3] += 1;
                        break;
                    case "E":
                        values[4] += 1;
                        break;
                    case "F":
                        values[5] += 1;
                        break;
                    case "G":
                        values[6] += 1;
                        break;
                }
                
            }
            foreach (int i in values)
            {
                if (i == 0)
                    values.Remove(i);
            }
            return values;
        }

        //Adds up money per trimester depending on Op Key
        /// <summary>
        /// Función que suma el valor de compra/venta de cada registro y lo devuelve en una estructura adaptada para un gráfico.
        /// </summary>
        /// <param name="getSells"> Comprueba las ventas en lugar de las compras.</param>
        /// <returns> Una lista de valores de gráfico.</returns>
        private ChartValues<float> GetBuySellsPerTrimester(bool getSells)
        {
            ChartValues<float> values = new ChartValues<float> {0,0,0,0};
            Dictionary<string, string> tempDeclaredData;
            foreach(DeclaredFormControl dfc in listaDeclarados)
            {
                tempDeclaredData = dfc.declared.declaredData;
                if (getSells)
                {
                    if (tempDeclaredData["OpKey"].Equals("B"))
                    {
                        if (!string.IsNullOrEmpty(tempDeclaredData["TrimestralOp1"])){
                            float temp = float.Parse(tempDeclaredData["TrimestralOp1"].Replace(',','.'),NumberStyles.Float,CultureInfo.InvariantCulture);
                            values[0] += temp;
                        }
                        if (!string.IsNullOrEmpty(tempDeclaredData["TrimestralOp2"]))
                        {
                            float temp = float.Parse(tempDeclaredData["TrimestralOp2"].Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture);
                            values[1] += temp;
                        }
                        if (!string.IsNullOrEmpty(tempDeclaredData["TrimestralOp3"]))
                        {
                            float temp = float.Parse(tempDeclaredData["TrimestralOp3"].Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture);
                            values[2] += temp;
                        }
                        if (!string.IsNullOrEmpty(tempDeclaredData["TrimestralOp4"]))
                        {
                            float temp = float.Parse(tempDeclaredData["TrimestralOp4"].Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture);
                            values[3] += temp;
                        }
                    }
                }
                else
                {
                    if (tempDeclaredData["OpKey"].Equals("A"))
                    {
                        if (!string.IsNullOrEmpty(tempDeclaredData["TrimestralOp1"]))
                        {
                            float temp = float.Parse(tempDeclaredData["TrimestralOp1"].Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture);
                            values[0] += temp;
                        }
                        if (!string.IsNullOrEmpty(tempDeclaredData["TrimestralOp2"]))
                        {
                            float temp = float.Parse(tempDeclaredData["TrimestralOp2"].Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture);
                            values[1] += temp;
                        }
                        if (!string.IsNullOrEmpty(tempDeclaredData["TrimestralOp3"]))
                        {
                            float temp = float.Parse(tempDeclaredData["TrimestralOp3"].Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture);
                            values[2] += temp;
                        }
                        if (!string.IsNullOrEmpty(tempDeclaredData["TrimestralOp4"]))
                        {
                            float temp = float.Parse(tempDeclaredData["TrimestralOp4"].Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture);
                            values[3] += temp;
                        }
                    }
                }
            }

            return values;
        }

        //Gets anual money per province and stores in a dictionary
        /// <summary>
        /// Suma el dinero de compra/venta de cada registro, clasificando por provincias.
        /// </summary>
        /// <param name="getSells"> Comprueba las ventas en lugar de las compras.</param>
        /// <returns>Un diccionario cuya clave es la provincia.</returns>
        private Dictionary<string,double> GetBuySellsPerRegion(bool getSells)
        {
            Dictionary<string, double> data = new Dictionary<string, double>();
            Dictionary<string, string> tempDeclaredData = new Dictionary<string, string>();

            foreach (DeclaredFormControl declaredFormControl in listaDeclarados)
            {
                tempDeclaredData = declaredFormControl.declared.declaredData;

                if (getSells)
                {
                    if (tempDeclaredData["OpKey"].Equals("B"))
                    {
                        double amount;
                        if (!string.IsNullOrEmpty(tempDeclaredData["ProvinceCode"]) && !tempDeclaredData["ProvinceCode"].Equals("99"))
                        {
                            amount = double.Parse(tempDeclaredData["AnualMoney"].Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            amount = 0;
                        }

                        if(amount != 0)
                        {
                            if (data.ContainsKey(tempDeclaredData["ProvinceCode"]))
                                data[tempDeclaredData["ProvinceCode"]] += amount;
                            else
                            {
                                data.Add(tempDeclaredData["ProvinceCode"], amount);
                            }
                        }
                    }
                }
                else
                {
                    if (tempDeclaredData["OpKey"].Equals("A"))
                    {
                        double amount;
                        if (!string.IsNullOrEmpty(tempDeclaredData["ProvinceCode"]) && !tempDeclaredData["ProvinceCode"].Equals("99"))
                        {
                            amount = double.Parse(tempDeclaredData["AnualMoney"].Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            amount = 0;
                        }

                        if (amount != 0)
                        {
                            if (data.ContainsKey(tempDeclaredData["ProvinceCode"]))
                                data[tempDeclaredData["ProvinceCode"]] += amount;
                            else
                            {
                                data.Add(tempDeclaredData["ProvinceCode"], amount);
                            }
                        }
                    }
                }
            }

            return data;
        }

        //Gets anual money per province and stores in a SeriesCollection of PieSeries
        /// <summary>
        /// Suma el dinero de compra/venta de cada registro y lo devuelve en una estructura adaptada a un gráfico de tarta.
        /// </summary>
        /// <param name="getSells"> Comprueba las ventas en lugar de las compras.</param>
        /// <returns> Una serie de colecciones de datos de gráfico.</returns>
        private SeriesCollection GetBuySellsPerRegionAsPie(bool getSells)
        {
            SeriesCollection series = new SeriesCollection();
            Dictionary<string, string> tempDeclaredData = new Dictionary<string, string>();
            Func<ChartPoint, string> PieFormatter;
            foreach (DeclaredFormControl declaredFormControl in listaDeclarados)
            {
                tempDeclaredData = declaredFormControl.declared.declaredData;

                if (getSells)
                {
                    if (tempDeclaredData["OpKey"].Equals("B"))
                    {
                        double amount;
                        if (!string.IsNullOrEmpty(tempDeclaredData["ProvinceCode"]) && !string.IsNullOrWhiteSpace(tempDeclaredData["ProvinceCode"]) && !tempDeclaredData["ProvinceCode"].Equals("99"))
                        {
                            amount = double.Parse(tempDeclaredData["AnualMoney"].Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            amount = 0;
                        }

                        if (amount != 0 && Province.CodeToName(tempDeclaredData["ProvinceCode"]) != null)
                        {
                            if(series.Count == 0)
                            {
                                series.Add(new PieSeries
                                {
                                    Title = Province.CodeToName(tempDeclaredData["ProvinceCode"]),
                                    Values = new ChartValues<double> { amount },
                                    DataLabels = true,
                                    LabelPoint = (PieFormatter = chartPoint => string.Format("{0}€ ({1:P})",chartPoint.Y,chartPoint.Participation))
                                });
                            }
                            else
                            {
                                bool foundEqual = false;
                                foreach (PieSeries ps in series)
                                {
                                    if (ps.Title.Equals(Province.CodeToName(tempDeclaredData["ProvinceCode"])))
                                    {
                                        (ps.Values as ChartValues<double>)[0] += amount;
                                        foundEqual = true;
                                        break;
                                    }

                                    
                                }

                                if (!foundEqual)
                                {
                                    series.Add(new PieSeries
                                    {
                                        Title = Province.CodeToName(tempDeclaredData["ProvinceCode"]),
                                        Values = new ChartValues<double> { amount },
                                        DataLabels = true,
                                        LabelPoint = (PieFormatter = chartPoint => string.Format("{0}€ ({1:P})", chartPoint.Y, chartPoint.Participation))
                                    });
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (tempDeclaredData["OpKey"].Equals("A"))
                    {
                        double amount;
                        if (!string.IsNullOrEmpty(tempDeclaredData["ProvinceCode"]) && !string.IsNullOrWhiteSpace(tempDeclaredData["ProvinceCode"]) && !tempDeclaredData["ProvinceCode"].Equals("99"))
                        {
                            amount = double.Parse(tempDeclaredData["AnualMoney"].Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            amount = 0;
                        }

                        if (amount != 0 && Province.CodeToName(tempDeclaredData["ProvinceCode"]) != null)
                        {
                            if (series.Count == 0)
                            {
                                series.Add(new PieSeries
                                {
                                    Title = Province.CodeToName(tempDeclaredData["ProvinceCode"]),
                                    Values = new ChartValues<double> { amount },
                                    DataLabels = true,
                                    LabelPoint = (PieFormatter = chartPoint => string.Format("{0}€ ({1:P})", chartPoint.Y, chartPoint.Participation))
                                });
                            }
                            else
                            {
                                bool foundEqual = false;
                                foreach (PieSeries ps in series)
                                {
                                    if (ps.Title.Equals(Province.CodeToName(tempDeclaredData["ProvinceCode"])))
                                    {
                                        (ps.Values as ChartValues<double>)[0] += amount;
                                        foundEqual = true;
                                        break;
                                    }


                                }

                                if (!foundEqual)
                                {
                                    series.Add(new PieSeries
                                    {
                                        Title = Province.CodeToName(tempDeclaredData["ProvinceCode"]),
                                        Values = new ChartValues<double> { amount },
                                        DataLabels = true,
                                        LabelPoint = (PieFormatter = chartPoint => string.Format("{0}€ ({1:P})", chartPoint.Y, chartPoint.Participation))
                                    });
                                }
                            }
                        }
                    }
                }
            }

            return series;
        }
    }

    /// <summary>
    /// Clase abstracta de provincia
    /// </summary>
    public class Province
    {
        /// <summary>
        /// Inicializa una nueva instancia de <c>Province</c>
        /// </summary>
        public Province()
        {

        }

        /// <summary>
        /// Transforma el código numérico de una provincia en su nombre real
        /// </summary>
        /// <param name="code"> El código numérico de la provincia.</param>
        /// <returns> El nombre de la provincia como cadena.</returns>
        public static string CodeToName(string code)
        {
            switch (code)
            {
                case "01":
                    return "Álava";
                case "02":
                    return "Albacete";
                case "03":
                    return "Alicante";
                case "04":
                    return "Almería";
                case "05":
                    return "Ávila";
                case "06":
                    return "Badajoz";
                case "07":
                    return "Baleares";
                case "08":
                    return "Barcelona";
                case "09":
                    return "Burgos";
                case "10":
                    return "Cáceres";
                case "11":
                    return "Cádiz";
                case "12":
                    return "Castellón";
                case "13":
                    return "Ciudad Real";
                case "14":
                    return "Córdoba";
                case "15":
                    return "A Coruña";
                case "16":
                    return "Cuenca";
                case "17":
                    return "Girona";
                case "18":
                    return "Granada";
                case "19":
                    return "Guadalajara";
                case "20":
                    return "Gipúzkoa";
                case "21":
                    return "Huelva";
                case "22":
                    return "Huesca";
                case "23":
                    return "Jaén";
                case "24":
                    return "León";
                case "25":
                    return "Lleida";
                case "26":
                    return "La Rioja";
                case "27":
                    return "Lugo";
                case "28":
                    return "Madrid";
                case "29":
                    return "Málaga";
                case "30":
                    return "Murcia";
                case "31":
                    return "Navarra";
                case "32":
                    return "Ourense";
                case "33":
                    return "Asturias";
                case "34":
                    return "Palencia";
                case "35":
                    return "Las Palmas";
                case "36":
                    return "Pontevedra";
                case "37":
                    return "Salamanca";
                case "38":
                    return "Sta. Cruz de Tenerife";
                case "39":
                    return "Cantabria";
                case "40":
                    return "Segovia";
                case "41":
                    return "Sevilla";
                case "42":
                    return "Soria";
                case "43":
                    return "Tarragona";
                case "44":
                    return "Teruel";
                case "45":
                    return "Toledo";
                case "46":
                    return "Valencia";
                case "47":
                    return "Valladolid";
                case "48":
                    return "Bizkaia";
                case "49":
                    return "Zamora";
                case "50":
                    return "Zaragoza";
                case "51":
                    return "Ceuta";
                case "52":
                    return "Melilla";

                default:
                    return null;
            }
        }
    }
}
