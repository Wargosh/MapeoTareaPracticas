
using System.Collections.Generic;

namespace MapeoTareaPracticas.Models
{
    public class Schema
    {
        public string DocType { get; set; }                  // tipo de documento
        public string UserID { get; set; }                   // id del archivo XML
        public List<string> dataFROM = new List<string>();   // nombres de las columnas FRON del archivo XML
        public List<string> dataTO = new List<string>();     // nombres de las columnas TO del archivo XML
    }
}
