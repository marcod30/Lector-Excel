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
    public class ScrollEventArgs : EventArgs
    {
        public int Position;
    }

    public delegate void ScrollDialogDelegate(object sender, ScrollEventArgs e);

    /// <summary>
    /// Lógica de interacción para ScrollToDialog.xaml
    /// </summary>
    public partial class ScrollToDialog : Window
    {
        public int maxValue { get; set; }
        public int returnValue { get; set; }

        const string NUMERICAL_REGEX = @"\d?";

        public event ScrollDialogDelegate ScrollDelegate;

        public ScrollToDialog()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            nud_MainNumericUpDown.Maximum = maxValue;
        }


        private void Nud_MainNumericUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            returnValue = (int)nud_MainNumericUpDown.Value;

            ScrollEventArgs scrollEventArgs = new ScrollEventArgs();
            scrollEventArgs.Position = returnValue;
            OnPositionChanged(scrollEventArgs);
        }

        private void OnPositionChanged(ScrollEventArgs e)
        {
            ScrollDelegate?.Invoke(this, e);
        }
    }
}
