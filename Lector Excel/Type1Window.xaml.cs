using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
        /*
         * Lista holds the information from Type1Window's TextBoxes in order of appearance
         * For uses in other classes, Lista should ALWAYS have the fields in its following positions,
         * even if they are empty:
         *  0  => txt_Ejercicio
         *  1  => txt_NIF
         *  2  => txt_Name
         *  3  => txt_SupportType
         *  4  => txt_Phone
         *  5  => txt_RelationName
         *  6  => txt_DeclarationID
         *  7  => txt_ComplementaryDec
         *  8  => txt_SustitutiveDec
         *  9  => txt_PrevDeclarationID
         *  10 => txt_Entities
         *  11 => txt_TotalMoney
         *  12 => txt_TotalProperties
         *  13 => txt_TotalMoneyRental
         *  14 => txt_NIFLegal
        */
        // Handles changes confirmation
        private void Btn_OK_Click(object sender, RoutedEventArgs e)
        {
            Lista.Clear();
            Lista.Add(txt_Ejercicio.Text);
            Lista.Add(txt_NIF.Text.ToUpper());
            Lista.Add(txt_Name.Text.ToUpper());
            Lista.Add(txt_SupportType.Text.ToUpper());
            Lista.Add(txt_Phone.Text);
            Lista.Add(txt_RelationName.Text.ToUpper());
            Lista.Add(txt_DeclarationID.Text);
            Lista.Add(txt_ComplementaryDec.Text.ToUpper());
            Lista.Add(txt_SustitutiveDec.Text.ToUpper());
            Lista.Add(txt_PrevDeclarationID.Text);
            Lista.Add(txt_Entities.Text);
            Lista.Add(txt_TotalMoney.Text);
            Lista.Add(txt_TotalProperties.Text);
            Lista.Add(txt_TotalMoneyRental.Text);
            Lista.Add(txt_NIFLegal.Text.ToUpper());
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
                int index = 0;
                Lista.Clear();
                foreach(string s in temp)
                {
                    Lista.Add(temp[index++]);
                }

                txt_Ejercicio.Text = Lista[0];
                txt_NIF.Text = Lista[1];
                txt_Name.Text = Lista[2];
                txt_SupportType.Text = Lista[3];
                txt_Phone.Text = Lista[4];
                txt_RelationName.Text = Lista[5];
                txt_DeclarationID.Text = Lista[6];
                txt_ComplementaryDec.Text = Lista[7];
                txt_SustitutiveDec.Text = Lista[8];
                txt_PrevDeclarationID.Text = Lista[9];
                txt_Entities.Text = Lista[10];
                txt_TotalMoney.Text = Lista[11];
                txt_TotalProperties.Text = Lista[12];
                txt_TotalMoneyRental.Text = Lista[13];
                txt_NIFLegal.Text = Lista[14];
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
                txt_NIF.Text = Lista[1];
                txt_Name.Text = Lista[2];
                txt_SupportType.Text = Lista[3];
                txt_Phone.Text = Lista[4];
                txt_RelationName.Text = Lista[5];
                txt_DeclarationID.Text = Lista[6];
                txt_ComplementaryDec.Text = Lista[7];
                txt_SustitutiveDec.Text = Lista[8];
                txt_PrevDeclarationID.Text = Lista[9];
                txt_Entities.Text = Lista[10];
                txt_TotalMoney.Text = Lista[11];
                txt_TotalProperties.Text = Lista[12];
                txt_TotalMoneyRental.Text = Lista[13];
                txt_NIFLegal.Text = Lista[14];
            }
        }
    }
}
