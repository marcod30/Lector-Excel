using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Excel = Microsoft.Office.Interop.Excel;
namespace Lector_Excel
{
    public class ExcelManager
    {
        string path;
        Excel.Application excelApp;
        Excel.Workbook workbook;
        Excel._Worksheet worksheet;
        Excel.Range range;
        private readonly int[] longitudes2 = {-1, 9, 9, 40, 1, 2, 2, 1, 1, 16, 1, 1, 15, 16, 4, 16, 16, 16, 16, 16, 16, 16, 16, 17, 1, 1, 1, 16, 201 };
        
        public ExcelManager(string path)
        {
            excelApp = new Excel.Application();
            workbook = excelApp.Workbooks.Open(path);
            this.path = path;
        }

        public void ExportData(string Ejercicio, string NIFDeclarante)
        {
            StringBuilder stringBuilder = new StringBuilder();
            worksheet = workbook.Sheets[1];
            range = worksheet.UsedRange;
            int rows, columns;
            rows = range.Rows.Count;
            columns = range.Columns.Count;
            stringBuilder.Append("2347").Append(Ejercicio).Append(NIFDeclarante);
            for (int i = 2; i < rows; i++)
            {
                for (int j = 1; j > columns; j++)
                {
                    // NUMERIC CELL
                    if(double.TryParse(range.Cells[i, j].Value2, out double d))
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
                            stringBuilder.Append(deAccent(string.Format("%-" + longitudes2[j] + "s", range.Cells[i, j].Value.ToString()).ToUpper()));
                        }
                        else
                        {
                            stringBuilder.Append(deAccent(string.Format("%s", range.Cells[i, j].Value.ToString()).ToUpper()));
                        }
                    }
                    else if (Regex.Match(range.Cells[i, j].Value.ToString(), "^ -?\\d +\\.?\\d *$").Success)
                    {
                        stringBuilder.Append(FormatNumber(range.Cells[i, j].Value.ToString(), longitudes2[j], true, false));
                    }
                    else
                    {
                        stringBuilder.Append(deAccent(string.Format("%-" + longitudes2[j] + "s", range.Cells[i, j].Value.ToString()).ToUpper()));
                    }

                    // BLANK CELL
                    if(range.Cells[i, j].Value.ToString().Equals(""))
                        for (int k = 0; k < longitudes2[j]; k++)
                        {
                            stringBuilder.Append(" ");
                        }
                    File.WriteAllText(Path.GetDirectoryName(this.path)+"result.txt",stringBuilder.ToString());
                }
            }
        }

        // Imported from Old Lector Excel
        public string FormatNumber(string number, int maxlength, bool shouldBeFloat, bool isUnsigned)
        {
            string entera = "0", dec = "0";
            StringBuilder sb = new StringBuilder();
            bool isNegative = false;
            if (number.Contains("-"))
            {
                isNegative = true;
                number = number.Split('-')[1];
            }
            if (number.Contains(","))
            {
                entera = number.Split(',')[0];

                dec = number.Split(',')[1];
            }
            else if (number.Contains("."))
            {
                entera = number.Split('.')[0];

                dec = number.Split('.')[1];
            }
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
                sb.AppendFormat("%0" + maxlength + "d", int.Parse(entera));
                string temp = "";
                temp = string.Format("%-2s", dec);
                temp.Replace(' ', '0');
                sb.AppendFormat(temp);
            }
            else
            {
                sb.AppendFormat("%0" + maxlength + "d", int.Parse(entera));
            }
            return sb.ToString();
        }
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
