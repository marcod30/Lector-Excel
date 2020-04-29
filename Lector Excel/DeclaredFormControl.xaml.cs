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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lector_Excel
{
    /// <summary>
    /// Lógica de interacción para DeclaredFormControl.xaml
    /// </summary>
    public partial class DeclaredFormControl : UserControl
    {
        public DeclaredFormControl()
        {
            InitializeComponent();
        }

        private void Btn_DeleteDeclared_Click(object sender, RoutedEventArgs e)
        {
            return;
        }

        private void Txt_DeclaredNIF_TextChanged(object sender, RoutedEventArgs e)
        {
            if (txt_CommunityOpNIF.IsEnabled)
            {
                if(txt_DeclaredNIF.Text != "")
                {
                    txt_CommunityOpNIF.IsEnabled = false;
                    lbl_CommunityOpNIF.IsEnabled = false;
                }
            }
            else
            {
                if (txt_DeclaredNIF.Text == "")
                {
                    txt_CommunityOpNIF.IsEnabled = true;
                    lbl_CommunityOpNIF.IsEnabled = true;
                }
            }
        }

        private void Txt_CommunityOpNIF_TextChanged(object sender, TextChangedEventArgs e)
        {
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
    }
}
