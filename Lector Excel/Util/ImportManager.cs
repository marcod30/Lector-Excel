using Reader_347;
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
    /// <summary>
    /// Clase encargada de importar los datos de un archivo BOE.
    /// </summary>
    public class ImportManager
    {
        private string importFilePath;

        /// <summary>
        /// Inicializa una nueva instancia de <c>ImportManager</c>.
        /// </summary>
        /// <param name="path"> El archivo del que importar los datos.</param>
        public ImportManager(string path)
        {
            this.importFilePath = path;
        }


        //Imports all data fields from a BOE formatted text file
        /// <summary>
        /// Importa los datos y los encapsula en estructuras <c>Declared</c>.
        /// </summary>
        /// <param name="Type1Fields">La lista donde guardar los datos de tipo 1.</param>
        /// <param name="declaredList">La lista de declarados donde guardar los datos.</param>
        /// <returns>True si la importación se produjo sin errores.</returns>
        public bool ImportFromText(out Type1Registry Type1Fields, out List<Declared> declaredList)
        {
            StreamReader file = new StreamReader(importFilePath, Encoding.GetEncoding("ISO-8859-1"));
            Type1Registry returnType1 = new Type1Registry();
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
                        returnType1.Ejercicio = (line.Substring(4, 4));  //Exercise
                        returnType1.DeclarantNIF = (FormatString(line.Substring(8, 9)));  //Declarant NIF
                        returnType1.DeclarantName = (EncodeToUTF8(FormatString(line.Substring(17, 40))));  //Declarant Name
                        returnType1.SupportType = (FormatString(line.Substring(57, 1))); //Support Type
                        returnType1.RelationsPhone = (FormatString(line.Substring(58, 9)));  //Related Phone
                        returnType1.RelationsName = (EncodeToUTF8(FormatString(line.Substring(67, 40))));  //Related Name
                        returnType1.DeclarationID = (FormatString(line.Substring(107, 13)));    //Declaration ID
                        returnType1.IsComplementaryDec = (FormatString(line.Substring(120, 1))).ToUpper().Equals("C");    //Complementary Dec
                        returnType1.IsSustitutiveDec = (FormatString(line.Substring(121, 1))).ToUpper().Equals("S");    //Sustitutive Dec
                        returnType1.IsNonSpecialDec = !returnType1.IsComplementaryDec && !returnType1.IsSustitutiveDec;
                        returnType1.PrevDeclarationID = (FormatString(line.Substring(122, 13)));    //Prev. Declaration ID
                        returnType1.TotalEntities = (FormatString(line.Substring(135, 9)));   //Total Entities
                        returnType1.TotalAnualMoney = (FormatNumber(line.Substring(144, 16), false));   //Total Money
                        returnType1.TotalProperties = (FormatString(line.Substring(160, 9)));   //Total Properties
                        returnType1.TotalMoneyRental = (FormatNumber(line.Substring(169, 16), false));   //Total Money Rental
                        returnType1.LegalRepNIF = (FormatString(line.Substring(390, 9)));   //Legal Rep. NIF
                    }
                    else
                    {
                        //Verify we are in registry type 2
                        if (!line.Substring(0, 4).Equals("2347"))
                        {
                            throw new BadFileFormattingException(counter);
                        }

                        if (!line.Substring(75, 1).ToUpper().Equals("I"))
                        {
                            Declared d = new Declared(false);

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
                        else
                        {
                            Declared d = new Declared(true);

                            d.declaredData["RenterNIF"] = FormatString(line.Substring(17, 9));
                            d.declaredData["LegalRepNIF"] = FormatString(line.Substring(26, 9));
                            d.declaredData["RenterName"] = EncodeToUTF8(FormatString(line.Substring(35, 40)));

                            d.declaredData["TotalMoney"] = FormatNumber(line.Substring(98, 16), false);

                            d.declaredData["Situation"] = line.Substring(114, 1);
                            d.declaredData["CatRef"] = FormatString(line.Substring(115, 25));
                            
                            d.declaredData["StreetType"] = EncodeToUTF8(FormatString(line.Substring(140, 5)));
                            d.declaredData["StreetName"] = EncodeToUTF8(FormatString(line.Substring(145, 50)));
                            d.declaredData["TypeNum"] = FormatString(line.Substring(195, 3));
                            d.declaredData["HouseNum"] = FormatString(line.Substring(198, 5));
                            d.declaredData["QualNum"] = FormatString(line.Substring(203, 3));
                            d.declaredData["Block"] = FormatString(line.Substring(206, 3));
                            d.declaredData["Port"] = FormatString(line.Substring(209, 3));
                            d.declaredData["Stair"] = FormatString(line.Substring(212, 3));
                            d.declaredData["Floor"] = FormatString(line.Substring(215, 3));
                            d.declaredData["Door"] = FormatString(line.Substring(218, 3));

                            d.declaredData["Complement"] = EncodeToUTF8(FormatString(line.Substring(221, 40)));
                            d.declaredData["Location"] = EncodeToUTF8(FormatString(line.Substring(261, 30)));
                            d.declaredData["Town"] = EncodeToUTF8(FormatString(line.Substring(291, 30)));
                            d.declaredData["TownCode"] = FormatString(line.Substring(321, 5));
                            d.declaredData["ProvinceCode"] = line.Substring(326, 2);
                            d.declaredData["PostalCode"] = line.Substring(328, 5);
                            

                            returnList.Add(d);
                        }
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
        /// <summary>
        /// Convierte un número de un campo del modelo 347 a su lectura original.
        /// </summary>
        /// <param name="number">El número a formatear.</param>
        /// <param name="isUnsigned">True si el número es sin signo.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Convierte una cadena de un campo del modelo 347 a su lectura original.
        /// </summary>
        /// <remarks>
        /// Este método se utiliza principalmente para las cadenas vacías y de solo espacios.
        /// </remarks>
        /// <param name="s">La cadena a convertir.</param>
        /// <returns>La cadena modificada.</returns>
        private string FormatString(string s)
        {
            if (string.IsNullOrWhiteSpace(s) && s != null)
            {
                return "";
            }

            s.TrimEnd(' ');
            return s;
        }

        // Encodes string from Latin-1 to default UTF8
        /// <summary>
        /// Codifica una cadena desde Latin-1 a UTF8.
        /// </summary>
        /// <param name="str">La cadena a codificar.</param>
        /// <returns>Una cadena codificada en UTF-8.</returns>
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

    /// <summary>
    /// Clase de excepción utilizada por <c>ImportManager</c>.
    /// </summary>
    [Serializable]
    class BadFileFormattingException : Exception
    {
        /// <summary>
        /// Inicializa una nueva instancia de <c>BadFileFormattingException</c>.
        /// </summary>
        public BadFileFormattingException()
        {

        }

        /// <summary>
        /// Inicializa una nueva instancia de <c>BadFileFormattingException</c>, para la línea del fichero que ha fallado.
        /// </summary>
        /// <param name="lineNumber">El número de la línea errónea del fichero.</param>
        public BadFileFormattingException(int lineNumber)
            : base(string.Format("Archivo mal formado. Error al leer la línea: {0}", lineNumber))
        {

        }

    }
}