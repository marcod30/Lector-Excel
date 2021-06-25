using Reader_347.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;
namespace Lector_Excel
{
    /// <summary>
    /// Clase del gestor de archivos Excel.
    /// </summary>
    public class ExcelManager
    {
        string path;
        ///<value>La referencia al proceso de Excel.</value>
        Excel.Application excelApp;
        ///<value>La referencia a la colección de libros.</value>
        Excel.Workbooks workbooks;
        ///<value>La referencia al libro.</value>
        Excel.Workbook workbook;
        ///<value>La referencia a la hoja.</value>
        Excel._Worksheet worksheet;
        ///<value>La referencia al rango de celdas.</value>
        Excel.Range range;
                                            // Excel is not 0 based, thus the array's first position is not used
        private readonly int[] longitudes = {-1, 9, 9, 40, 1, 2, 2, 1, 1, 16, 1, 1, 15, 16, 4, 16, 16, 16, 16, 16, 16, 16, 16, 17, 1, 1, 1, 16, 201 };
        const int MAX_ALLOWED_COLUMNS = 28; // Model 347 has 28 data fields only, so if further data is found, it will be ignored

        /// <summary>
        /// Inicializa una nueva instancia de <c>ExcelManager</c>.
        /// </summary>
        /// <param name="path"> El directorio del archivo Excel.</param>
        public ExcelManager(string path)
        {
            
            this.path = path;
        }

        /// <summary>
        /// Obtiene los datos en bruto del archivo Excel.
        /// </summary>
        /// <param name="Positions"> Las celdas de Excel en las que se encuentran los datos.</param>
        /// <param name="bw"> El <c>BackgroundWorker</c> encargado de reportar el progreso.</param>
        /// <returns> Una lista con estructuras <c>Declared</c>.</returns>
        public List<Declared> ImportExcelData(ExcelSettings Positions, BackgroundWorker bw)
        {
            try
            {
                excelApp = new Excel.Application();
                workbooks = excelApp.Workbooks;
                workbook = workbooks.Open(path);

                int rows, columns;
                Debug.WriteLine("STARTING IMPORT FROM "+this.path);

                worksheet = workbook.Sheets[1];
                range = worksheet.UsedRange;
                rows = range.Rows.Count;
                columns = range.Columns.Count;

                Debug.WriteLine("El excel tiene " + rows + " filas y " + columns + " columnas");

                List<Declared> returnList = new List<Declared>();
                for (int i = 2; i <= rows; i++) // We skip the row that contains the header?
                {
                    Declared declared = new Declared();

                    declared.declaredData["DeclaredNIF"] = (range.Cells[i, Positions.DeclaredNIF].Value2 != null) ? range.Cells[i, Positions.DeclaredNIF].Value2.ToString() : "";
                    declared.declaredData["LegalRepNIF"] = (range.Cells[i, Positions.LegalRepNIF].Value2 != null) ? range.Cells[i, Positions.LegalRepNIF].Value2.ToString() : "";
                    declared.declaredData["CommunityOpNIF"] = (range.Cells[i, Positions.CommunityOpNIF].Value2 != null) ? range.Cells[i, Positions.CommunityOpNIF].Value2.ToString() : "";
                    declared.declaredData["DeclaredName"] = (range.Cells[i, Positions.DeclaredName].Value2 != null) ? range.Cells[i, Positions.DeclaredName].Value2.ToString() : "";
                    declared.declaredData["ProvinceCode"] = (range.Cells[i, Positions.ProvinceCode].Value2 != null) ? range.Cells[i, Positions.ProvinceCode].Value2.ToString() : "";
                    declared.declaredData["CountryCode"] = (range.Cells[i, Positions.StateCode].Value2 != null) ? range.Cells[i, Positions.StateCode].Value2.ToString() : "";
                    declared.declaredData["OpKey"] = (range.Cells[i, Positions.OpKey].Value2 != null) ? range.Cells[i, Positions.OpKey].Value2.ToString() : "";
                    declared.declaredData["OpInsurance"] = (range.Cells[i, Positions.OpInsurance].Value2 != null) ? range.Cells[i, Positions.OpInsurance].Value2.ToString() : "";
                    declared.declaredData["LocalBusinessLease"] = (range.Cells[i, Positions.LocalBusinessRental].Value2 != null) ? range.Cells[i, Positions.LocalBusinessRental].Value2.ToString() : "";
                    declared.declaredData["OpIVA"] = (range.Cells[i, Positions.OpSpecialRegIVA].Value2 != null) ? range.Cells[i, Positions.OpSpecialRegIVA].Value2.ToString() : "";
                    declared.declaredData["OpPassive"] = (range.Cells[i, Positions.OpPassive].Value2 != null) ? range.Cells[i, Positions.OpPassive].Value2.ToString() : "";
                    declared.declaredData["OpCustoms"] = (range.Cells[i, Positions.OpRegNotCustoms].Value2 != null) ? range.Cells[i, Positions.OpRegNotCustoms].Value2.ToString() : "";
                    declared.declaredData["TotalMoney"] = (range.Cells[i, Positions.MetalMoney].Value2 != null) ? range.Cells[i, Positions.MetalMoney].Value2.ToString() : "";
                    declared.declaredData["AnualMoney"] = (range.Cells[i, Positions.AnualOpMoney].Value2 != null) ? range.Cells[i, Positions.AnualOpMoney].Value2.ToString() : "";
                    declared.declaredData["AnualPropertyMoney"] = (range.Cells[i, Positions.AnualPropertyTransmissionIVA].Value2 != null) ? range.Cells[i, Positions.AnualPropertyTransmissionIVA].Value2.ToString() : "";
                    declared.declaredData["AnualOpIVA"] = (range.Cells[i, Positions.AnualMoneyDevengedIVA].Value2 != null) ? range.Cells[i, Positions.AnualMoneyDevengedIVA].Value2.ToString() : "";
                    declared.declaredData["Exercise"] = (range.Cells[i, Positions.Exercise].Value2 != null) ? range.Cells[i, Positions.Exercise].Value2.ToString() : "" ;
                    declared.declaredData["TrimestralOp1"] = (range.Cells[i, Positions.OpMoney1T].Value2 != null) ? range.Cells[i, Positions.OpMoney1T].Value2.ToString() : "";
                    declared.declaredData["TrimestralOp2"] = (range.Cells[i, Positions.OpMoney2T].Value2 != null) ? range.Cells[i, Positions.OpMoney2T].Value2.ToString() : "";
                    declared.declaredData["TrimestralOp3"] = (range.Cells[i, Positions.OpMoney3T].Value2 != null) ? range.Cells[i, Positions.OpMoney3T].Value2.ToString() : "";
                    declared.declaredData["TrimestralOp4"] = (range.Cells[i, Positions.OpMoney4T].Value2 != null) ? range.Cells[i, Positions.OpMoney4T].Value2.ToString() : "";
                    declared.declaredData["AnualPropertyIVAOp1"] = (range.Cells[i, Positions.PropertyTransmissionIVA1T].Value2 != null) ? range.Cells[i, Positions.PropertyTransmissionIVA1T].Value2.ToString() : "";
                    declared.declaredData["AnualPropertyIVAOp2"] = (range.Cells[i, Positions.PropertyTransmissionIVA2T].Value2 != null) ? range.Cells[i, Positions.PropertyTransmissionIVA2T].Value2.ToString() : "";
                    declared.declaredData["AnualPropertyIVAOp3"] = (range.Cells[i, Positions.PropertyTransmissionIVA3T].Value2 != null) ? range.Cells[i, Positions.PropertyTransmissionIVA3T].Value2.ToString() : "";
                    declared.declaredData["AnualPropertyIVAOp4"] = (range.Cells[i, Positions.PropertyTransmissionIVA4T].Value2 != null) ? range.Cells[i, Positions.PropertyTransmissionIVA4T].Value2.ToString() : "";

                    returnList.Add(declared);

                    float progress = (float)i / rows * 100;
                    Debug.WriteLine(progress + "%");
                    bw.ReportProgress((int)progress);
                }
                
                return returnList;
            }
            
            catch (Exception e)
            {
                MessageBoxResult msg = MessageBox.Show("Ha ocurrido un error. La importación se interrumpirá.\nCódigo del error: "+e, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            
            finally
            {
                //cleanup
                GC.Collect();
                GC.WaitForPendingFinalizers();

                //rule of thumb for releasing com objects:
                //  never use two dots, all COM objects must be referenced and released individually
                //  ex: [somthing].[something].[something] is bad

                //release com objects to fully kill excel process from running in the background
                Marshal.ReleaseComObject(range);
                Marshal.ReleaseComObject(worksheet);

                //close and release
                workbook.Close();
                Marshal.ReleaseComObject(workbooks);
                Marshal.ReleaseComObject(workbook);

                //quit and release
                excelApp.Quit();
                Marshal.ReleaseComObject(excelApp);
            }
        }

        /// <summary>
        /// Exporta el contenido a un archivo Excel.
        /// </summary>
        /// <param name="Positions"> Las celdas de Excel donde irán los datos.</param>
        /// <param name="declareds"> La lista con los declarados a exportar.</param>
        /// <param name="bw"> El <c>BackgroundWorker</c> encargado de reportar el progreso.</param>
        /// <returns> True si la exportación se hizo con éxito. </returns>
        public bool ExportToExcel(ExcelSettings Positions, List<Declared> declareds, BackgroundWorker bw)
        {
            try
            {
                excelApp = new Excel.Application();
                excelApp.Visible = false;

                workbooks = excelApp.Workbooks;
                workbook = workbooks.Add();
                worksheet = excelApp.ActiveSheet;

                Debug.WriteLine("STARTING EXPORT TO " + this.path);

                range = worksheet.UsedRange;

                int titleRow = 2;
                if (!Positions.FirstRowIsTitle)
                {
                    titleRow = 0;

                    //Set title row
                    worksheet.Rows[1].Font.Bold = true;

                    worksheet.Cells[1, Positions.DeclaredNIF] = "NIF Declarado";
                    worksheet.Cells[1, Positions.LegalRepNIF] = "NIF Rep. Legal";
                    worksheet.Cells[1, Positions.CommunityOpNIF] = "NIF Op. Comunitario";
                    worksheet.Cells[1, Positions.DeclaredName] = "Nombre Declarado";
                    worksheet.Cells[1, Positions.ProvinceCode] = "Cod. Provincia";
                    worksheet.Cells[1, Positions.StateCode] = "Cod. Pais";
                    worksheet.Cells[1, Positions.OpKey] = "Clave Op.";
                    worksheet.Cells[1, Positions.OpInsurance] = "Op. Seguros";
                    worksheet.Cells[1, Positions.LocalBusinessRental] = "Arr. local negocio";
                    worksheet.Cells[1, Positions.OpSpecialRegIVA] = "Op. IVA caja";
                    worksheet.Cells[1, Positions.OpPassive] = "Op. Inv. sujeto pasivo";
                    worksheet.Cells[1, Positions.OpRegNotCustoms] = "Op. Reg. no aduanero";
                    worksheet.Cells[1, Positions.MetalMoney] = "Importe metalico";
                    worksheet.Cells[1, Positions.AnualOpMoney] = "Importe anual";
                    worksheet.Cells[1, Positions.AnualPropertyTransmissionIVA] = "Imp. anual por t. de inmueble";
                    worksheet.Cells[1, Positions.AnualMoneyDevengedIVA] = "Imp. anual de op. devengadas";
                    worksheet.Cells[1, Positions.Exercise] = "Ejercicio";
                    worksheet.Cells[1, Positions.OpMoney1T] = "Imp. op. T1";
                    worksheet.Cells[1, Positions.OpMoney2T] = "Imp. op. T2";
                    worksheet.Cells[1, Positions.OpMoney3T] = "Imp. op. T3";
                    worksheet.Cells[1, Positions.OpMoney4T] = "Imp. op. T4";
                    worksheet.Cells[1, Positions.PropertyTransmissionIVA1T] = "Imp. anual t. de inmueble T1";
                    worksheet.Cells[1, Positions.PropertyTransmissionIVA2T] = "Imp. anual t. de inmueble T1";
                    worksheet.Cells[1, Positions.PropertyTransmissionIVA3T] = "Imp. anual t. de inmueble T1";
                    worksheet.Cells[1, Positions.PropertyTransmissionIVA4T] = "Imp. anual t. de inmueble T1";
                }

                for (int i = 0; i < declareds.Count; i++)
                {
                    worksheet.Cells[i + titleRow, Positions.DeclaredNIF] = declareds[i].declaredData["DeclaredNIF"];
                    worksheet.Cells[i + titleRow, Positions.LegalRepNIF] = declareds[i].declaredData["LegalRepNIF"];
                    worksheet.Cells[i + titleRow, Positions.CommunityOpNIF] = declareds[i].declaredData["CommunityOpNIF"];
                    worksheet.Cells[i + titleRow, Positions.DeclaredName] = declareds[i].declaredData["DeclaredName"];
                    worksheet.Cells[i + titleRow, Positions.ProvinceCode] = declareds[i].declaredData["ProvinceCode"];
                    worksheet.Cells[i + titleRow, Positions.StateCode] = declareds[i].declaredData["CountryCode"];
                    worksheet.Cells[i + titleRow, Positions.OpKey] = declareds[i].declaredData["OpKey"];
                    worksheet.Cells[i + titleRow, Positions.OpInsurance] = declareds[i].declaredData["OpInsurance"];
                    worksheet.Cells[i + titleRow, Positions.LocalBusinessRental] = declareds[i].declaredData["LocalBusinessLease"];
                    worksheet.Cells[i + titleRow, Positions.OpSpecialRegIVA] = declareds[i].declaredData["OpIVA"];
                    worksheet.Cells[i + titleRow, Positions.OpPassive] = declareds[i].declaredData["OpPassive"];
                    worksheet.Cells[i + titleRow, Positions.OpRegNotCustoms] = declareds[i].declaredData["OpCustoms"];
                    worksheet.Cells[i + titleRow, Positions.MetalMoney] = declareds[i].declaredData["TotalMoney"];
                    worksheet.Cells[i + titleRow, Positions.AnualOpMoney] = declareds[i].declaredData["AnualMoney"];
                    worksheet.Cells[i + titleRow, Positions.AnualPropertyTransmissionIVA] = declareds[i].declaredData["AnualPropertyMoney"];
                    worksheet.Cells[i + titleRow, Positions.AnualMoneyDevengedIVA] = declareds[i].declaredData["AnualOpIVA"];
                    worksheet.Cells[i + titleRow, Positions.Exercise] = declareds[i].declaredData["Exercise"];
                    worksheet.Cells[i + titleRow, Positions.OpMoney1T] = declareds[i].declaredData["TrimestralOp1"];
                    worksheet.Cells[i + titleRow, Positions.OpMoney2T] = declareds[i].declaredData["TrimestralOp2"];
                    worksheet.Cells[i + titleRow, Positions.OpMoney3T] = declareds[i].declaredData["TrimestralOp3"];
                    worksheet.Cells[i + titleRow, Positions.OpMoney4T] = declareds[i].declaredData["TrimestralOp4"];
                    worksheet.Cells[i + titleRow, Positions.PropertyTransmissionIVA1T] = declareds[i].declaredData["AnualPropertyIVAOp1"];
                    worksheet.Cells[i + titleRow, Positions.PropertyTransmissionIVA2T] = declareds[i].declaredData["AnualPropertyIVAOp2"];
                    worksheet.Cells[i + titleRow, Positions.PropertyTransmissionIVA3T] = declareds[i].declaredData["AnualPropertyIVAOp3"];
                    worksheet.Cells[i + titleRow, Positions.PropertyTransmissionIVA4T] = declareds[i].declaredData["AnualPropertyIVAOp4"];


                    //Report progress through BackgroundWorker
                    float progress = (float)i / declareds.Count * 100;
                    Debug.WriteLine(progress + "%");
                    bw.ReportProgress((int)progress);
                }

                //Set column size to fit text
                for(int i = 1; i <= declareds[0].declaredData.Keys.Count; i++)
                {
                    worksheet.Columns[i].AutoFit();
                }

                //Disable alerts so overwrite popup does not appear
                excelApp.DisplayAlerts = false; 
                //Save document with this spaghet
                workbook.SaveAs(path, Excel.XlFileFormat.xlOpenXMLWorkbook, System.Reflection.Missing.Value, System.Reflection.Missing.Value, false, false, Excel.XlSaveAsAccessMode.xlNoChange,
                    Excel.XlSaveConflictResolution.xlUserResolution, true, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value);

                //Reenable alerts
                excelApp.DisplayAlerts = true;
                return true;
            }

            catch (Exception e)
            {
                MessageBoxResult msg = MessageBox.Show("Ha ocurrido un error. La exportación se interrumpirá.\nCódigo del error: " + e, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            finally
            {
                //cleanup
                GC.Collect();
                GC.WaitForPendingFinalizers();

                //rule of thumb for releasing com objects:
                //  never use two dots, all COM objects must be referenced and released individually
                //  ex: [somthing].[something].[something] is bad

                //release com objects to fully kill excel process from running in the background
                Marshal.ReleaseComObject(range);
                Marshal.ReleaseComObject(worksheet);

                //close and release
                workbook.Close();
                Marshal.ReleaseComObject(workbooks);
                Marshal.ReleaseComObject(workbook);

                //quit and release
                excelApp.Quit();
                Marshal.ReleaseComObject(excelApp);
            }
        }
    }
}
