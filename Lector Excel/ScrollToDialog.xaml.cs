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
    /// Clase de argumentos de evento para el delegado de la barra de desplazamiento.
    /// </summary>
    public class ScrollEventArgs : EventArgs
    {
        ///<value>La posición a la que se debe desplazar.</value>
        public int Position;
    }

    /// <summary>
    /// Delegado empleado para comunicarse con <c>MainWindow</c>.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ScrollDialogDelegate(object sender, ScrollEventArgs e);

    /// <summary>
    /// Clase de la ventana del selector de registros.
    /// </summary>
    public partial class ScrollToDialog : Window
    {
        /// <value>Obtiene o cambia el máximo valor seleccionable.</value>
        public int maxValue { get; set; }
        /// <value>Obtiene o cambia el valor de retorno.</value>
        public int returnValue { get; set; }

        const string NUMERICAL_REGEX = @"\d?";

        /// <value>Evento asociado al delegado.</value>
        public event ScrollDialogDelegate ScrollDelegate;

        /// <summary>
        /// Inicializa una nueva instancia de <c>ScrollToDialog</c>.
        /// </summary>
        public ScrollToDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Función de evento de carga de la ventana.
        /// </summary>
        /// <remarks>Asocia el máximo registro seleccionable cuando la ventana termina de cargarse.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            nud_MainNumericUpDown.Maximum = maxValue;
        }

        /// <summary>
        /// Actualiza el valor de retorno cuando el registro seleccionado cambia.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Nud_MainNumericUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            returnValue = (int)nud_MainNumericUpDown.Value;

            ScrollEventArgs scrollEventArgs = new ScrollEventArgs();
            scrollEventArgs.Position = returnValue;
            OnPositionChanged(scrollEventArgs);
        }

        /// <summary>
        /// Invoca al delegado.
        /// </summary>
        /// <param name="e"></param>
        private void OnPositionChanged(ScrollEventArgs e)
        {
            ScrollDelegate?.Invoke(this, e);
        }
    }
}
