using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lector_Excel
{
    /// <summary>
    /// La clase de un declarado del modelo 347.
    /// </summary>
    public class Declared
    {
        /// <value>Marca el declarado como una propiedad física.</value>
        public readonly bool isPropertyDeclared;
        /// <value>Marca el declarado si tiene errores.</value>
        public bool mayContainErrors = false;
        /// <value> El diccionario de datos del declarado.</value>
        public Dictionary<string, string> declaredData;

        /// <summary>
        /// Inicializa una nueva instancia de <c>Declared</c>.
        /// </summary>
        /// <param name="isProperty">Indica si se está declarando un inmueble.</param>
        public Declared(bool isProperty)
        {
            declaredData = new Dictionary<string, string>();
            if (!isProperty)
            {
                declaredData.Add("DeclaredNIF", "");
                declaredData.Add("LegalRepNIF", "");
                declaredData.Add("CommunityOpNIF", "");
                declaredData.Add("DeclaredName", "");
                declaredData.Add("ProvinceCode", "");
                declaredData.Add("CountryCode", "");
                declaredData.Add("OpKey", " ");
                declaredData.Add("OpInsurance", " ");
                declaredData.Add("LocalBusinessLease", " ");
                declaredData.Add("OpIVA", " ");
                declaredData.Add("OpPassive", " ");
                declaredData.Add("OpCustoms", " ");
                declaredData.Add("TotalMoney", "");
                declaredData.Add("AnualMoney", "");
                declaredData.Add("AnualPropertyMoney", "");
                declaredData.Add("AnualOpIVA", "");
                declaredData.Add("Exercise", "");
                declaredData.Add("TrimestralOp1", "");
                declaredData.Add("TrimestralOp2", "");
                declaredData.Add("TrimestralOp3", "");
                declaredData.Add("TrimestralOp4", "");
                declaredData.Add("AnualPropertyIVAOp1", "");
                declaredData.Add("AnualPropertyIVAOp2", "");
                declaredData.Add("AnualPropertyIVAOp3", "");
                declaredData.Add("AnualPropertyIVAOp4", "");
            }
            else
            {
                declaredData.Add("RenterNIF", "");
                declaredData.Add("LegalRepNIF", "");
                declaredData.Add("RenterName", "");
                declaredData.Add("TotalMoney", "");
                declaredData.Add("CatRef", " ");
                declaredData.Add("Situation", " ");
                declaredData.Add("StreetType", " ");
                declaredData.Add("StreetName", " ");
                declaredData.Add("TypeNum", " ");
                declaredData.Add("HouseNum", " ");
                declaredData.Add("QualNum", "");
                declaredData.Add("Block", "");
                declaredData.Add("Port", "");
                declaredData.Add("Stair", "");
                declaredData.Add("Floor", "");
                declaredData.Add("Door", "");
                declaredData.Add("Complement", "");
                declaredData.Add("Location", "");
                declaredData.Add("Town", "");
                declaredData.Add("TownCode", "");
                declaredData.Add("ProvinceCode", "");
                declaredData.Add("PostalCode", "");
            }
            isPropertyDeclared = isProperty;
        }

        /// <summary>
        /// Inicializa una nueva instancia de <c>Declared</c>, modificando sus propiedades.
        /// </summary>
        /// <param name="dict"> El diccionario de datos.</param>
        /// <param name="isProperty"> Indica si se está declarando un inmueble.</param>
        public Declared(Dictionary<string,string> dict, bool isProperty)
        {
            isPropertyDeclared = isProperty;
            declaredData = dict;
        }
    }
}
