using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reader_347.Models
{
    /// <summary>
    /// Clase encargada de almacenar los ajustes de Excel.
    /// </summary>
    public sealed class ExcelSettings
    {
        //Implement a really simple Singleton pattern
        private static ExcelSettings _settings = null;

        /// <summary>
        /// Inicializa una nueva instancia de <c>ExcelSettings</c>.
        /// </summary>
        private ExcelSettings()
        {

        }

        /// <summary>
        /// Permite el acceso a la configuración de Excel. Si no existía, crea una nueva instancia.
        /// </summary>
        /// <remarks> Implementa el patrón Singleton, de modo que solo existe una instancia de la configuración.</remarks>
        public static ExcelSettings Settings
        {
            get
            {
                if (_settings == null)
                    _settings = new ExcelSettings();
                return _settings;
            }
        }

        public string DeclaredNIF { get => declaredNIF; set => declaredNIF = value; }
        public string LegalRepNIF { get => legalRepNIF; set => legalRepNIF = value; }
        public string DeclaredName { get => declaredName; set => declaredName = value; }
        public string ProvinceCode { get => provinceCode; set => provinceCode = value; }
        public string StateCode { get => stateCode; set => stateCode = value; }
        public string OpKey { get => opKey; set => opKey = value; }
        public string AnualOpMoney { get => anualOpMoney; set => anualOpMoney = value; }
        public string OpInsurance { get => opInsurance; set => opInsurance = value; }
        public string LocalBusinessRental { get => localBusinessRental; set => localBusinessRental = value; }
        public string MetalMoney { get => metalMoney; set => metalMoney = value; }
        public string AnualPropertyTransmissionIVA { get => anualPropertyTransmissionIVA; set => anualPropertyTransmissionIVA = value; }
        public string Exercise { get => exercise; set => exercise = value; }
        public string OpMoney1T { get => opMoney1T; set => opMoney1T = value; }
        public string PropertyTransmissionIVA1T { get => propertyTransmissionIVA1T; set => propertyTransmissionIVA1T = value; }
        public string OpMoney2T { get => opMoney2T; set => opMoney2T = value; }
        public string PropertyTransmissionIVA2T { get => propertyTransmissionIVA2T; set => propertyTransmissionIVA2T = value; }
        public string OpMoney3T { get => opMoney3T; set => opMoney3T = value; }
        public string PropertyTransmissionIVA3T { get => propertyTransmissionIVA3T; set => propertyTransmissionIVA3T = value; }
        public string OpMoney4T { get => opMoney4T; set => opMoney4T = value; }
        public string PropertyTransmissionIVA4T { get => propertyTransmissionIVA4T; set => propertyTransmissionIVA4T = value; }
        public string CommunityOpNIF { get => communityOpNIF; set => communityOpNIF = value; }
        public string OpSpecialRegIVA { get => opSpecialRegIVA; set => opSpecialRegIVA = value; }
        public string OpPassive { get => opPassive; set => opPassive = value; }
        public string OpRegNotCustoms { get => opRegNotCustoms; set => opRegNotCustoms = value; }
        public string AnualMoneyDevengedIVA { get => anualMoneyDevengedIVA; set => anualMoneyDevengedIVA = value; }
        public bool FirstRowIsTitle { get => firstRowIsTitle; set => firstRowIsTitle = value; }

        private string declaredNIF, legalRepNIF, declaredName, provinceCode, stateCode, opKey, anualOpMoney, opInsurance,
                       localBusinessRental, metalMoney, anualPropertyTransmissionIVA, exercise, opMoney1T, propertyTransmissionIVA1T,
                       opMoney2T, propertyTransmissionIVA2T, opMoney3T, propertyTransmissionIVA3T, opMoney4T, propertyTransmissionIVA4T,
                       communityOpNIF, opSpecialRegIVA, opPassive, opRegNotCustoms, anualMoneyDevengedIVA;
        private bool firstRowIsTitle = true;
    }
}
