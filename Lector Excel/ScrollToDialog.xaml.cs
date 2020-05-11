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
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace Reader_347
{
    /// <summary>
    /// Lógica de interacción para ScrollToDialog.xaml
    /// </summary>
    public partial class ScrollToDialog : Window
    {
        public int maxValue { get; set; }
        public int returnValue { get; set; }

        const string NUMERICAL_REGEX = @"\d?";

        public ScrollToDialog()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            nud_MainNumericUpDown.Maximum = maxValue;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Value_IsValid();
            returnValue = (int)nud_MainNumericUpDown.Value;
            this.DialogResult = true;
        }

        private void Value_IsValid()
        {
            if(!Regex.IsMatch(nud_MainNumericUpDown.Value.ToString(), NUMERICAL_REGEX))
            {
                nud_MainNumericUpDown.Value = 1;
                return;
            }
            if (nud_MainNumericUpDown.Value > maxValue)
            {
                nud_MainNumericUpDown.Value = maxValue;
                return;
            }
            if (nud_MainNumericUpDown.Value < 1)
            {
                nud_MainNumericUpDown.Value = 1;
                return;
            }
        }
    }
}
