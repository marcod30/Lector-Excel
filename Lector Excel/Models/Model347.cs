using Lector_Excel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Reader_347
{
    public sealed class Model347
    {
        //Implement a really simple Singleton pattern
        private static Model347 _model = null;

        private Model347()
        {

        }

        public static Model347 Model
        {
            get
            {
                if (_model == null)
                    _model = new Model347();
                return _model;
            }
        }

        private static List<string> type1Fields;
        private static ObservableCollection<DeclaredFormControl> listaDeclarados;

        public static List<string> Type1Fields { get => type1Fields; set => type1Fields = value; }
        public static ObservableCollection<DeclaredFormControl> ListaDeclarados { get => listaDeclarados; set => listaDeclarados = value; }
    }
}
