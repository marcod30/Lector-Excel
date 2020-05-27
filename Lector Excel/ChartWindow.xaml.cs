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

using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Wpf.Charts.Base;

namespace Reader_347
{
    public class ChartEventArgs : EventArgs
    {
        public string chartType;
    }

    public delegate void ChartSetterDelegate(object sender, ChartEventArgs e);
    /// <summary>
    /// Lógica de interacción para ChartWindow.xaml
    /// </summary>
    public partial class ChartWindow : Window
    {
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        public event ChartSetterDelegate ChartDelegate;

        public ChartWindow()
        {
            InitializeComponent();

            //Remove this placeholder code!!
            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "A la mierda",
                    Values = new ChartValues<int> {0,5,10,50}
                }
            };

            Labels = new[] { "A", "B", "C", "D" , "E"};
            Formatter = value => value.ToString("N");
            //---

            DataContext = this;
        }

        //Sends info to parent window
        private void OnChartSelection(ChartEventArgs e)
        {
            ChartDelegate?.Invoke(this, e);
        }

        //Creates a new CartesianChart (placeholder code!)
        private void CreateCartesianChart()
        {
            //Clear all charts from dockpanel
            int i = 0;
            foreach (object o in dock_Main.Children)
            {
                i++;
            }
            if(dock_Main.Children[i-1] is Chart)
                dock_Main.Children.RemoveAt(i-1);

            //Initialize new Series Collection
            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "A la mierda",
                    Values = new ChartValues<int> {0,5,10,50}
                }
            };

            Labels = new[] { "A", "B", "C", "D" };
            Formatter = value => value.ToString("N");

            //Initialize new chart
            CartesianChart cartesianChart = new CartesianChart
            {
                AxisX = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Eje x",
                    }
                },

                AxisY = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Eje y"
                    }
                }
            };

            //Set all necessary bindings
            cartesianChart.AxisX[0].SetBinding(Axis.LabelsProperty, new Binding { Source = this.Labels });
            cartesianChart.AxisY[0].SetBinding(Axis.LabelFormatterProperty, new Binding { Source = this.Formatter });
            cartesianChart.SetBinding(CartesianChart.SeriesProperty, new Binding { Source = this.SeriesCollection });

            //Set dock and add to DockPanel
            DockPanel.SetDock(cartesianChart, Dock.Bottom);
            dock_Main.Children.Add(cartesianChart);
        }

        //Request a CartesianChart, Vertical Bars, comparing Amount of registries per Op. Key
        private void Menu_VertBar_RegPerOpKey_Click(object sender, RoutedEventArgs e)
        {
            ChartEventArgs chartEventArgs = new ChartEventArgs { chartType = "VerticalBar_RegistryPerOpKey" };
            OnChartSelection(chartEventArgs);
            //Clear all charts from dockpanel
            int i = 0;
            foreach (object o in dock_Main.Children)
            {
                i++;
            }
            if (dock_Main.Children[i - 1] is Chart)
                dock_Main.Children.RemoveAt(i - 1);

            //Initialize new chart
            CartesianChart cartesianChart = new CartesianChart
            {
                AxisX = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Clave de operación",
                    }
                },

                AxisY = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Cantidad de registros"
                    }
                }
            };

            Labels = new[] { "A", "B", "C", "D", "E", "F", "G"};
            Formatter = value => value.ToString("N");

            //Set all necessary bindings
            cartesianChart.AxisX[0].SetBinding(Axis.LabelsProperty, new Binding { Source = this.Labels });
            cartesianChart.AxisY[0].SetBinding(Axis.LabelFormatterProperty, new Binding { Source = this.Formatter });
            cartesianChart.SetBinding(CartesianChart.SeriesProperty, new Binding { Source = this.SeriesCollection });

            //Set dock and add to DockPanel
            DockPanel.SetDock(cartesianChart, Dock.Bottom);
            dock_Main.Children.Add(cartesianChart);
            
        }
    }
}
