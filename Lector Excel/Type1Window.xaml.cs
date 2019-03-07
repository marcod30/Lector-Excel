using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
        }
        public List<string> Lista { get; set; }
        // Handles changes confirmation
        private void Btn_OK_Click(object sender, RoutedEventArgs e)
        {
            if (Lista == null)
                Lista = new List<string>();
            Lista.Add(txt_Ejercicio.Text);
            Lista.Add(txt_Name.Text);
            Lista.Add(txt_NIF.Text);
            Lista.Add(txt_Entities.Text);
            Lista.Add(txt_TotalMoney.Text);
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

        }

        private void Menu_exportData_Click(object sender, RoutedEventArgs e)
        {
            if(Lista.Count > 0)
            //The using statement automatically flushes AND CLOSES the stream and calls IDisposable.Dispose on the stream object.
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(@Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\PersonalDataType1.lectorexcel"))
            {
                foreach (string s in Lista)
                {
                    sw.WriteLine(s);
                }
            }
        }
    }
}
