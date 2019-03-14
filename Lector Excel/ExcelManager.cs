using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        // Exports the Type 1 data
        public void ExportType1Data(List<string> Type1Data, string exportingPath = "")
        {
            StringBuilder sb = new StringBuilder();
            Type1Data[1] = deAccent(Type1Data[1]);
            sb.Append("1347");
            //sb.AppendFormat("%04d", Type1Data[0]);
            sb.Append(Type1Data[0].PadLeft(4,'0'));
            sb.Append(Type1Data[2]);
            sb.Append(Type1Data[1].PadRight(40));
            //sb.AppendFormat("%-40s", Type1Data[1]);
            sb.Append("T000000000");

            for (int i = 0; i < 40; i++)
            {
                sb.Append(" ");
            }

            sb.Append("3470000000000  0000000000000");
            sb.AppendFormat("%09s", Type1Data[3]);

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
                sb.AppendFormat("%013s", entera);
                sb.AppendFormat("%-2s", dec).Replace(' ', '0');
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
                sb.AppendFormat("%013s", entera);
                sb.AppendFormat("%-2s", dec).Replace(' ', '0');
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
                sb.AppendFormat("%013s", Type1Data[4]);
                sb.Append("00");
            }

            sb.Append("000000000 000000000000000");

            for (int i = 0; i < 315; i++)
            {
                sb.Append(" ");
            }
            File.WriteAllText(Path.GetDirectoryName(this.path) + "\\result.txt", sb.ToString());
        }

        // Opens a text file and starts exporting the data
        public void ExportData(List<string> Type1Data,string exportingPath = "")
        {
            StringBuilder stringBuilder = new StringBuilder();
            int rows, columns;
            Debug.WriteLine("STARTING EXPORT TO: "+ Path.GetDirectoryName(this.path) + "\\result.txt");
            ExportType1Data(Type1Data);

            worksheet = workbook.Sheets[1];
            range = worksheet.UsedRange;
            rows = range.Rows.Count;
            columns = range.Columns.Count;

            stringBuilder.Append("2347").Append(Type1Data[0]).Append(Type1Data[2]);

            for (int i = 2; i < rows; i++)
            {
                for (int j = 1; j > columns; j++)
                {
                    Debug.Write("Exporting cell " + i + ", " + j + ": " + range.Cells[i, j].Value);
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
                    File.WriteAllText(Path.GetDirectoryName(this.path)+"\\result.txt",stringBuilder.ToString());
                }
            }
        }

        // Puts the number in the required 347 Model Format
        // Type1Data[4]d from Old Lector Excel
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
