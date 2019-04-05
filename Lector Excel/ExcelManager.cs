using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

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
        private readonly int[] longitudes2 = {-1, 9, 9, 40, 1, 2, 2, 1, 1, 16, 1, 1, 15, 16, 4, 16, 16, 16, 16, 16, 16, 16, 16, 17, 1, 1, 1, 16, 201 };
        const int MAX_ALLOWED_COLUMNS = 28; //Model 347 has 28 data fields only, so if further data is found, it will be ignored

        public ExcelManager(string path)
        {
            
            this.path = path;
        }

        // Exports the Type 1 data
        public void ExportType1Data(List<string> Type1Data, string exportingPath = "")
        {
            StringBuilder sb = new StringBuilder();
            Type1Data[2] = deAccent(Type1Data[2]);  // Delete special chars of Name
            Type1Data[5] = deAccent(Type1Data[5]);  // Delete special chars of Relation Name
            sb.Append("1347");
            //sb.AppendFormat("%04d", Type1Data[0]);
            sb.Append(Type1Data[0].PadLeft(4, '0')); // Append Ejercicio, padding with zeroes
            sb.Append(Type1Data[1]);    // Append NIF
            sb.Append(Type1Data[2].PadRight(40));   // Append Name, requires padding
            //sb.AppendFormat("%-40s", Type1Data[1]);
            sb.Append(Type1Data[3].PadLeft(1));    // Append Support Type, replacing if empty

            sb.Append(Type1Data[4].PadRight(9, '0'));    // Append Phone, padding with zeroes if empty
            sb.Append(Type1Data[5].PadRight(40));   // Append Relation Name, requires padding

            sb.Append(Type1Data[6].PadRight(13, '0'));    // Append Declaration ID
            sb.Append(Type1Data[7].PadLeft(1));    // Append Complementary Dec, replacing if empty
            sb.Append(Type1Data[8].PadLeft(1));    // Append Sustitutive Dec, replacing if empty
            sb.Append(Type1Data[9].PadRight(13, '0'));    // Append Previous Declaration ID, padding with zeroes

            sb.Append(Type1Data[10].PadLeft(9, '0'));   // Append Total number of entities, padding with zeroes
            //sb.AppendFormat("%09s", Type1Data[3]);
            sb.Append(FormatNumber(Type1Data[11],16,true,false));   // Append Total Money, with floating point and sign formatting
            /*
            if (Type1Data[4].Contains(","))
            {
                bool isNegative = false;
                if (Type1Data[4].Contains("-"))
                {
                    isNegative = true;
                    Type1Data[4] = Type1Data[4].Split('-')[1];
                }
                string entera = Type1Data[4].Split(',')[0];
                string dec = Type1Data[4].Split(',')[1];
                if (isNegative)
                {
                    sb.Append("N");
                }
                else
                {
                    sb.Append(" ");
                }
                sb.Append(entera.PadLeft(13,'0'));
                //sb.AppendFormat("%013s", entera);
                sb.Append(dec.PadRight(2).Replace(' ', '0'));
                //sb.AppendFormat("%-2s", dec).Replace(' ', '0');
            }
            else if (Type1Data[4].Contains("."))
            {
                bool isNegative = false;
                if (Type1Data[4].Contains("-"))
                {
                    isNegative = true;
                    Type1Data[4] = Type1Data[4].Split('-')[1];
                }
                string entera = Type1Data[4].Split('.')[0];
                string dec = Type1Data[4].Split('.')[1];
                if (isNegative)
                {
                    sb.Append("N");
                }
                else
                {
                    sb.Append(" ");
                }
                sb.Append(entera.PadLeft(13, '0'));
                //sb.AppendFormat("%013s", entera);
                sb.Append(dec.PadRight(2).Replace(' ', '0'));
                //sb.AppendFormat("%-2s", dec).Replace(' ', '0');
            }
            else
            {
                bool isNegative = false;
                if (Type1Data[4].Contains("-"))
                {
                    isNegative = true;
                    Type1Data[4] = Type1Data[4].Split('-')[1];
                }
                if (isNegative)
                {
                    sb.Append("N");
                }
                else
                {
                    sb.Append(" ");
                }
                sb.Append(Type1Data[4].PadLeft(13,'0'));
                //sb.AppendFormat("%013s", Type1Data[4]);
                sb.Append("00");
            }
            */
            sb.Append(Type1Data[12].PadLeft(9, '0'));   // Append Properties amount, padding with zeroes if empty
            sb.Append(FormatNumber(Type1Data[13], 16, true, false));    // Append Total Money Rental, with floating point and sign formatting

            // Append 205 blank characters
            for (int i = 0; i < 205; i++)
            {
                sb.Append(" ");
            }
            sb.Append(Type1Data[14].PadLeft(9));   // Append Legal NIF, padding with spaces if empty

            // Append 101 blank characters
            for (int i = 0; i < 101; i++)
            {
                sb.Append(" ");
            }

            sb.Append(Environment.NewLine); // Append a new line
            if(exportingPath.Equals(""))
                File.WriteAllText(Path.GetDirectoryName(this.path) + "\\result.txt", sb.ToString());
            else
                File.WriteAllText(exportingPath, sb.ToString());
        }

        // Opens a text file and starts exporting the data
        public void ExportData(List<string> Type1Data, BackgroundWorker bw, string exportingPath = "")
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                excelApp = new Excel.Application();
                workbooks = excelApp.Workbooks;
                workbook = workbooks.Open(path);

                int rows, columns;
                Debug.WriteLine("STARTING EXPORT TO: " + Path.GetDirectoryName(this.path) + "\\result.txt");

                ExportType1Data(Type1Data,exportingPath);

                worksheet = workbook.Sheets[1];
                range = worksheet.UsedRange;
                rows = range.Rows.Count;
                columns = range.Columns.Count;

                Debug.WriteLine("El excel tiene " + rows + " filas y " + columns + " columnas");
                

                for (int i = 2; i <= rows; i++)
                {
                    if (range.Cells[i, 1].Value2 != null)
                        stringBuilder.Append("2347").Append(Type1Data[0]).Append(Type1Data[1]);
                    for (int j = 1; j <= MAX_ALLOWED_COLUMNS; j++)
                    {

                        //Debug.Write("Exporting cell " + i + ", " + j + ": " + range.Cells[i, j].Value2.ToString());
                        /*
                        if (range.Cells[i, j].Value2 != null)
                            MessageBox.Show("Exporting cell " + i + ", " + j + ": " + range.Cells[i, j].Value2.ToString());
                            */

                        // BLANK CELL
                        if (range.Cells[i, j] != null)
                            if (range.Cells[i, j].Value2 == null)
                                for (int k = 0; k < longitudes2[j]; k++)
                                {
                                    stringBuilder.Append(" ");
                                }
                            else if(range.Cells[i,1].Value2 != null )
                            {
                                // NUMERIC CELL
                                if (double.TryParse(range.Cells[i, j].Value2.ToString(), out double d))
                                    if (j == 5 || j == 6 || j == 14)
                                    {
                                        stringBuilder.Append(FormatNumber(range.Cells[i, j].Value2.ToString(), longitudes2[j], false, true));
                                    }
                                    else if (j == 12)
                                    {
                                        stringBuilder.Append(FormatNumber(range.Cells[i, j].Value2.ToString(), longitudes2[j], true, true));
                                    }
                                    else
                                    {
                                        stringBuilder.Append(FormatNumber(range.Cells[i, j].Value2.ToString(), longitudes2[j], true, false));
                                    }
                                else
                                // STRING CELL
                                if (!range.Cells[i, j].Value.ToString().Contains("-"))
                                {
                                    if (longitudes2[j] != 1)
                                    {
                                        //stringBuilder.Append(deAccent(string.Format("%-" + longitudes2[j] + "s", range.Cells[i, j].Value.ToString()).ToUpper()));
                                        stringBuilder.Append(deAccent(range.Cells[i, j].Value2.ToString().PadRight(longitudes2[j]).ToUpper()));
                                    }
                                    else
                                    {
                                        //stringBuilder.Append(deAccent(string.Format("%s", range.Cells[i, j].Value2.ToString()).ToUpper()));
                                        stringBuilder.Append(deAccent(range.Cells[i, j].Value2.ToString().ToUpper()));
                                    }
                                }
                                /*
                                else if (Regex.Match(range.Cells[i, j].Value2.ToString(), "^ -?\\d +\\.?\\d *$").Success)
                                {
                                    stringBuilder.Append(FormatNumber(range.Cells[i, j].Value2.ToString(), longitudes2[j], true, false));
                                }
                                */
                                else
                                {
                                    //stringBuilder.Append(deAccent(string.Format("%-" + longitudes2[j] + "s", range.Cells[i, j].Value2.ToString()).ToUpper()));
                                    stringBuilder.Append(deAccent(range.Cells[i, j].Value2.ToString().PadRight(longitudes2[j]).ToUpper()));
                                }
                            }
                    }
                    stringBuilder.Append(Environment.NewLine);
                    float progress = (float)i / rows * 100;
                    Debug.WriteLine(progress + "%");
                    bw.ReportProgress((int)progress);
                }
                stringBuilder.Append(Environment.NewLine);
                if(exportingPath.Equals(""))
                    File.AppendAllText(Path.GetDirectoryName(this.path) + "\\result.txt", stringBuilder.ToString());
                else
                    File.AppendAllText(exportingPath, stringBuilder.ToString());

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
                //sb.AppendFormat("%0" + maxlength + "d", int.Parse(entera));
                /*
                string temp = "";
                temp = string.Format("%-2s", dec);
                temp.Replace(' ', '0');
                */
                sb.Append(dec.PadRight(2,'0'));
                //sb.AppendFormat(temp);
            }
            else
            {
                sb.Append(entera.PadLeft(maxlength,'0'));
                //sb.AppendFormat("%0" + maxlength + "d", int.Parse(entera));
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
