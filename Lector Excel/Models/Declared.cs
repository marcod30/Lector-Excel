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
        bool isPropertyDeclared;
        /// <value>Marca el declarado si tiene errores.</value>
        public bool mayContainErrors = false;
        /// <value> El diccionario de datos del declarado.</value>
        public Dictionary<string, string> declaredData;

        /// <summary>
        /// Inicializa una nueva instancia de <c>Declared</c>.
        /// </summary>
        public Declared()
        {
            declaredData = new Dictionary<string, string>();
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
            isPropertyDeclared = false;
        }

        /// <summary>
        /// Inicializa una nueva instancia de <c>Declared</c>, modificando sus propiedades.
        /// </summary>
        /// <param name="dict"> El diccionario de datos.</param>
        /// <param name="isProperty"> El declarado es de una propiedad física.</param>
        public Declared(Dictionary<string,string> dict, bool isProperty)
        {
            isPropertyDeclared = isProperty;
            declaredData = dict;
        }
    }
}
