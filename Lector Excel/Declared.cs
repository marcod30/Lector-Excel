using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lector_Excel
{
    public class Declared
    {
        bool isPropertyDeclared;
        public bool mayContainErrors = false;
        public Dictionary<string, string> declaredData;

        //Constructor
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

        public Declared(Dictionary<string,string> dict, bool isProperty)
        {
            isPropertyDeclared = isProperty;
            declaredData = dict;
        }
    }
}
