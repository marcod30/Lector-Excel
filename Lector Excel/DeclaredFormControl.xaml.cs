using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Lógica de interacción para DeclaredFormControl.xaml
    /// </summary>
    public partial class DeclaredFormControl : UserControl
    {
        public event EventHandler DeleteButtonClick; //Delegate for deleting
        public Declared declared;
        //Regexps
        const string DNI_REGEX = @"^(\d{8})([A-Z])$";
        const string CIF_REGEX = @"^([ABCDEFGHJKLMNPQRSUVW])(\d{7})([0-9]|[A-J])$";
        const string NIE_REGEX = @"^[XYZ]\d{7,8}[A-Z]$";
        const string STATE_CODE_REGEX = @"(\d{2})";
        const string UNSIGNED_FLOAT_REGEX = @"^(\d)+((\.|\,)(\d{1,2}))?$";
        const string SIGNED_FLOAT_REGEX = @"^\-?(\d)+((\.|\,)(\d{1,2}))?$";

        public DeclaredFormControl()
        {
            InitializeComponent();
            declared = new Declared();
        }

        //Send delegate handler
        protected virtual void OnDeleteButtonClick(EventArgs e)
        {
            var handler = DeleteButtonClick;
            if(handler != null)
            {
                handler(this, e);
            }
        }

        //On delete button click
        private void Btn_DeleteDeclared_Click(object sender, RoutedEventArgs e)
        {
            OnDeleteButtonClick(e);
        }

        //If NIF Textbox changes
        private void Txt_DeclaredNIF_TextChanged(object sender, RoutedEventArgs e)
        {
            var thisTextBox = sender as TextBox;
            //Disable Community NIF textbox as they are incompatible
            if (txt_CommunityOpNIF.IsEnabled)
            {
                if(!thisTextBox.Text.Equals(""))
                {
                    txt_CommunityOpNIF.IsEnabled = false;
                    lbl_CommunityOpNIF.IsEnabled = false;
                }
            }
            else
            {
                if (thisTextBox.Text.Equals(""))
                {
                    txt_CommunityOpNIF.IsEnabled = true;
                    lbl_CommunityOpNIF.IsEnabled = true;
                }
            }

            if(!thisTextBox.Text.Equals("") && !IsNIFValid(thisTextBox.Text)){
                thisTextBox.BorderBrush = Brushes.Red;
            }
            else
            {
                thisTextBox.ClearValue(TextBox.BorderBrushProperty);
            }
        }

        //If Legal Representative NIF textbox changes
        private void Txt_LegalRepNIF_TextChanged(object sender, TextChangedEventArgs e)
        {
            var thisTextBox = sender as TextBox;
            if (!thisTextBox.Text.Equals("") && !Regex.IsMatch(thisTextBox.Text,DNI_REGEX))
            {
                thisTextBox.BorderBrush = Brushes.Red;
            }
            else
            {
                thisTextBox.ClearValue(TextBox.BorderBrushProperty);
            }
        }

        //Function to validate a NIF through regular expressions
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

        //If community NIF textbox changes
        private void Txt_CommunityOpNIF_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Disable Declared NIF textbox as they are incompatible
            if (txt_DeclaredNIF.IsEnabled)
            {
                if (txt_CommunityOpNIF.Text != "")
                {
                    txt_DeclaredNIF.IsEnabled = false;
                    lbl_DeclaredNIF.IsEnabled = false;
                }

            }
            else
            {
                if (txt_CommunityOpNIF.Text == "")
                {
                    txt_DeclaredNIF.IsEnabled = true;
                    lbl_DeclaredNIF.IsEnabled = true;
                }
            }
        }

        //If province code textbox changes OR State code textbox changes
        private void Txt_ProvinceCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            var thisTextBox = sender as TextBox;
            if (!thisTextBox.Text.Equals("") && !Regex.IsMatch(thisTextBox.Text,STATE_CODE_REGEX))
            {
                thisTextBox.BorderBrush = Brushes.Red;
            }
            else
            {
                thisTextBox.ClearValue(TextBox.BorderBrushProperty);
            }
        }

        //If any textbox that should contain a signed float number changes
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

        //If any textbox that should contain an unsigned float number changes
        private void Txt_UnsignedFloat_TextChanged(object sender, TextChangedEventArgs e)
        {
            var thisTextBox = sender as TextBox;
            if (!thisTextBox.Text.Equals("") && !Regex.IsMatch(thisTextBox.Text, UNSIGNED_FLOAT_REGEX))
            {
                thisTextBox.BorderBrush = Brushes.Red;
            }
            else
            {
                thisTextBox.ClearValue(TextBox.BorderBrushProperty);
            }
        }

        //If any textbox loses focus, try to send content to data class Declared
        private void Txt_Any_LostFocus(object sender, RoutedEventArgs e)
        {
            var thisTextBox = sender as TextBox;
            Debug.WriteLine(thisTextBox.Name+" LOST FOCUS TRIGGERED!!!");
            if (thisTextBox.BorderBrush != Brushes.Red)
            {
                string keyName = thisTextBox.Name.Substring(4); //Get Name subtracting "txt_"
                if (declared.declaredData.ContainsKey(keyName))
                {
                    Debug.WriteLine("Key " + keyName + " exists!");
                    Debug.WriteLine("Updating dict value to " + thisTextBox.Text);
                    declared.declaredData[keyName] = thisTextBox.Text;
                }
            }
            else
                Debug.WriteLine(thisTextBox.Name + " has invalid data!!");
        }

        //If any checkbox is checked or unchecked, try to send content to data class Declared
        private void Chk_Any_Checked_Changed(object sender, RoutedEventArgs e)
        {
            var thisCheckbox = sender as CheckBox;
            string keyName = thisCheckbox.Name.Substring(4); //Get Name subtracting "chk_"
            if (declared.declaredData.ContainsKey(keyName))
            {
                if (thisCheckbox.IsChecked == true)
                {
                    Debug.WriteLine(thisCheckbox.Name + " HAS BEEN CHECKED!!!");
                    declared.declaredData[keyName] = "X";
                }
                else if (thisCheckbox.IsChecked == false)
                {
                    Debug.WriteLine(thisCheckbox.Name + " HAS BEEN UNCHECKED!!!");
                    declared.declaredData[keyName] = " ";
                }
                else
                    Debug.WriteLine(thisCheckbox.Name + ": ERROR PARSING ISCHECKED PROPERTY!!!");
            }
        }

        private void Txt_Exercise_TextChanged(object sender, TextChangedEventArgs e)
        {
            var thisTextBox = sender as TextBox;
            if (!thisTextBox.Text.Equals("") && !Regex.IsMatch(thisTextBox.Text, @"(\d{4})"))
            {
                thisTextBox.BorderBrush = Brushes.Red;
            }
            else
            {
                thisTextBox.ClearValue(TextBox.BorderBrushProperty);
            }
        }
    }
}
