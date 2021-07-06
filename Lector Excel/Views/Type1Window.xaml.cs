using Microsoft.Win32;
using Reader_347;
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
    /// Clase de la ventana de edición del registro de tipo 1.
    /// </summary>
    public partial class Type1Window : Window
    {
        /// <summary>
        /// Inicializa una nueva instancia de <c>Type1Window</c>.
        /// </summary>
        public Type1Window()
        {
            InitializeComponent();
        }
        /// <value>Obtiene o cambia la lista de datos del registro.</value>
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
        /// <summary>
        /// Función de evento de click izquierdo asociado a "Guardar cambios".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_OK_Click(object sender, RoutedEventArgs e)
        {
            UpdateModel();
            //Lista.Clear();
            //Lista.Add(txt_Ejercicio.Text);
            //Lista.Add(txt_NIF.Text.ToUpper());
            //Lista.Add(txt_Name.Text.ToUpper());
            //Lista.Add(txt_SupportType.Text.ToUpper());
            //Lista.Add(txt_Phone.Text);
            //Lista.Add(txt_RelationName.Text.ToUpper());
            //Lista.Add(txt_DeclarationID.Text);
            //if (rad_ComplementaryDec.IsChecked == true)
            //{
            //    Lista.Add("C");
            //}
            //else
            //{
            //    Lista.Add("");
            //}
            //if (rad_SustitutiveDec.IsChecked == true)
            //{
            //    Lista.Add("S");
            //}
            //else
            //{
            //    Lista.Add("");
            //}
            ////Lista.Add(txt_ComplementaryDec.Text.ToUpper());
            ////Lista.Add(txt_SustitutiveDec.Text.ToUpper());
            //Lista.Add(txt_PrevDeclarationID.Text);
            //Lista.Add(txt_Entities.Text);
            //Lista.Add(txt_TotalMoney.Text);
            //Lista.Add(txt_TotalProperties.Text);
            //Lista.Add(txt_TotalMoneyRental.Text);
            //Lista.Add(txt_NIFLegal.Text.ToUpper());
            this.DialogResult = true;
            this.Close();
        }

        // Handles window force close
        /// <summary>
        /// Función de evento de click izquiero asociado a "No guardar".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        /// <summary>
        /// Función de evento de click izquierdo asociado a "Abrir archivo".
        /// </summary>
        /// <remarks>Recupera los datos de registro de tipo 1 de un archivo.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_importData_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de configuración tipo 1 (*.t1347)|*.t1347|Archivos de LectorExcel (*.lectorexcel)|*.lectorexcel";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                string[] temp;
                temp = File.ReadAllLines(openFileDialog.FileName);
                /*
                if(temp.Count() != main_canvas.Children.OfType<TextBox>().Count() - 2)
                {
                    MessageBox.Show("El archivo no contiene una estructura de datos adecuada. Asegúrese de que se trata del archivo correcto.", "Error al importar", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                */
                UpdateModel(temp);
                UpdateTextBoxes();
                //int index = 0;
                //Lista.Clear();
                //foreach(string s in temp)
                //{
                //    Lista.Add(temp[index++]);
                //}

                //txt_Ejercicio.Text = Lista[0];
                //txt_NIF.Text = Lista[1];
                //txt_Name.Text = Lista[2];
                //txt_SupportType.Text = Lista[3];
                //txt_Phone.Text = Lista[4];
                //txt_RelationName.Text = Lista[5];
                //txt_DeclarationID.Text = Lista[6];
                //if (Lista[7].Equals("C"))
                //{
                //    rad_ComplementaryDec.IsChecked = true;
                //}
                //else
                //{
                //    rad_ComplementaryDec.IsChecked = false;
                //}
                //if (Lista[8].Equals("S"))
                //{
                //    rad_SustitutiveDec.IsChecked = true;
                //}
                //else
                //{
                //    rad_SustitutiveDec.IsChecked = false;
                //}
                //if (!Lista[7].Equals("C") && !Lista[8].Equals("S"))
                //{
                //    rad_NoTypeDec.IsChecked = true;
                //}
                //txt_PrevDeclarationID.Text = Lista[9];
                //txt_Entities.Text = Lista[10];
                //txt_TotalMoney.Text = Lista[11];
                //txt_TotalProperties.Text = Lista[12];
                //txt_TotalMoneyRental.Text = Lista[13];
                //txt_NIFLegal.Text = Lista[14];
            }
        }

        /// <summary>
        /// Función de evento de click izquierdo asociado a "Guardar en archivo".
        /// </summary>
        /// <remarks>Almacena los datos del registro de tipo 1 en un archivo.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_exportData_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Archivos de configuración tipo 1 (*.t1347)|*.t1347|Archivos de LectorExcel (*.lectorexcel)|*.lectorexcel";
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (sfd.ShowDialog() == true)
            {
                //The using statement automatically flushes AND CLOSES the stream and calls IDisposable.Dispose on the stream object.
                using (StreamWriter sw = new StreamWriter(sfd.FileName))
                {
                    sw.WriteLine(Model347.Model.Type1Fields.Ejercicio);
                    sw.WriteLine(Model347.Model.Type1Fields.DeclarantNIF);
                    sw.WriteLine(Model347.Model.Type1Fields.DeclarantName);
                    sw.WriteLine(Model347.Model.Type1Fields.SupportType);
                    sw.WriteLine(Model347.Model.Type1Fields.RelationsPhone);
                    sw.WriteLine(Model347.Model.Type1Fields.RelationsName);
                    sw.WriteLine(Model347.Model.Type1Fields.DeclarationID);

                    if (Model347.Model.Type1Fields.IsComplementaryDec)
                        sw.WriteLine("C");
                    else if (Model347.Model.Type1Fields.IsSustitutiveDec)
                        sw.WriteLine("S");
                    else
                        sw.WriteLine(" ");

                    sw.WriteLine(Model347.Model.Type1Fields.TotalEntities);
                    sw.WriteLine(Model347.Model.Type1Fields.TotalAnualMoney);
                    sw.WriteLine(Model347.Model.Type1Fields.TotalProperties);
                    sw.WriteLine(Model347.Model.Type1Fields.TotalMoneyRental);
                    sw.WriteLine(Model347.Model.Type1Fields.LegalRepNIF);
                }
                /*
                if (Lista.Count > 0)
                    //The using statement automatically flushes AND CLOSES the stream and calls IDisposable.Dispose on the stream object.
                    using (StreamWriter sw = new StreamWriter(sfd.FileName))
                    {
                        foreach (string s in Lista)
                        {
                            sw.WriteLine(s);
                        }
                    }
                */
            }
        }

        // When Window is completely loaded, execute this
        /// <summary>
        /// Función de evento de carga de la ventana.
        /// </summary>
        /// <remarks>Comprueba si había datos del registro de tipo 1 en memoria y, de ser así, los carga.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateTextBoxes();

            /*
            if (Lista != null && Lista.Count > 0)
            {
                txt_Ejercicio.Text = Lista[0];
                txt_NIF.Text = Lista[1];
                txt_Name.Text = Lista[2];
                txt_SupportType.Text = Lista[3];
                txt_Phone.Text = Lista[4];
                txt_RelationName.Text = Lista[5];
                txt_DeclarationID.Text = Lista[6];
                if (Lista[7].Equals("C"))
                {
                    rad_ComplementaryDec.IsChecked = true;
                }
                else
                {
                    rad_ComplementaryDec.IsChecked = false;
                }
                if (Lista[8].Equals("S"))
                {
                    rad_SustitutiveDec.IsChecked = true;
                }
                else
                {
                    rad_SustitutiveDec.IsChecked = false;
                }
                if (!Lista[7].Equals("C") && !Lista[8].Equals("S"))
                {
                    rad_NoTypeDec.IsChecked = true;
                }
                txt_PrevDeclarationID.Text = Lista[9];
                txt_Entities.Text = Lista[10];
                txt_TotalMoney.Text = Lista[11];
                txt_TotalProperties.Text = Lista[12];
                txt_TotalMoneyRental.Text = Lista[13];
                txt_NIFLegal.Text = Lista[14];
            }
            */
        }

        /// <summary>
        /// Actualiza los campos de texto con los valores del modelo.
        /// </summary>
        private void UpdateTextBoxes()
        {
            if (Model347.Model.Type1Fields != null)
            {
                txt_Ejercicio.Text = Model347.Model.Type1Fields.Ejercicio;
                txt_NIF.Text = Model347.Model.Type1Fields.DeclarantNIF;
                txt_Name.Text = Model347.Model.Type1Fields.DeclarantName;
                txt_SupportType.Text = Model347.Model.Type1Fields.SupportType;
                txt_Phone.Text = Model347.Model.Type1Fields.RelationsPhone;
                txt_RelationName.Text = Model347.Model.Type1Fields.RelationsName;
                txt_DeclarationID.Text = Model347.Model.Type1Fields.DeclarationID;
                txt_PrevDeclarationID.Text = Model347.Model.Type1Fields.PrevDeclarationID;
                txt_Entities.Text = Model347.Model.Type1Fields.TotalEntities;
                txt_TotalMoney.Text = Model347.Model.Type1Fields.TotalAnualMoney;
                txt_TotalProperties.Text = Model347.Model.Type1Fields.TotalProperties;
                txt_TotalMoneyRental.Text = Model347.Model.Type1Fields.TotalMoneyRental;
                txt_NIFLegal.Text = Model347.Model.Type1Fields.LegalRepNIF;

                rad_ComplementaryDec.IsChecked = Model347.Model.Type1Fields.IsComplementaryDec;
                rad_SustitutiveDec.IsChecked = Model347.Model.Type1Fields.IsSustitutiveDec;
                rad_NoTypeDec.IsChecked = Model347.Model.Type1Fields.IsNonSpecialDec;
            }
        }

        /// <summary>
        /// Actualiza el modelo con los valores de los campos de texto o los de una lista.
        /// </summary>
        /// <param name="fields">Lista con los valores a actualizar. Si es null, se usan los campos de texto.</param>
        private void UpdateModel(string[] fields=null)
        {
            if (Model347.Model.Type1Fields != null)
            {
                if(fields != null)
                {
                    Model347.Model.Type1Fields.Ejercicio = fields[0];
                    Model347.Model.Type1Fields.DeclarantNIF = fields[1];
                    Model347.Model.Type1Fields.DeclarantName = fields[2];
                    Model347.Model.Type1Fields.SupportType = fields[3];
                    Model347.Model.Type1Fields.RelationsPhone = fields[4];
                    Model347.Model.Type1Fields.RelationsName = fields[5];
                    Model347.Model.Type1Fields.DeclarationID = fields[6];
                    Model347.Model.Type1Fields.PrevDeclarationID = fields[8];
                    Model347.Model.Type1Fields.TotalEntities = fields[9];
                    Model347.Model.Type1Fields.TotalAnualMoney = fields[10];
                    Model347.Model.Type1Fields.TotalProperties = fields[11];
                    Model347.Model.Type1Fields.TotalMoneyRental = fields[12];
                    Model347.Model.Type1Fields.LegalRepNIF = fields[13];

                    if (fields[7].Equals("C"))
                        Model347.Model.Type1Fields.IsComplementaryDec = true;
                    else if (fields[7].Equals("S"))
                        Model347.Model.Type1Fields.IsSustitutiveDec = true;
                    else
                        Model347.Model.Type1Fields.IsNonSpecialDec = true;
                }
                else
                {
                    Model347.Model.Type1Fields.Ejercicio = txt_Ejercicio.Text;
                    Model347.Model.Type1Fields.DeclarantNIF = txt_NIF.Text.ToUpper();
                    Model347.Model.Type1Fields.DeclarantName = txt_Name.Text.ToUpper();
                    Model347.Model.Type1Fields.SupportType = txt_SupportType.Text.ToUpper();
                    Model347.Model.Type1Fields.RelationsPhone = txt_Phone.Text;
                    Model347.Model.Type1Fields.RelationsName = txt_RelationName.Text.ToUpper();
                    Model347.Model.Type1Fields.DeclarationID = txt_DeclarationID.Text;
                    Model347.Model.Type1Fields.PrevDeclarationID = txt_PrevDeclarationID.Text;
                    Model347.Model.Type1Fields.TotalEntities = txt_Entities.Text;
                    Model347.Model.Type1Fields.TotalAnualMoney = txt_TotalMoney.Text;
                    Model347.Model.Type1Fields.TotalProperties = txt_TotalProperties.Text;
                    Model347.Model.Type1Fields.TotalMoneyRental = txt_TotalMoneyRental.Text;
                    Model347.Model.Type1Fields.LegalRepNIF = txt_NIFLegal.Text.ToUpper();

                    if (rad_ComplementaryDec.IsChecked == true)
                    {
                        Model347.Model.Type1Fields.IsComplementaryDec = true;
                        Model347.Model.Type1Fields.IsSustitutiveDec = false;
                        Model347.Model.Type1Fields.IsNonSpecialDec = false;
                    }
                    else if (rad_SustitutiveDec.IsChecked == true)
                    {
                        Model347.Model.Type1Fields.IsComplementaryDec = false;
                        Model347.Model.Type1Fields.IsSustitutiveDec = true;
                        Model347.Model.Type1Fields.IsNonSpecialDec = false;
                    }
                    else
                    {
                        Model347.Model.Type1Fields.IsComplementaryDec = false;
                        Model347.Model.Type1Fields.IsSustitutiveDec = false;
                        Model347.Model.Type1Fields.IsNonSpecialDec = true;
                    }
                }
            }
        }
    }
}
