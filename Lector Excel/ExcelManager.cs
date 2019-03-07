using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Excel = Microsoft.Office.Interop.Excel;
namespace Lector_Excel
{
    public class ExcelManager
    {
        Excel.Application excelApp;
        Excel.Workbook workbook;
        Excel._Worksheet worksheet;
        Excel.Range range;
        private readonly int[] longitudes2 = { 9, 9, 40, 1, 2, 2, 1, 1, 16, 1, 1, 15, 16, 4, 16, 16, 16, 16, 16, 16, 16, 16, 17, 1, 1, 1, 16, 201 };
        public ExcelManager(string path)
        {
            excelApp = new Excel.Application();
            workbook = excelApp.Workbooks.Open(path);
        }

        public void GetCellValues()
        {
            worksheet = workbook.Sheets[1];
            range = worksheet.UsedRange;
            int rows, columns;
            rows = range.Rows.Count;
            columns = range.Columns.Count;
        }

        public void ExportData()
        {
            //TO DO: Progress Bar
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
    }
}
