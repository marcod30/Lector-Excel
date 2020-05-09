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
    public class ExcelManager
    {
        string path;
        Excel.Application excelApp;
        Excel.Workbooks workbooks;
        Excel.Workbook workbook;
        Excel._Worksheet worksheet;
        Excel.Range range;
                                            // Excel is not 0 based, thus the array's first position is not used
        private readonly int[] longitudes = {-1, 9, 9, 40, 1, 2, 2, 1, 1, 16, 1, 1, 15, 16, 4, 16, 16, 16, 16, 16, 16, 16, 16, 17, 1, 1, 1, 16, 201 };
        const int MAX_ALLOWED_COLUMNS = 28; // Model 347 has 28 data fields only, so if further data is found, it will be ignored

        public ExcelManager(string path)
        {
            
            this.path = path;
        }

        public List<Declared> ImportExcelData(List<string> Positions, BackgroundWorker bw)
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

                    declared.declaredData["DeclaredNIF"] = (range.Cells[i, Positions[0]].Value2 != null) ? range.Cells[i, Positions[0]].Value2.ToString() : "";
                    declared.declaredData["LegalRepNIF"] = (range.Cells[i, Positions[1]].Value2 != null) ? range.Cells[i, Positions[1]].Value2.ToString() : "";
                    declared.declaredData["CommunityOpNIF"] = (range.Cells[i, Positions[20]].Value2 != null) ? range.Cells[i, Positions[20]].Value2.ToString() : "";
                    declared.declaredData["DeclaredName"] = (range.Cells[i, Positions[2]].Value2 != null) ? range.Cells[i, Positions[2]].Value2.ToString() : "";
                    declared.declaredData["ProvinceCode"] = (range.Cells[i, Positions[3]].Value2 != null) ? range.Cells[i, Positions[3]].Value2.ToString() : "";
                    declared.declaredData["CountryCode"] = (range.Cells[i, Positions[4]].Value2 != null) ? range.Cells[i, Positions[4]].Value2.ToString() : "";
                    declared.declaredData["OpKey"] = (range.Cells[i, Positions[5]].Value2 != null) ? range.Cells[i, Positions[5]].Value2.ToString() : "";
                    declared.declaredData["OpInsurance"] = (range.Cells[i, Positions[7]].Value2 != null) ? range.Cells[i, Positions[7]].Value2.ToString() : "";
                    declared.declaredData["LocalBusinessLease"] = (range.Cells[i, Positions[8]].Value2 != null) ? range.Cells[i, Positions[8]].Value2.ToString() : "";
                    declared.declaredData["OpIVA"] = (range.Cells[i, Positions[21]].Value2 != null) ? range.Cells[i, Positions[21]].Value2.ToString() : "";
                    declared.declaredData["OpPassive"] = (range.Cells[i, Positions[22]].Value2 != null) ? range.Cells[i, Positions[22]].Value2.ToString() : "";
                    declared.declaredData["OpCustoms"] = (range.Cells[i, Positions[23]].Value2 != null) ? range.Cells[i, Positions[23]].Value2.ToString() : "";
                    declared.declaredData["TotalMoney"] = (range.Cells[i, Positions[9]].Value2 != null) ? range.Cells[i, Positions[9]].Value2.ToString() : "";
                    declared.declaredData["AnualMoney"] = (range.Cells[i, Positions[6]].Value2 != null) ? range.Cells[i, Positions[6]].Value2.ToString() : "";
                    declared.declaredData["AnualPropertyMoney"] = (range.Cells[i, Positions[10]].Value2 != null) ? range.Cells[i, Positions[10]].Value2.ToString() : "";
                    declared.declaredData["AnualOpIVA"] = (range.Cells[i, Positions[24]].Value2 != null) ? range.Cells[i, Positions[24]].Value2.ToString() : "";
                    declared.declaredData["Exercise"] = (range.Cells[i, Positions[11]].Value2 != null) ? range.Cells[i, Positions[11]].Value2.ToString() : "" ;
                    declared.declaredData["TrimestralOp1"] = (range.Cells[i, Positions[12]].Value2 != null) ? range.Cells[i, Positions[12]].Value2.ToString() : "";
                    declared.declaredData["TrimestralOp2"] = (range.Cells[i, Positions[14]].Value2 != null) ? range.Cells[i, Positions[14]].Value2.ToString() : "";
                    declared.declaredData["TrimestralOp3"] = (range.Cells[i, Positions[16]].Value2 != null) ? range.Cells[i, Positions[16]].Value2.ToString() : "";
                    declared.declaredData["TrimestralOp4"] = (range.Cells[i, Positions[18]].Value2 != null) ? range.Cells[i, Positions[18]].Value2.ToString() : "";
                    declared.declaredData["AnualPropertyIVAOp1"] = (range.Cells[i, Positions[13]].Value2 != null) ? range.Cells[i, Positions[13]].Value2.ToString() : "";
                    declared.declaredData["AnualPropertyIVAOp2"] = (range.Cells[i, Positions[15]].Value2 != null) ? range.Cells[i, Positions[15]].Value2.ToString() : "";
                    declared.declaredData["AnualPropertyIVAOp3"] = (range.Cells[i, Positions[17]].Value2 != null) ? range.Cells[i, Positions[17]].Value2.ToString() : "";
                    declared.declaredData["AnualPropertyIVAOp4"] = (range.Cells[i, Positions[19]].Value2 != null) ? range.Cells[i, Positions[19]].Value2.ToString() : "";

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
                Marshal.ReleaseComObject(workbook);

                //quit and release
                excelApp.Quit();
                Marshal.ReleaseComObject(excelApp);
            }
        }

        // Puts the number in the required 347 Model Format
        // Imported from Old Lector Excel
        public string FormatNumber(string number, int maxlength, bool shouldBeFloat, bool isUnsigned)
        {
            string entera = "0", dec = "0";
            StringBuilder sb = new StringBuilder();
            bool isNegative = false;

            //Si el numero es negativo
            if (number.Contains("-"))
            {
                isNegative = true;
                number = number.Split('-')[1];
            }

            //Si es decimal
            if (number.Contains(","))
            {
                entera = number.Split(',')[0];

                dec = number.Split(',')[1];
            }
            else if (number.Contains("."))
            {
                entera = number.Split('.')[0];

                dec = number.Split('.')[1];
            }//Si es entero
            else
            {
                entera = number;
            }

            //Agregar simbolo segun el numero
            if (isNegative)
            {
                sb.Append("N");
            }
            else if ((shouldBeFloat) && (!isUnsigned))
            {
                sb.Append(" ");
            }

            if (shouldBeFloat)
            {
                if (!isUnsigned)
                {
                    maxlength -= 3;
                }
                else
                {
                    maxlength -= 2;
                }
                sb.Append(entera.PadLeft(maxlength,'0'));
                
                sb.Append(dec.PadRight(2,'0'));
                
            }
            else
            {
                sb.Append(entera.PadLeft(maxlength,'0'));
            }

            return sb.ToString();
        }

        // Tries to convert accentuated chars into their non-accentuated variants
        static string deAccent(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
