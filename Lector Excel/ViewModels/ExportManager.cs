using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lector_Excel
{
    /// <summary>
    /// Clase encargada de exportar los datos a ficheros BOE.
    /// </summary>
    public class ExportManager
    {
        ///<value>El fichero de destino de la exportación.</value>
        private string exportPath;
        //private readonly int[] longitudes = {9, 9, 40, 1, 2, 2, 1, 1, 16, 1, 1, 15, 16, 4, 16, 16, 16, 16, 16, 16, 16, 16, 17, 1, 1, 1, 16, 201 };
        ///<value>Los datos del registro de tipo 1.</value>
        private List<string> Type1Data;

        /// <summary>
        /// Inicializa una nueva instancia de <c>ExportManager</c>.
        /// </summary>
        /// <param name="exportPath">El directorio del fichero donde exportar.</param>
        /// <param name="type1">Los datos del registro de tipo 1.</param>
        public ExportManager(string exportPath, List<string> type1)
        {
            this.exportPath = exportPath;
            this.Type1Data = type1;
        }

        // Exports the Type 1 data
        /// <summary>
        /// Exporta la información del registro de tipo 1.
        /// </summary>
        /// <param name="exportingPath"> El directorio del fichero.</param>
        public void ExportType1Data(string exportingPath = "")
        {
            StringBuilder sb = new StringBuilder();
            Type1Data[2] = deAccent(Type1Data[2]);  // Delete special chars of Name
            Type1Data[5] = deAccent(Type1Data[5]);  // Delete special chars of Relation Name
            sb.Append(EncodeToLatin("1347"));

            sb.Append(EncodeToLatin(Type1Data[0].PadLeft(4, '0'))); // Append Ejercicio, padding with zeroes
            sb.Append(EncodeToLatin(Type1Data[1]));    // Append NIF
            sb.Append(EncodeToLatin(Type1Data[2].PadRight(40)));   // Append Name, requires padding

            sb.Append(EncodeToLatin(Type1Data[3].PadLeft(1)));    // Append Support Type, replacing if empty

            sb.Append(EncodeToLatin(Type1Data[4].PadRight(9, '0')));    // Append Phone, padding with zeroes if empty
            sb.Append(EncodeToLatin(Type1Data[5].PadRight(40)));   // Append Relation Name, requires padding

            sb.Append(EncodeToLatin(Type1Data[6].PadRight(13, '0')));    // Append Declaration ID
            sb.Append(EncodeToLatin(Type1Data[7].PadLeft(1)));    // Append Complementary Dec, replacing if empty
            sb.Append(EncodeToLatin(Type1Data[8].PadLeft(1)));    // Append Sustitutive Dec, replacing if empty
            sb.Append(EncodeToLatin(Type1Data[9].PadRight(13, '0')));    // Append Previous Declaration ID, padding with zeroes

            sb.Append(EncodeToLatin(Type1Data[10].PadLeft(9, '0')));   // Append Total number of entities, padding with zeroes

            sb.Append(EncodeToLatin(FormatNumber(Type1Data[11], 16, true, false)));   // Append Total Money, with floating point and sign formatting

            sb.Append(EncodeToLatin(Type1Data[12].PadLeft(9, '0')));   // Append Properties amount, padding with zeroes if empty
            sb.Append(EncodeToLatin(FormatNumber(Type1Data[13], 16, true, false)));    // Append Total Money Rental, with floating point and sign formatting

            // Append 205 blank characters
            for (int i = 0; i < 205; i++)
            {
                sb.Append(" ");
            }
            sb.Append(EncodeToLatin(Type1Data[14].PadLeft(9)));   // Append Legal NIF, padding with spaces if empty

            // Append 101 blank characters
            for (int i = 0; i < 101; i++)
            {
                sb.Append(" ");
            }

            sb.Append(Environment.NewLine); // Append a new line
            if (exportingPath.Equals(""))
                File.WriteAllText(Path.GetDirectoryName(this.exportPath) + "\\result.txt", sb.ToString(),Encoding.GetEncoding("ISO-8859-1"));
            else
                File.WriteAllText(exportingPath, sb.ToString(), Encoding.GetEncoding("ISO-8859-1"));
        }


        //Export Type 2
        /// <summary>
        /// Exporta los datos de tipo 2.
        /// </summary>
        /// <param name="declareds"> La lista de declarados que se va a exportar.</param>
        /// <param name="bw"> El <c>BackgroundWorker</c> encargado de reportar el progreso.</param>
        /// <returns></returns>
        public bool ExportFromMain(List<Declared> declareds, BackgroundWorker bw)
        {
            try
            {
                if (declareds.Count == 0 || bw == null)
                    return false;

                StringBuilder stringBuilder = new StringBuilder();
                ExportType1Data(this.exportPath);

                int i = 0;

                foreach (Declared dec in declareds)
                {
                    Debug.WriteLine("Appending declared " + i);
                    stringBuilder.Append(EncodeToLatin("2347")).Append(EncodeToLatin(Type1Data[0])).Append(EncodeToLatin(Type1Data[1])); // We append Type1Data [0] and [1], as they are the same fields for Type 2

                    stringBuilder.Append(EncodeToLatin(dec.declaredData["DeclaredNIF"].ToUpper().PadRight(9)));
                    stringBuilder.Append(EncodeToLatin(dec.declaredData["LegalRepNIF"].ToUpper().PadRight(9)));
                    stringBuilder.Append(EncodeToLatin(dec.declaredData["DeclaredName"].ToUpper().PadRight(40)));

                    stringBuilder.Append(EncodeToLatin("D"));

                    stringBuilder.Append(EncodeToLatin(FormatNumber(dec.declaredData["ProvinceCode"], 2, false, true)));
                    stringBuilder.Append(EncodeToLatin(dec.declaredData["CountryCode"].ToUpper().PadRight(2)));

                    stringBuilder.Append(" ");

                    stringBuilder.Append(EncodeToLatin(dec.declaredData["OpKey"]));

                    stringBuilder.Append(EncodeToLatin(FormatNumber(dec.declaredData["AnualMoney"], 16, true, false)));
                    stringBuilder.Append(EncodeToLatin(dec.declaredData["OpInsurance"].PadLeft(1)));
                    stringBuilder.Append(EncodeToLatin(dec.declaredData["LocalBusinessLease"].PadLeft(1)));

                    stringBuilder.Append(EncodeToLatin(FormatNumber(dec.declaredData["TotalMoney"], 15, true, true)));
                    stringBuilder.Append(EncodeToLatin(FormatNumber(dec.declaredData["AnualPropertyMoney"], 16, true, false)));

                    stringBuilder.Append(EncodeToLatin(FormatNumber(dec.declaredData["Exercise"], 4, false, true)));

                    stringBuilder.Append(EncodeToLatin(FormatNumber(dec.declaredData["TrimestralOp1"], 16, true, false)));
                    stringBuilder.Append(EncodeToLatin(FormatNumber(dec.declaredData["AnualPropertyIVAOp1"], 16, true, false)));

                    stringBuilder.Append(EncodeToLatin(FormatNumber(dec.declaredData["TrimestralOp2"], 16, true, false)));
                    stringBuilder.Append(EncodeToLatin(FormatNumber(dec.declaredData["AnualPropertyIVAOp2"], 16, true, false)));

                    stringBuilder.Append(EncodeToLatin(FormatNumber(dec.declaredData["TrimestralOp3"], 16, true, false)));
                    stringBuilder.Append(EncodeToLatin(FormatNumber(dec.declaredData["AnualPropertyIVAOp3"], 16, true, false)));

                    stringBuilder.Append(EncodeToLatin(FormatNumber(dec.declaredData["TrimestralOp4"], 16, true, false)));
                    stringBuilder.Append(EncodeToLatin(FormatNumber(dec.declaredData["AnualPropertyIVAOp4"], 16, true, false)));

                    stringBuilder.Append(EncodeToLatin(dec.declaredData["CommunityOpNIF"].ToUpper().PadRight(17)));

                    stringBuilder.Append(EncodeToLatin(dec.declaredData["OpIVA"].PadLeft(1)));
                    stringBuilder.Append(EncodeToLatin(dec.declaredData["OpPassive"].PadLeft(1)));
                    stringBuilder.Append(EncodeToLatin(dec.declaredData["OpCustoms"].PadLeft(1)));

                    stringBuilder.Append(EncodeToLatin(FormatNumber(dec.declaredData["AnualOpIVA"], 16, true, false)));

                    //Append 201 blank characters
                    for (int k = 0; k < 201; k++)
                    {
                        stringBuilder.Append(" ");
                    }
                    stringBuilder.Append(Environment.NewLine); // Append a new line

                    //Report progress to Background Worker
                    i++;
                    float progress = (float)i / declareds.Count * 100;
                    bw.ReportProgress((int)progress);
                }
                File.AppendAllText(this.exportPath, stringBuilder.ToString(), Encoding.GetEncoding("ISO-8859-1"));
                return true;
            }
            catch (Exception e)
            {
                MessageBoxResult msg = MessageBox.Show("Ha ocurrido un error. La exportación se interrumpirá.\nCódigo del error: " + e, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
	

        // Puts the number in the required 347 Model Format
        // Imported from Old Lector Excel
        /// <summary>
        /// Pone el número recibido en el formato requerido por el campo del modelo 347.
        /// </summary>
        /// <param name="number">El número a formatear.</param>
        /// <param name="maxlength">La longitud máxima del campo.</param>
        /// <param name="shouldBeFloat">True si el número debería ser formateado como número de coma flotante.</param>
        /// <param name="isUnsigned">True si el número es sin signo.</param>
        /// <returns></returns>
        private string FormatNumber(string number, int maxlength, bool shouldBeFloat, bool isUnsigned)
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
        /// <summary>
        /// Convierte los caracteres acentuados en sus variantes sin acentuar.
        /// </summary>
        /// <param name="text">El texto del que quitar los acentos.</param>
        /// <returns>Una cadena con el texto sin acentos.</returns>
        private string deAccent(string text)
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

        // Encodes string to Latin-1, as required from model
        /// <summary>
        /// Codifica el texto en Latin-1.
        /// </summary>
        /// <param name="str">El texto a codificar.</param>
        /// <returns>Una cadena con el texto codificado en Latin-1.</returns>
        private string EncodeToLatin(string str)
        {
            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            //Encoding utf8 = Encoding.UTF8;
            byte[] utfBytes = Encoding.Default.GetBytes(str);
            byte[] isoBytes = Encoding.Convert(Encoding.Default, iso, utfBytes);
            string msg = iso.GetString(isoBytes);
            return msg;
        }
    }
}
