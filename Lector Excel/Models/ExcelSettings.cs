using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reader_347.Models
{
    public sealed class ExcelSettings
    {
        //Implement a really simple Singleton pattern
        private static ExcelSettings _settings = null;

        private ExcelSettings()
        {

        }

        public static ExcelSettings Settings
        {
            get
            {
                if (_settings == null)
                    _settings = new ExcelSettings();
                return _settings;
            }
        }

        private static List<string> _fields;
        public static List<string> Fields { get => _fields; set => _fields = value; }
    }
}
