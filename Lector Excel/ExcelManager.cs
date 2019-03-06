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
        public ExcelManager(String path)
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
    }
}
