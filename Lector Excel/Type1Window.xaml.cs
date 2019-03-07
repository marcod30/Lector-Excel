﻿using System;
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

        // Handles changes confirmation
        private void Btn_OK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        // Handles window force close
        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
