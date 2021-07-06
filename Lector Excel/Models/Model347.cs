using Lector_Excel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Reader_347
{
    /// <summary>
    /// Clase encargada de almacenar los datos del modelo.
    /// </summary>
    public sealed class Model347
    {
        //Implement a really simple Singleton pattern
        private static Model347 _model = null;

        /// <summary>
        /// Inicializa una nueva instancia de <c>Model347</c>.
        /// </summary>
        private Model347()
        {

        }

        /// <summary>
        /// Permite el acceso al modelo. Si no existía, crea una nueva instancia.
        /// </summary>
        /// <remarks> Implementa el patrón Singleton, de modo que solo existe una instancia del modelo.</remarks>
        public static Model347 Model
        {
            get
            {
                if (_model == null)
                    _model = new Model347(); //This fails if multiple threads access! The app is not designed for multi-thread yet
                return _model;
            }
        }

        private Type1Registry type1Fields = new Type1Registry();
        private ObservableCollection<UserControl> listaDeclarados;

        /// <summary>
        /// Obtiene o modifica el registro de tipo 1.
        /// </summary>
        public Type1Registry Type1Fields { get => type1Fields; set => type1Fields = value; }
        /// <summary>
        /// Obtiene o modifica la lista de declarados (registros de tipo 2).
        /// </summary>
        public ObservableCollection<UserControl> ListaDeclarados { get => listaDeclarados; set => listaDeclarados = value; }
    }

    /// <summary>
    /// Clase auxiliar del modelo encargada de almacenar los datos del registro de tipo 1.
    /// </summary>
    public sealed class Type1Registry
    {
        private string ejercicio, declarantNIF, supportType = "T", declarantName, relationsName, relationsPhone,
                       declarationID, prevDeclarationID, totalEntities = "0", totalAnualMoney = "0", totalProperties = "0",
                       totalMoneyRental = "0", legalRepNIF;
        private bool isComplementaryDec = false, isSustitutiveDec = false, isNonSpecialDec = true;
        /// <summary>
        /// Inicializa una nueva instancia de <c>Type1Registry</c>.
        /// </summary>
        public Type1Registry()
        {

        }
        public string Ejercicio { get => ejercicio; set => ejercicio = value; }
        public string DeclarantNIF { get => declarantNIF; set => declarantNIF = value; }
        public string SupportType { get => supportType; set => supportType = value; }
        public string DeclarantName { get => declarantName; set => declarantName = value; }
        public string RelationsName { get => relationsName; set => relationsName = value; }
        public string RelationsPhone { get => relationsPhone; set => relationsPhone = value; }
        public string DeclarationID { get => declarationID; set => declarationID = value; }
        public string PrevDeclarationID { get => prevDeclarationID; set => prevDeclarationID = value; }
        public string TotalEntities { get => totalEntities; set => totalEntities = value; }
        public string TotalAnualMoney { get => totalAnualMoney; set => totalAnualMoney = value; }
        public string TotalProperties { get => totalProperties; set => totalProperties = value; }
        public string TotalMoneyRental { get => totalMoneyRental; set => totalMoneyRental = value; }
        public string LegalRepNIF { get => legalRepNIF; set => legalRepNIF = value; }
        public bool IsComplementaryDec { get => isComplementaryDec; set => isComplementaryDec = value; }
        public bool IsSustitutiveDec { get => isSustitutiveDec; set => isSustitutiveDec = value; }
        public bool IsNonSpecialDec { get => isNonSpecialDec; set => isNonSpecialDec = value; }
    }
}
