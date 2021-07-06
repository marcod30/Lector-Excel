using Microsoft.Win32;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System;
using System.IO;
using Reader_347.Models;

namespace Lector_Excel
{
    /// <summary>
    /// Clase de la ventana de configuración de Excel.
    /// </summary>
    public partial class ImportSettings : Window
    {
        /// <value> Contiene las posiciones de cada campo en el Excel.</value>
        private List<string> positions { get; set; }

        /// <value> Almacena la configuración de Excel.</value>
        private ExcelSettings Settings = ExcelSettings.Settings;

        /// <summary>
        /// Inicializa una nueva instancia de <c>ImportSettings</c>.
        /// </summary>
        public ImportSettings()
        {
            InitializeComponent();
            positions = new List<string>();
            DataContext = this;
        }

        // Handles manual window closing, without saving changes
        /// <summary>
        /// Función de evento de click izquierdo asociado a "Cerrar ventana".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        // Handles manual window closing and saves changes
        /// <summary>
        /// Función de evento de click izquierdo asociado a "Guardar cambios".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            //Set settings fields
            Settings.DeclaredNIF = txt_declaredNIF.Text;
            Settings.DeclaredName = txt_declaredName.Text;
            Settings.LegalRepNIF = txt_legalRepNIF.Text;
            Settings.SheetType = txt_sheetType.Text;
            Settings.ProvinceCode = txt_provinceCode.Text;
            Settings.StateCode = txt_stateCode.Text;
            Settings.OpKey = txt_opKey.Text;
            Settings.AnualOpMoney = txt_anualMoney.Text;
            Settings.OpInsurance = txt_opInsurance.Text;
            Settings.LocalBusinessRental = txt_localBusinessRental.Text;
            Settings.MetalMoney = txt_metalMoney.Text;
            Settings.AnualPropertyTransmissionIVA = txt_anualMoneyPropIVA.Text;
            Settings.Exercise = txt_exercise.Text;
            Settings.OpMoney1T = txt_opMoney1.Text;
            Settings.OpMoney2T = txt_opMoney2.Text;
            Settings.OpMoney3T = txt_opMoney3.Text;
            Settings.OpMoney4T = txt_opMoney4.Text;
            Settings.PropertyTransmissionIVA1T = txt_moneyPropIVA1.Text;
            Settings.PropertyTransmissionIVA2T = txt_moneyPropIVA2.Text;
            Settings.PropertyTransmissionIVA3T = txt_moneyPropIVA3.Text;
            Settings.PropertyTransmissionIVA4T = txt_moneyPropIVA4.Text;
            Settings.CommunityOpNIF = txt_commOpNIF.Text;
            Settings.OpSpecialRegIVA = txt_opSpecialIVA.Text;
            Settings.OpPassive = txt_opPassive.Text;
            Settings.OpRegNotCustoms = txt_opAduanero.Text;
            Settings.AnualMoneyDevengedIVA = txt_anualMoneyBoxIVA.Text;

            Settings.FirstRowIsTitle = (bool) chk_TitleRow.IsChecked;

            this.DialogResult = true;
            this.Close();
        }

        // Checks if there is any duplicate or empty fields. If there were any, returns true, otherwise returns false.
        /// <summary>
        /// Comprueba si algún campo está vacío o duplicado y avisa al usuario.
        /// </summary>
        /// <returns>True si algún campo estaba vacío o duplicado, de lo contrario false.</returns>
        private bool CheckForEmptyAndDuplicates()
        {
            if (positions.Count() != positions.Distinct().Count())   // Check if there are duplicates
            {
                MessageBox.Show("Parece que ha introducido valores duplicados. Por favor, revise los campos e inténtelo de nuevo", "Valores repetidos", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            else if (!positions.All(s => !s.Equals("")) || !positions.All(s => !s.Contains(" ")))  // Check if any field was empty or contains blank spaces
            {
                MessageBox.Show("Parece que ha dejado algún campo vacío o con espacios. Por favor, revise los campos e inténtelo de nuevo", "Campo erróneo", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }

            return false;
        }

        // Called after window is fully loaded
        /// <summary>
        /// Función de evento de carga de ventana.
        /// </summary>
        /// <remarks>Comprueba si ya hay datos en la aplicación y de ser así los muestra.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txt_declaredNIF.Text = Settings.DeclaredNIF;
            txt_declaredName.Text = Settings.DeclaredName;
            txt_legalRepNIF.Text = Settings.LegalRepNIF;
            txt_sheetType.Text = Settings.SheetType;
            txt_provinceCode.Text = Settings.ProvinceCode;
            txt_stateCode.Text = Settings.StateCode;
            txt_opKey.Text = Settings.OpKey;
            txt_anualMoney.Text = Settings.AnualOpMoney;
            txt_opInsurance.Text = Settings.OpInsurance;
            txt_localBusinessRental.Text = Settings.LocalBusinessRental;
            txt_metalMoney.Text = Settings.MetalMoney;
            txt_anualMoneyPropIVA.Text = Settings.AnualPropertyTransmissionIVA;
            txt_exercise.Text = Settings.Exercise;
            txt_opMoney1.Text = Settings.OpMoney1T;
            txt_opMoney2.Text = Settings.OpMoney2T;
            txt_opMoney3.Text = Settings.OpMoney3T;
            txt_opMoney4.Text = Settings.OpMoney4T;
            txt_moneyPropIVA1.Text = Settings.PropertyTransmissionIVA1T;
            txt_moneyPropIVA2.Text = Settings.PropertyTransmissionIVA2T;
            txt_moneyPropIVA3.Text = Settings.PropertyTransmissionIVA3T;
            txt_moneyPropIVA4.Text = Settings.PropertyTransmissionIVA4T;
            txt_commOpNIF.Text = Settings.CommunityOpNIF;
            txt_opSpecialIVA.Text = Settings.OpSpecialRegIVA;
            txt_opPassive.Text = Settings.OpPassive;
            txt_opAduanero.Text = Settings.OpRegNotCustoms;
            txt_anualMoneyBoxIVA.Text = Settings.AnualMoneyDevengedIVA;

            chk_TitleRow.IsChecked = Settings.FirstRowIsTitle;

            positions.Clear();
            foreach (TextBox t in stack_text.Children.OfType<TextBox>())
            {
                if (t.IsEnabled)
                {
                    positions.Add(t.Text.ToUpper());
                }
            }
        }

        // When called, resets all TextBoxes to their default values
        /// <summary>
        /// Función de evento de click izquierdo asociado a "Reiniciar configuración".
        /// </summary>
        /// <remarks>Pone todos los campos a sus valores por defecto.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            chk_TitleRow.IsChecked = true;
        }

        // Handles opening file and writing in textboxes
        /// <summary>
        /// Función de evento de click izquierdo asociado a "Abrir archivo".
        /// </summary>
        /// <remarks>
        /// Carga la configuración de Excel desde un archivo y la escribe en los campos.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_LoadFromFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de configuración de importación (*.is347)|*is347|Archivos de LectorExcel (*.lectorexcel)|*.lectorexcel";
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
        /// <summary>
        /// Función de evento de click izquierdo asociado a "Guardar archivo".
        /// </summary>
        /// <remarks>Guarda los datos de los campos en un archivo.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_SaveToFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Archivos de configuración de Excel (*.is347)|*is347|Archivos de LectorExcel (*.lectorexcel)|*.lectorexcel";
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
