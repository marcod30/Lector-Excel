using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lector_Excel
{
    public class ExportManager
    {
        private string exportPath;
        //private readonly int[] longitudes = {9, 9, 40, 1, 2, 2, 1, 1, 16, 1, 1, 15, 16, 4, 16, 16, 16, 16, 16, 16, 16, 16, 17, 1, 1, 1, 16, 201 };
        private List<string> Type1Data;

        //Contructor
        public ExportManager(string exportPath, List<string> type1)
        {
            this.exportPath = exportPath;
            this.Type1Data = type1;
        }

        // Exports the Type 1 data
        public void ExportType1Data(string exportingPath = "")
        {
            StringBuilder sb = new StringBuilder();
            Type1Data[2] = deAccent(Type1Data[2]);  // Delete special chars of Name
            Type1Data[5] = deAccent(Type1Data[5]);  // Delete special chars of Relation Name
            sb.Append("1347");

            sb.Append(Type1Data[0].PadLeft(4, '0')); // Append Ejercicio, padding with zeroes
            sb.Append(Type1Data[1]);    // Append NIF
            sb.Append(Type1Data[2].PadRight(40));   // Append Name, requires padding

            sb.Append(Type1Data[3].PadLeft(1));    // Append Support Type, replacing if empty

            sb.Append(Type1Data[4].PadRight(9, '0'));    // Append Phone, padding with zeroes if empty
            sb.Append(Type1Data[5].PadRight(40));   // Append Relation Name, requires padding

            sb.Append(Type1Data[6].PadRight(13, '0'));    // Append Declaration ID
            sb.Append(Type1Data[7].PadLeft(1));    // Append Complementary Dec, replacing if empty
            sb.Append(Type1Data[8].PadLeft(1));    // Append Sustitutive Dec, replacing if empty
            sb.Append(Type1Data[9].PadRight(13, '0'));    // Append Previous Declaration ID, padding with zeroes

            sb.Append(Type1Data[10].PadLeft(9, '0'));   // Append Total number of entities, padding with zeroes

            sb.Append(FormatNumber(Type1Data[11], 16, true, false));   // Append Total Money, with floating point and sign formatting

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
            if (exportingPath.Equals(""))
                File.WriteAllText(Path.GetDirectoryName(this.exportPath) + "\\result.txt", sb.ToString());
            else
                File.WriteAllText(exportingPath, sb.ToString());
        }


        //Export Type 2
        public bool ExportFromMain(List<Declared> declareds, BackgroundWorker bw)
        {
            if (declareds.Count == 0 || bw == null)
                return false;

            StringBuilder stringBuilder = new StringBuilder();
            ExportType1Data(this.exportPath);
            
            stringBuilder.Append("2347").Append(Type1Data[0]).Append(Type1Data[1]); // We append Type1Data [0] and [1], as they are the same fields for Type 2
            int i = 0;

            foreach (Declared dec in declareds)
            {
                stringBuilder.Append(dec.declaredData["DeclaredNIF"].ToUpper().PadRight(9));
                stringBuilder.Append(dec.declaredData["LegalRepNIF"].ToUpper().PadRight(9));
                stringBuilder.Append(dec.declaredData["DeclaredName"].ToUpper().PadRight(40));

                stringBuilder.Append("D");

                stringBuilder.Append(FormatNumber(dec.declaredData["ProvinceCode"], 2, false, true));
                stringBuilder.Append(FormatNumber(dec.declaredData["CountryCode"], 2, false, true));

                stringBuilder.Append(" ");

                stringBuilder.Append(dec.declaredData["OpKey"]);

                stringBuilder.Append(FormatNumber(dec.declaredData["AnualMoney"], 16, true, false));
                stringBuilder.Append(dec.declaredData["OpInsurance"]);
                stringBuilder.Append(dec.declaredData["LocalBusinessLease"]);

                stringBuilder.Append(FormatNumber(dec.declaredData["TotalMoney"], 15, true, true));
                stringBuilder.Append(FormatNumber(dec.declaredData["AnualPropertyMoney"], 16, true, false));

                stringBuilder.Append(FormatNumber(dec.declaredData["Exercise"], 4, false, true));

                stringBuilder.Append(FormatNumber(dec.declaredData["TrimestralOp1"], 16, true, false));
                stringBuilder.Append(FormatNumber(dec.declaredData["AnualPropertyIVAOp1"], 16, true, false));

                stringBuilder.Append(FormatNumber(dec.declaredData["TrimestralOp2"], 16, true, false));
                stringBuilder.Append(FormatNumber(dec.declaredData["AnualPropertyIVAOp2"], 16, true, false));

                stringBuilder.Append(FormatNumber(dec.declaredData["TrimestralOp3"], 16, true, false));
                stringBuilder.Append(FormatNumber(dec.declaredData["AnualPropertyIVAOp3"], 16, true, false));

                stringBuilder.Append(FormatNumber(dec.declaredData["TrimestralOp4"], 16, true, false));
                stringBuilder.Append(FormatNumber(dec.declaredData["AnualPropertyIVAOp4"], 16, true, false));

                stringBuilder.Append(dec.declaredData["CommunityOpNIF"].ToUpper().PadRight(17));

                stringBuilder.Append(dec.declaredData["OpIVA"]);
                stringBuilder.Append(dec.declaredData["OpPassive"]);
                stringBuilder.Append(dec.declaredData["OpCustoms"]);

                stringBuilder.Append(FormatNumber(dec.declaredData["AnualOpIVA"], 16, true, false));

                //Append 201 blank characters
                for (int k = 0; k < 201; k++)
                {
                    stringBuilder.Append(" ");
                }
                stringBuilder.Append(Environment.NewLine); // Append a new line

                //Report progress to Background Worker
                i++;
                int progress = i / declareds.Count * 100;
                bw.ReportProgress(progress);
            }

            return true;
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
                sb.Append(entera.PadLeft(maxlength, '0'));

                sb.Append(dec.PadRight(2, '0'));

            }
            else
            {
                sb.Append(entera.PadLeft(maxlength, '0'));
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
