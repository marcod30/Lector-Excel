using Lector_Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Reader_347.Views
{
    /// <summary>
    /// Clase de agrupación de formularios de un registro de inmueble.
    /// </summary>
    public partial class PropertyFormControl : UserControl, INotifyPropertyChanged
    {
        /// <value>Evento de delegado cuando se elimina un registro.</value>
        public event EventHandler DeleteButtonClick; //Delegate for deleting
        /// <value>Evento que se lanza cuando una propiedad del registro cambia.</value>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <value> El inmueble asociado al formulario.</value>
        public Declared property;

        //Regexps
        const string DNI_REGEX = @"^(\d{8})([a-zA-Z])$";
        const string CIF_REGEX = @"^([abcdefghjklmnpqrsuvwABCDEFGHJKLMNPQRSUVW])(\d{7})([0-9]|[a-jA-J])$";
        const string NIE_REGEX = @"^[xyzXYZ]\d{7,8}[a-zA-Z]$";
        const string PROV_CODE_REGEX = @"(\d{2})";
        const string INTEGER_REGEX = @"^(\d)+$";
        const string SIGNED_FLOAT_REGEX = @"^\-?(\d)+((\.|\,)(\d{1,2}))?$";

        /// <summary>
        /// Inicializa una nueva instancia de <c>PropertyFormControl</c>.
        /// </summary>
        public PropertyFormControl()
        {
            InitializeComponent();
            property = new Declared(true);
        }

        //Send delegate handler
        /// <summary>
        /// Evento que despliega el delegado.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDeleteButtonClick(EventArgs e)
        {
            DeleteButtonClick?.Invoke(this, e);
        }

        /// <summary>
        /// Botón de evento de click izquierdo asociado a "Eliminar inmueble"
        /// </summary>
        /// <remarks> Invoca al delegado. </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_DeleteProperty_Click(object sender, RoutedEventArgs e)
        {
            OnDeleteButtonClick(e);
        }

        //If NIF Textbox changes
        /// <summary>
        /// Función de evento de cambio de texto asociado a "NIF de arrendatario".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Txt_RenterNIF_TextChanged(object sender, TextChangedEventArgs e)
        {
            var thisTextBox = sender as TextBox;

            if (!thisTextBox.Text.Equals("") && !IsNIFValid(thisTextBox.Text))
            {
                thisTextBox.BorderBrush = Brushes.Red;
            }
            else
            {
                //PropertyChanged(this,new PropertyChangedEventArgs(thisTextBox.Name));
                thisTextBox.ClearValue(TextBox.BorderBrushProperty);
            }
        }

        //If Legal Representative NIF textbox changes
        /// <summary>
        /// Función de evento de cambio de texto asociado a "NIF de rep. legal".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Txt_LegalRepNIF_TextChanged(object sender, TextChangedEventArgs e)
        {
            var thisTextBox = sender as TextBox;
            if (!thisTextBox.Text.Equals("") && !Regex.IsMatch(thisTextBox.Text, DNI_REGEX))
            {
                thisTextBox.BorderBrush = Brushes.Red;
            }
            else
            {
                thisTextBox.ClearValue(TextBox.BorderBrushProperty);
            }
        }

        //Function to validate a NIF through regular expressions
        /// <summary>
        /// Valida un NIF, NIE o CIF.
        /// </summary>
        /// <param name="nif">El NIF que se desea validar.</param>
        /// <returns>True si el NIF es válido, de lo contrario false.</returns>
        private bool IsNIFValid(string nif)
        {
            if (Regex.IsMatch(nif, DNI_REGEX))
                return true;
            if (Regex.IsMatch(nif, NIE_REGEX))
                return true;
            if (Regex.IsMatch(nif, CIF_REGEX))
                return true;

            return false;
        }

        //If province code textbox changes
        /// <summary>
        /// Función de evento de cambio de texto asociado a "Código de provincia".
        /// </summary>
        /// <remarks>Activa o desactiva el código de país según el valor.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Txt_ProvinceCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            var thisTextBox = sender as TextBox;
            if (!thisTextBox.Text.Equals("") && !Regex.IsMatch(thisTextBox.Text, PROV_CODE_REGEX))
            {
                thisTextBox.BorderBrush = Brushes.Red;
            }
            else
            {
                thisTextBox.ClearValue(TextBox.BorderBrushProperty);
            }
        }

        //If any textbox that should contain a signed float number changes
        /// <summary>
        /// Función de evento de cambio de texto asociado a cualquier campo que pueda contener un <c>float</c> con signo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Txt_SignedFloat_TextChanged(object sender, TextChangedEventArgs e)
        {
            var thisTextBox = sender as TextBox;
            if (!thisTextBox.Text.Equals("") && !Regex.IsMatch(thisTextBox.Text, SIGNED_FLOAT_REGEX))
            {
                thisTextBox.BorderBrush = Brushes.Red;
            }
            else
            {
                thisTextBox.ClearValue(TextBox.BorderBrushProperty);
            }
        }

        //If any textbox that should contain a integer number changes
        /// <summary>
        /// Función de evento de cambio de texto asociado a cualquier campo que pueda contener un <c>int</c>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Txt_Integer_TextChanged(object sender, TextChangedEventArgs e)
        {
            var thisTextBox = sender as TextBox;
            if(!thisTextBox.Text.Equals("") && !Regex.IsMatch(thisTextBox.Text, INTEGER_REGEX))
            {
                thisTextBox.BorderBrush = Brushes.Red;
            }
            else
            {
                thisTextBox.ClearValue(TextBox.BorderBrushProperty);
            }
        }

        private void Txt_PostalCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            var thisTextBox = sender as TextBox;
            if (!thisTextBox.Text.Equals("") && !Regex.IsMatch(thisTextBox.Text, @"(\d{5})"))
            {
                thisTextBox.BorderBrush = Brushes.Red;
            }
            else
            {
                thisTextBox.ClearValue(TextBox.BorderBrushProperty);
            }
        }

        //If any textbox loses focus, try to send content to data class Declared
        /// <summary>
        /// Función de evento de pérdida de foco asociado a todos los campos.
        /// </summary>
        /// <remarks> Actualiza el dato correspondiente del <c>Declared</c> asociado.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Txt_Any_LostFocus(object sender, RoutedEventArgs e)
        {
            var thisTextBox = sender as TextBox;
            Debug.WriteLine(thisTextBox.Name + " LOST FOCUS TRIGGERED!!!");
            if (thisTextBox.BorderBrush != Brushes.Red)
            {
                string keyName = thisTextBox.Name.Substring(4); //Get Name subtracting "txt_"
                if (property.declaredData.ContainsKey(keyName))
                {
                    Debug.WriteLine("Key " + keyName + " exists!");
                    Debug.WriteLine("Updating dict value to " + thisTextBox.Text);
                    property.declaredData[keyName] = thisTextBox.Text;

                    //Notify of property change
                    PropertyChanged(this, new PropertyChangedEventArgs(thisTextBox.Name));
                }
            }
            else
                Debug.WriteLine(thisTextBox.Name + " has invalid data!!");
        }

        //If there is a TextBox with invalid data, return false
        /// <summary>
        /// Comprueba que todos los campos contienen datos válidos.
        /// </summary>
        /// <returns>False si algún campo contiene algún error, de lo contrario devuelve true.</returns>
        public bool IsAllDataValid()
        {
            foreach (Grid g in groupStack.Children.OfType<Grid>())
            {
                foreach (TextBox t in g.Children.OfType<TextBox>())
                {
                    if (t.BorderBrush == Brushes.Red)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
