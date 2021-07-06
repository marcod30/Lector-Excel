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

        /// <summary> El NIF del declarado.</summary>
        public string DeclaredNIF { get => declaredNIF; set => declaredNIF = value; }
        /// <summary> El NIF del representante legal.</summary>
        public string LegalRepNIF { get => legalRepNIF; set => legalRepNIF = value; }
        /// <summary> Nombre y apellidos, razón social o denominación del declarado.</summary>
        public string DeclaredName { get => declaredName; set => declaredName = value; }
        /// <summary> Código de provincia.</summary>
        public string ProvinceCode { get => provinceCode; set => provinceCode = value; }
        /// <summary> Código de país.</summary>
        public string StateCode { get => stateCode; set => stateCode = value; }
        /// <summary> Clave de operación.</summary>
        public string OpKey { get => opKey; set => opKey = value; }
        /// <summary> Importe anual de las operaciones.</summary>
        public string AnualOpMoney { get => anualOpMoney; set => anualOpMoney = value; }
        /// <summary> Operación seguro.</summary>
        public string OpInsurance { get => opInsurance; set => opInsurance = value; }
        /// <summary> Arrendamiento local de negocio.</summary>
        public string LocalBusinessRental { get => localBusinessRental; set => localBusinessRental = value; }
        /// <summary> Importe percibido en metálico.</summary>
        public string MetalMoney { get => metalMoney; set => metalMoney = value; }
        /// <summary> Importe anual percibido por transmisiones de inmuebles sujetas a IVA.</summary>
        public string AnualPropertyTransmissionIVA { get => anualPropertyTransmissionIVA; set => anualPropertyTransmissionIVA = value; }
        /// <summary> Ejercicio.</summary>
        public string Exercise { get => exercise; set => exercise = value; }
        /// <summary> Importe de las operaciones (1er trimestre).</summary>
        public string OpMoney1T { get => opMoney1T; set => opMoney1T = value; }
        /// <summary> Importe percibido por transmisiones de inmuebles sujetas a IVA (1T).</summary>
        public string PropertyTransmissionIVA1T { get => propertyTransmissionIVA1T; set => propertyTransmissionIVA1T = value; }
        /// <summary> Importe de las operaciones (2o trimestre).</summary>
        public string OpMoney2T { get => opMoney2T; set => opMoney2T = value; }
        /// <summary> Importe percibido por transmisiones de inmuebles sujetas a IVA (2T).</summary>
        public string PropertyTransmissionIVA2T { get => propertyTransmissionIVA2T; set => propertyTransmissionIVA2T = value; }
        /// <summary> Importe de las operaciones (3er trimestre).</summary>
        public string OpMoney3T { get => opMoney3T; set => opMoney3T = value; }
        /// <summary> Importe percibido por transmisiones de inmuebles sujetas a IVA (3T).</summary>
        public string PropertyTransmissionIVA3T { get => propertyTransmissionIVA3T; set => propertyTransmissionIVA3T = value; }
        /// <summary> Importe de las operaciones (4o trimestre).</summary>
        public string OpMoney4T { get => opMoney4T; set => opMoney4T = value; }
        /// <summary> Importe percibido por transmisiones de inmuebles sujetas a IVA (4T).</summary>
        public string PropertyTransmissionIVA4T { get => propertyTransmissionIVA4T; set => propertyTransmissionIVA4T = value; }
        /// <summary> NIF del operador comunitario.</summary>
        public string CommunityOpNIF { get => communityOpNIF; set => communityOpNIF = value; }
        /// <summary> Operaciones régimen especial con criterio de caja IVA.</summary>
        public string OpSpecialRegIVA { get => opSpecialRegIVA; set => opSpecialRegIVA = value; }
        /// <summary> Operación con inversión del sujeto pasivo.</summary>
        public string OpPassive { get => opPassive; set => opPassive = value; }
        /// <summary> Operación con bienes vinculados o destinados a vincularse al régimen de depósito distinto del aduanero.</summary>
        public string OpRegNotCustoms { get => opRegNotCustoms; set => opRegNotCustoms = value; }
        /// <summary> Importe anual de las operaciones devengadas conforme al criterio de caja del IVA.</summary>
        public string AnualMoneyDevengedIVA { get => anualMoneyDevengedIVA; set => anualMoneyDevengedIVA = value; }
        /// <summary> Indica si la primera fila del Excel contiene los títulos de los campos.</summary>
        public bool FirstRowIsTitle { get => firstRowIsTitle; set => firstRowIsTitle = value; }
        /// <summary> Tipo de hoja</summary>
        public string SheetType { get => sheetType; set => sheetType = value; }

        private string declaredNIF = "A", legalRepNIF = "B", declaredName = "C", sheetType = "D", provinceCode = "E", stateCode = "F", opKey = "G", anualOpMoney = "H", opInsurance = "I",
                       localBusinessRental = "J", metalMoney ="K", anualPropertyTransmissionIVA = "L", exercise ="M", opMoney1T ="N", propertyTransmissionIVA1T ="O",
                       opMoney2T ="P", propertyTransmissionIVA2T = "Q", opMoney3T = "R", propertyTransmissionIVA3T = "S", opMoney4T ="T", propertyTransmissionIVA4T = "U",
                       communityOpNIF = "V", opSpecialRegIVA = "W", opPassive = "X", opRegNotCustoms = "Y", anualMoneyDevengedIVA ="Z";
        private bool firstRowIsTitle = true;
    }
}
