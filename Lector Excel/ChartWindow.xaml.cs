using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
//using System.Windows.Shapes;

using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Wpf.Charts.Base;
using Lector_Excel;

namespace Reader_347
{
    /// <summary>
    /// Clase de EventArgs usada por el delegado del visor de gráficos.
    /// </summary>
    public class ChartEventArgs : EventArgs
    {
        /// <value> El tipo de gráfico</value>
        public string chartType;
    }
    /// <summary>
    /// Delegado empleado por el visor de gráficos para la comunicación con <c>MainWindow</c>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ChartSetterDelegate(object sender, ChartEventArgs e);

    /// <summary>
    /// La clase de la ventana del visor de gráficos.
    /// </summary>
    public partial class ChartWindow : Window
    {
        ///<value> Obtiene o cambia la serie de colecciones de datos del gráfico actual.</value>
        public SeriesCollection SeriesCollection { get; set; }
        ///<value> Obtiene o cambia la lista de etiquetas del gráfico actual.</value>
        public string[] Labels { get; set; }
        ///<value> Obtiene o cambia el formateador del gráfico actual.</value>
        public Func<double, string> Formatter { get; set; }
        ///<value> Obtiene o cambia el formateador del gráfico de tarta actual.</value>
        public Func<ChartPoint, string> PieFormatter { get; set; }
        ///<value> Obtiene o cambia el diccionario de valores del gráfico de mapa actual.</value>
        public Dictionary<string,double> MapValues { get; set; }
        ///<value> Obtiene o cambia el tipo de gráfico actual.</value>
        public string ChartType { get; set; }
        ///<value> Obtiene o cambia el gráfico de mapa actual.</value>
        public GeoMap GeoMap { get; set; }

        /// <summary>
        /// Evento asociado al delegado del visor de gráficos.
        /// </summary>
        public event ChartSetterDelegate ChartDelegate;

        /// <summary>
        /// Inicializa una nueva instancia de <c>ChartWindow</c>.
        /// </summary>
        public ChartWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        //Sends info to parent window
        /// <summary>
        /// Invoca el delegado y envía información a <c>MainWindow</c>.
        /// </summary>
        /// <param name="e"></param>
        private void OnChartSelection(ChartEventArgs e)
        {
            dock_Main.Children.Remove(lbl_ChartNotSelected);
            if (!menu_SaveGraphAs.IsEnabled)
            {
                menu_SaveGraphAs.IsEnabled = true;
            }
            ChartDelegate?.Invoke(this, e);
        }

        //Request a CartesianChart, Vertical Bars, comparing Amount of registries per Op. Key
        /// <summary>
        /// Función de evento de click izquierdo asociado a "Mostrar gráfico de barras verticales".
        /// </summary>
        /// <remarks>El gráfico muestra los registros por clave de operación.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            if (dock_Main.Children[i - 1] is Chart || dock_Main.Children[i - 1] is GeoMap)
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
            this.ChartType = "VerticalBar_RegistryPerOpKey";
        }

        //Request a CartesianChart, Vertical Bars, showing money of buy/sell operation per trimester
        /// <summary>
        /// Función de evento de click izquierdo asociado a "Mostrar gráfico de barras verticales".
        /// </summary>
        /// <remarks>El gráfico muestra las compras/ventas por trimestre.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_VertBar_BuySellPerTrimester_Click(object sender, RoutedEventArgs e)
        {
            ChartEventArgs chartEventArgs = new ChartEventArgs { chartType = "VerticalBar_BuySellPerTrimester" };
            OnChartSelection(chartEventArgs);
            //Clear all charts from dockpanel
            int i = 0;
            foreach (object o in dock_Main.Children)
            {
                i++;
            }
            if (dock_Main.Children[i - 1] is Chart || dock_Main.Children[i - 1] is GeoMap)
                dock_Main.Children.RemoveAt(i - 1);

            //Initialize new chart
            CartesianChart cartesianChart = new CartesianChart
            {
                AxisX = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Trimestre",
                    }
                },

                AxisY = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Importe"
                    }
                }
            };

            Labels = new[] { "Trimestre 1", "Trimestre 2", "Trimestre 3", "Trimestre 4" };
            Formatter = value => value.ToString("N");

            //Set all necessary bindings
            cartesianChart.AxisX[0].SetBinding(Axis.LabelsProperty, new Binding { Source = this.Labels });
            cartesianChart.AxisY[0].SetBinding(Axis.LabelFormatterProperty, new Binding { Source = this.Formatter });
            cartesianChart.SetBinding(CartesianChart.SeriesProperty, new Binding { Source = this.SeriesCollection });

            //Set dock and add to DockPanel
            DockPanel.SetDock(cartesianChart, Dock.Bottom);
            dock_Main.Children.Add(cartesianChart);
            this.ChartType = "VerticalBar_BuySellPerTrimester";
        }

        //Request a CartesianChart, Horizontal Bars, comparing Amount of registries per Op. Key
        /// <summary>
        /// Función de evento de click izquierdo asociado a "Mostrar gráfico de barras horizontales".
        /// </summary>
        /// <remarks>El gráfico muestra los registros por clave de operación.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HorizBar_RegPerOpKey_Click(object sender, RoutedEventArgs e)
        {
            ChartEventArgs chartEventArgs = new ChartEventArgs { chartType = "HorizontalBar_RegistryPerOpKey" };
            OnChartSelection(chartEventArgs);
            //Clear all charts from dockpanel
            int i = 0;
            foreach (object o in dock_Main.Children)
            {
                i++;
            }
            if (dock_Main.Children[i - 1] is Chart || dock_Main.Children[i - 1] is GeoMap)
                dock_Main.Children.RemoveAt(i - 1);

            //Initialize new chart
            CartesianChart cartesianChart = new CartesianChart
            {
                AxisX = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Cantidad de registros",
                    }
                },

                AxisY = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Clave de operación"
                    }
                }
            };

            Labels = new[] { "A", "B", "C", "D", "E", "F", "G" };
            Formatter = value => value.ToString("N");

            //Set all necessary bindings
            cartesianChart.AxisX[0].SetBinding(Axis.LabelFormatterProperty, new Binding { Source = this.Formatter });
            cartesianChart.AxisY[0].SetBinding(Axis.LabelsProperty, new Binding { Source = this.Labels });
            cartesianChart.SetBinding(CartesianChart.SeriesProperty, new Binding { Source = this.SeriesCollection });

            //Set dock and add to DockPanel
            DockPanel.SetDock(cartesianChart, Dock.Bottom);
            dock_Main.Children.Add(cartesianChart);
            this.ChartType = "HorizontalBar_RegistryPerOpKey";
        }

        //Request a CartesianChart, Horizontal Bars, showing money of buy/sell operation per trimester
        /// <summary>
        /// Función de evento de click izquierdo asociado a "Mostrar gráfico de barras horizontales".
        /// </summary>
        /// <remarks>El gráfico muestra las compras/ventas por trimestre.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HorizBar_BuySellPerTrimester_Click(object sender, RoutedEventArgs e)
        {
            ChartEventArgs chartEventArgs = new ChartEventArgs { chartType = "HorizontalBar_BuySellPerTrimester" };
            OnChartSelection(chartEventArgs);
            //Clear all charts from dockpanel
            int i = 0;
            foreach (object o in dock_Main.Children)
            {
                i++;
            }
            if (dock_Main.Children[i - 1] is Chart || dock_Main.Children[i - 1] is GeoMap)
                dock_Main.Children.RemoveAt(i - 1);

            //Initialize new chart
            CartesianChart cartesianChart = new CartesianChart
            {
                AxisX = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Importe",
                    }
                },

                AxisY = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Trimestre"
                    }
                }
            };

            Labels = new[] { "Trimestre 1", "Trimestre 2", "Trimestre 3", "Trimestre 4" };
            Formatter = value => value.ToString("N");

            //Set all necessary bindings
            cartesianChart.AxisX[0].SetBinding(Axis.LabelFormatterProperty, new Binding { Source = this.Formatter });
            cartesianChart.AxisY[0].SetBinding(Axis.LabelsProperty, new Binding { Source = this.Labels });
            cartesianChart.SetBinding(CartesianChart.SeriesProperty, new Binding { Source = this.SeriesCollection });

            //Set dock and add to DockPanel
            DockPanel.SetDock(cartesianChart, Dock.Bottom);
            dock_Main.Children.Add(cartesianChart);
            this.ChartType = "HorizontalBar_BuySellPerTrimester";
        }

        //Request a CartesianChart, Lines, showing money of buy/sell operation per trimester
        /// <summary>
        /// Función de evento de click izquierdo asociado a "Mostrar gráfico de líneas".
        /// </summary>
        /// <remarks>El gráfico muestra las compras/ventas por trimestre.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_Line_BuySellPerTrimester_Click(object sender, RoutedEventArgs e)
        {
            ChartEventArgs chartEventArgs = new ChartEventArgs { chartType = "Line_BuySellPerTrimester" };
            OnChartSelection(chartEventArgs);
            //Clear all charts from dockpanel
            int i = 0;
            foreach (object o in dock_Main.Children)
            {
                i++;
            }
            if (dock_Main.Children[i - 1] is Chart || dock_Main.Children[i - 1] is GeoMap)
                dock_Main.Children.RemoveAt(i - 1);

            //Initialize new chart
            CartesianChart cartesianChart = new CartesianChart
            {
                AxisX = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Trimestre",
                    }
                },

                AxisY = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Importe"
                    }
                }
            };

            Labels = new[] { "Trimestre 1", "Trimestre 2", "Trimestre 3", "Trimestre 4" };
            Formatter = value => value.ToString("N");

            //Set all necessary bindings
            cartesianChart.AxisX[0].SetBinding(Axis.LabelsProperty, new Binding { Source = this.Labels });
            cartesianChart.AxisY[0].SetBinding(Axis.LabelFormatterProperty, new Binding { Source = this.Formatter });
            cartesianChart.SetBinding(CartesianChart.SeriesProperty, new Binding { Source = this.SeriesCollection });

            //Set dock and add to DockPanel
            DockPanel.SetDock(cartesianChart, Dock.Bottom);
            dock_Main.Children.Add(cartesianChart);
            this.ChartType = "Line_BuySellPerTrimester";
        }

        //Request a Geo Map, showing money of buy operations per region
        /// <summary>
        /// Función de evento de click izquierdo asociado a "Mostrar gráfico de mapa".
        /// </summary>
        /// <remarks>El gráfico muestra las compras por región.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_Map_BuyTotal_Click(object sender, RoutedEventArgs e)
        {
            ChartEventArgs chartEventArgs = new ChartEventArgs { chartType = "Map_BuyTotal" };
            OnChartSelection(chartEventArgs);
            //Clear all charts from dockpanel
            int i = 0;
            foreach (object o in dock_Main.Children)
            {
                i++;
            }

            if (dock_Main.Children[i - 1] is Chart || dock_Main.Children[i - 1] is GeoMap)
                dock_Main.Children.RemoveAt(i - 1);

            //Initialize new chart
            GeoMap = new GeoMap
            {
                Source = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources\\Spain.xml"),
                HeatMap = MapValues,
                Hoverable = true
            };
            

            //geoMap.SetBinding(GeoMap.HeatMapProperty, new Binding { Source = this.MapValues });
            //Set dock and add to DockPanel
            DockPanel.SetDock(GeoMap, Dock.Bottom);
            dock_Main.Children.Add(GeoMap);
            this.ChartType = "Map_BuyTotal";
        }

        //Request a Geo Map, showing money of sell operations per region
        /// <summary>
        /// Función de evento de click izquierdo asociado a "Mostrar gráfico de mapa".
        /// </summary>
        /// <remarks>El gráfico muestra las ventas por región.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_Map_SellTotal_Click(object sender, RoutedEventArgs e)
        {
            ChartEventArgs chartEventArgs = new ChartEventArgs { chartType = "Map_SellTotal" };
            OnChartSelection(chartEventArgs);
            //Clear all charts from dockpanel
            int i = 0;
            foreach (object o in dock_Main.Children)
            {
                i++;
            }

            if (dock_Main.Children[i - 1] is Chart || dock_Main.Children[i - 1] is GeoMap)
                dock_Main.Children.RemoveAt(i - 1);

            //Initialize new chart
            GeoMap = new GeoMap
            {
                Source = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources\\Spain.xml"),
                HeatMap = MapValues,
                Hoverable = true,
                DefaultLandFill = Brushes.White
            };


            //geoMap.SetBinding(GeoMap.HeatMapProperty, new Binding { Source = this.MapValues });
            //Set dock and add to DockPanel
            DockPanel.SetDock(GeoMap, Dock.Bottom);
            dock_Main.Children.Add(GeoMap);
            this.ChartType = "Map_SellTotal";
        }

        //Request Pie Chart, showing money of buy operations per region
        /// <summary>
        /// Función de evento de click izquierdo asociado a "Mostrar gráfico de tarta".
        /// </summary>
        /// <remarks>El gráfico muestra las compras por región.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_Pie_BuyPerRegion_Click(object sender, RoutedEventArgs e)
        {
            ChartEventArgs chartEventArgs = new ChartEventArgs { chartType = "Pie_BuyTotal" };
            OnChartSelection(chartEventArgs);

            //Clear all charts from dockpanel
            int i = 0;
            foreach (object o in dock_Main.Children)
            {
                i++;
            }

            if (dock_Main.Children[i - 1] is Chart || dock_Main.Children[i - 1] is GeoMap)
                dock_Main.Children.RemoveAt(i - 1);

            

            PieChart pieChart = new PieChart
            {
                LegendLocation = LegendLocation.Right,
                Hoverable = true,
            };

            pieChart.SetBinding(PieChart.SeriesProperty, new Binding { Source = this.SeriesCollection });
            DockPanel.SetDock(pieChart, Dock.Bottom);
            dock_Main.Children.Add(pieChart);
            this.ChartType = "Pie_BuyTotal";
        }

        //Request a Pie Chart, showing money of sell operations per region
        /// <summary>
        /// Función de evento de click izquierdo asociado a "Mostrar gráfico de tarta".
        /// </summary>
        /// <remarks>El gráfico muestra las ventas por región.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_Pie_SellPerRegion_Click(object sender, RoutedEventArgs e)
        {
            ChartEventArgs chartEventArgs = new ChartEventArgs { chartType = "Pie_SellTotal" };
            OnChartSelection(chartEventArgs);

            //Clear all charts from dockpanel
            int i = 0;
            foreach (object o in dock_Main.Children)
            {
                i++;
            }

            if (dock_Main.Children[i - 1] is Chart || dock_Main.Children[i - 1] is GeoMap)
                dock_Main.Children.RemoveAt(i - 1);



            PieChart pieChart = new PieChart
            {
                LegendLocation = LegendLocation.Right,
                Hoverable = true,
            };

            pieChart.SetBinding(PieChart.SeriesProperty, new Binding { Source = this.SeriesCollection });
            DockPanel.SetDock(pieChart, Dock.Bottom);
            dock_Main.Children.Add(pieChart);
            this.ChartType = "Pie_SellTotal";
        }

        //Saves the current graph as the desired format
        /// <summary>
        /// Función de evento de click izquierdo asociado a "Guardar gráfico".
        /// </summary>
        /// <remarks>Permite guardar el gráfico como imagen o PDF.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <see cref="PDFManager.CreatePDFWithImage(MemoryStream, ChartDataHolder)"/>
        private void Menu_SaveGraphAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Informe PDF (*.pdf)|*.pdf|Imagen PNG (*.png)|*.png";
            saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (saveDialog.ShowDialog() == true)
            {
                switch (saveDialog.FileName.Substring(saveDialog.FileName.Length-3, 3).ToLower())
                {
                    case "pdf":
                        PDFManager manager = new PDFManager(saveDialog.FileName);
                        ChartDataHolder chd = new ChartDataHolder(this.ChartType, dock_Main.Children.OfType<Chart>().DefaultIfEmpty(null).FirstOrDefault(), dock_Main.Children.OfType<GeoMap>().DefaultIfEmpty(null).FirstOrDefault());
                        manager.CreatePDFWithImage(CaptureScreen(this), chd);
                        break;
                    case "png":
                        PngBitmapEncoder encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(CaptureScreen(this)));
                        using (FileStream stream = new FileStream(saveDialog.FileName, FileMode.Create, FileAccess.Write))
                        {
                            encoder.Save(stream);
                        }
                        break;
                    default:
                        break;
                }
                
            }
        }

        //Capture UIElement and save as MemoryStream
        /// <summary>
        /// Hace una captura de pantalla para guardar el gráfico.
        /// </summary>
        /// <param name="source"> El elemento a capturar.</param>
        /// <returns> La captura en forma de <c>MemoryStream</c></returns>
        private MemoryStream CaptureScreen(UIElement source)
        {
            try
            {
                double Height, renderHeight, Width, renderWidth;

                Height = renderHeight = source.RenderSize.Height;
                Width = renderWidth = source.RenderSize.Width;

                //Specification for target bitmap like width/height pixel etc.
                RenderTargetBitmap renderTarget = new RenderTargetBitmap((int)renderWidth, (int)renderHeight, 96, 96, PixelFormats.Pbgra32);
                //creates Visual Brush of UIElement
                VisualBrush visualBrush = new VisualBrush(source);

                DrawingVisual drawingVisual = new DrawingVisual();
                using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                {
                    //draws image of element
                    drawingContext.DrawRectangle(visualBrush, null, new Rect(new Point(0, 0), new Point(Width, Height)));
                }
                //renders image
                renderTarget.Render(drawingVisual);

                //PNG encoder for creating PNG file
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderTarget));
                MemoryStream stream = new MemoryStream();
                encoder.Save(stream);

                return stream;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message,"ERROR",MessageBoxButton.OK,MessageBoxImage.Error);
                return null;
            }
        }
    }
    /// <summary>
    /// Clase auxiliar empleada para almacenar datos de un gráfico.
    /// </summary>
    public class ChartDataHolder
    {
        /// <value> La lista de datos del gráfico.</value>
        public List<string> ChartData;
        ///<value> El tipo de gráfico.</value>
        private string ChartType;
        ///<value> La referencia al gráfico.</value>
        private Chart CurrentChart;
        ///<value> La referencia al gráfico de mapa.</value>
        private GeoMap CurrentMapChart;

        /// <summary>
        /// Inicializa una nueva instancia de <c>ChartDataHolder</c>.
        /// </summary>
        /// <param name="chartType">El tipo de gráfico.</param>
        /// <param name="chart">La referencia al gráfico.</param>
        /// <param name="map">La referencia al gráfico de mapa.</param>
        public ChartDataHolder(string chartType, Chart chart = null, GeoMap map = null)
        {
            CurrentChart = chart;
            CurrentMapChart = map;
            ChartType = chartType;

            OrganizeChartData();
        }

        /// <summary>
        /// Recoge la información del gráfico y la organiza para su posterior representación.
        /// </summary>
        private void OrganizeChartData()
        {
            if(ChartData is null)
            {
                ChartData = new List<string>();
            }
            else
            {
                ChartData.Clear();
            }

            if (CurrentChart != null)
            {
                switch (ChartType)
                {
                    case "VerticalBar_RegistryPerOpKey":
                        ChartData.Add("Informe de registros por clave de operación");
                        int index = 0;
                        foreach (int v in CurrentChart.Series[0].Values)
                        {
                            ChartData.Add("Registros de tipo " + CurrentChart.AxisX[0].Labels[index++] + ": " + v);
                        }
                        break;
                    case "HorizontalBar_RegistryPerOpKey":
                        ChartData.Add("Informe de registros por clave de operación");
                        index = 0;
                        foreach (int v in CurrentChart.Series[0].Values)
                        {
                            ChartData.Add("Registros de tipo " + CurrentChart.AxisY[0].Labels[index++] + ": " + v);
                        }
                        break;
                    case "Line_BuySellPerTrimester":
                    case "VerticalBar_BuySellPerTrimester":
                        ChartData.Add("Informe de compra-venta por trimestre");
                        index = 0;
                        foreach(float v in CurrentChart.Series[0].Values)
                        {
                            ChartData.Add("Compras en Trimestre "+CurrentChart.AxisX[0].Labels[index] + ": " + v);
                            ChartData.Add("Ventas en Trimestre " + CurrentChart.AxisX[0].Labels[index] + ": " + CurrentChart.Series[1].Values[index++]);
                        }
                        break;
                    case "HorizontalBar_BuySellPerTrimester":
                        ChartData.Add("Informe de compra-venta por trimestre");
                        index = 0;
                        foreach (float v in CurrentChart.Series[0].Values)
                        {
                            ChartData.Add("Compras en Trimestre " + CurrentChart.AxisY[0].Labels[index] + ": " + v);
                            ChartData.Add("Ventas en Trimestre " + CurrentChart.AxisY[0].Labels[index] + ": " + CurrentChart.Series[1].Values[index++]);
                        }
                        break;
                    case "Pie_BuyTotal":
                        ChartData.Add("Informe de compras por región");
                        foreach(object o in CurrentChart.Series)
                        {
                            PieSeries currentPieSlice = o as PieSeries;
                            ChartData.Add("Compras en " + currentPieSlice.Title + ": "+ currentPieSlice.Values[0]);
                        }
                        break;
                    case "Pie_SellTotal":
                        ChartData.Add("Informe de ventas por región");
                        foreach (object o in CurrentChart.Series)
                        {
                            PieSeries currentPieSlice = o as PieSeries;
                            ChartData.Add("Ventas en " + currentPieSlice.Title + ": " + currentPieSlice.Values[0]);
                        }
                        break;
                }
            }
            else if(CurrentMapChart != null)
            {
                switch (ChartType)
                {
                    case "Map_BuyTotal":
                        ChartData.Add("Mapa de compras por región");
                        foreach(KeyValuePair<string,double> o in CurrentMapChart.HeatMap)
                        {
                            ChartData.Add("Compras en " + Province.CodeToName(o.Key) + ": " + o.Value);
                        }
                        break;
                    case "Map_SellTotal":
                        ChartData.Add("Mapa de ventas por región");
                        foreach (KeyValuePair<string, double> o in CurrentMapChart.HeatMap)
                        {
                            ChartData.Add("Ventas en " + Province.CodeToName(o.Key) + ": " + o.Value);
                        }
                        break;
                }
            }
        }
    }
}
