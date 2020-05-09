using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lector_Excel
{
    public class ImportManager
    {
        private string importFilePath;

        //Constructor
        public ImportManager(string path)
        {
            this.importFilePath = path;
        }


        //Imports all data fields from a BOE formatted text file
        public bool ImportFromText(out List<string> Type1Fields, out List<Declared> declaredList)
        {
            StreamReader file = new StreamReader(importFilePath, Encoding.GetEncoding("ISO-8859-1"));
            List<string> returnType1 = new List<string>();
            List<Declared> returnList = new List<Declared>();

            try
            {
                string line;
                int counter = 0;

                //For each line
                while ((line = file.ReadLine()) != null)
                {
                    Debug.WriteLine("Line number " + counter + " is " + line.Length + " chars long!!");
                    //Every line should be 500 chars long
                    if (line.Length != 500)
                    {
                        throw new BadFileFormattingException(counter);
                    }

                    //First line is registry type 1
                    if (counter == 0)
                    {
                        //Verify we are in registry type 1
                        if (!line.Substring(0, 4).Equals("1347"))
                        {
                            throw new BadFileFormattingException(counter);
                        }
                        returnType1.Add(line.Substring(4, 4));  //Exercise
                        returnType1.Add(FormatString(line.Substring(8, 9)));  //Declarant NIF
                        returnType1.Add(EncodeToUTF8(FormatString(line.Substring(17, 40))));  //Declarant Name
                        returnType1.Add(FormatString(line.Substring(57, 1))); //Support Type
                        returnType1.Add(FormatString(line.Substring(58, 9)));  //Related Phone
                        returnType1.Add(EncodeToUTF8(FormatString(line.Substring(67, 40))));  //Related Name
                        returnType1.Add(FormatString(line.Substring(107, 13)));    //Declaration ID
                        returnType1.Add(FormatString(line.Substring(120, 1)));    //Complementary Dec
                        returnType1.Add(FormatString(line.Substring(121, 1)));    //Sustitutive Dec
                        returnType1.Add(FormatString(line.Substring(122, 13)));    //Prev. Declaration ID
                        returnType1.Add(FormatString(line.Substring(135, 9)));   //Total Entities
                        returnType1.Add(FormatNumber(line.Substring(144, 16), false));   //Total Money
                        returnType1.Add(FormatString(line.Substring(160, 9)));   //Total Properties
                        returnType1.Add(FormatNumber(line.Substring(169, 16), false));   //Total Money Rental
                        returnType1.Add(FormatString(line.Substring(390, 9)));   //Legal Rep. NIF
                    }
                    else
                    {
                        //Verify we are in registry type 2
                        if (!line.Substring(0, 4).Equals("2347"))
                        {
                            throw new BadFileFormattingException(counter);
                        }
                        Declared d = new Declared();

                        d.declaredData["DeclaredNIF"] = FormatString(line.Substring(17, 9));
                        d.declaredData["LegalRepNIF"] = FormatString(line.Substring(26, 9));
                        d.declaredData["CommunityOpNIF"] = FormatString(line.Substring(263, 17));
                        d.declaredData["DeclaredName"] = EncodeToUTF8(FormatString(line.Substring(35, 40)));
                        d.declaredData["ProvinceCode"] = line.Substring(76, 2);
                        d.declaredData["CountryCode"] = line.Substring(78, 2);
                        d.declaredData["OpKey"] = FormatString(line.Substring(81, 1));
                        d.declaredData["OpInsurance"] = line.Substring(98, 1);
                        d.declaredData["LocalBusinessLease"] = line.Substring(99, 1);
                        d.declaredData["OpIVA"] = line.Substring(280, 1);
                        d.declaredData["OpPassive"] = line.Substring(281, 1);
                        d.declaredData["OpCustoms"] = line.Substring(282, 1);
                        d.declaredData["TotalMoney"] = FormatNumber(line.Substring(100, 15), true);
                        d.declaredData["AnualMoney"] = FormatNumber(line.Substring(82, 16), false);
                        d.declaredData["AnualPropertyMoney"] = FormatNumber(line.Substring(115, 16), false);
                        d.declaredData["AnualOpIVA"] = FormatNumber(line.Substring(283, 16), false);
                        d.declaredData["Exercise"] = FormatString(line.Substring(131, 4));
                        d.declaredData["TrimestralOp1"] = FormatNumber(line.Substring(135, 16), false);
                        d.declaredData["TrimestralOp2"] = FormatNumber(line.Substring(167, 16), false);
                        d.declaredData["TrimestralOp3"] = FormatNumber(line.Substring(199, 16), false);
                        d.declaredData["TrimestralOp4"] = FormatNumber(line.Substring(231, 16), false);
                        d.declaredData["AnualPropertyIVAOp1"] = FormatNumber(line.Substring(151, 16), false);
                        d.declaredData["AnualPropertyIVAOp2"] = FormatNumber(line.Substring(183, 16), false);
                        d.declaredData["AnualPropertyIVAOp3"] = FormatNumber(line.Substring(215, 16), false);
                        d.declaredData["AnualPropertyIVAOp4"] = FormatNumber(line.Substring(247, 16), false);

                        returnList.Add(d);
                    }

                    counter++;
                }

                Type1Fields = returnType1;
                declaredList = returnList;
                return true;
            }
            catch (Exception e)
            {
                MessageBoxResult msg = MessageBox.Show(e.Message, "Error al importar", MessageBoxButton.OK, MessageBoxImage.Error);
                Type1Fields = null;
                declaredList = null;
                return false;
            }
            finally
            {
                file.Close();
            }
        }

        //Formats number from BOE style to standard style
        private string FormatNumber(string number, bool isUnsigned)
        {
            string parteEntera, parteDecimal;

            if (isUnsigned)
            {
                parteEntera = number.Substring(0, 13);
                parteEntera = parteEntera.TrimStart('0');
                if (parteEntera.Equals(""))
                    parteEntera = "0";
                parteDecimal = number.Substring(12, 2);
            }
            else
            {
                parteEntera = number.Substring(1, 13);
                parteEntera = parteEntera.TrimStart('0');
                if (parteEntera.Equals(""))
                    parteEntera = "0";
                if (number[0].Equals('N'))
                    parteEntera = "-" + parteEntera;
                parteDecimal = number.Substring(13, 2);
            }

            return parteEntera + "," + parteDecimal;
        }

        //Formats string from BOE style to standard
        private string FormatString(string s)
        {
            if (string.IsNullOrWhiteSpace(s) && s != null)
            {
                return "";
            }

            return s;
        }

        // Encodes string from Latin-1 to default UTF8
        private string EncodeToUTF8(string str)
        {
            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            Encoding utf8 = Encoding.UTF8;
            byte[] isoBytes = iso.GetBytes(str);
            byte[] utfBytes = Encoding.Convert(iso, Encoding.Default, isoBytes);
            
            string msg = Encoding.Default.GetString(utfBytes);
            return msg;
        }
    }

    [Serializable]
    class BadFileFormattingException : Exception
    {
        public BadFileFormattingException()
        {

        }

        public BadFileFormattingException(int lineNumber)
            : base(string.Format("Archivo mal formado. Error al leer la línea: {0}", lineNumber))
        {

        }

    }
}