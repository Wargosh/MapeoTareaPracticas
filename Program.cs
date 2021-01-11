using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SpreadsheetLight;
using MapeoTareaPracticas.Models;
using System.IO;

namespace MapeoTareaPracticas
{
    class Program
    {
        public static string path = @"D:\Respaldo\ASP.NET\PracticasDesarrollo\MapeoTareaPracticas\";

        public static List<Object[]> dataList = new List<Object[]>(); // datos provenientes del archivo CSV o XLS

        public static List<int> validColumns = new List<int>(); // lista que almacena la posicion de los campos validos o que coinciden con el esquema cargado

        // contiene info del esquema recibido
        public static Schema schema = new Schema();

        static void Main(string[] args)
        {
            // primero obtener la info del XML
            ReadXML();

            Console.WriteLine("******************* Lectura del archivo .XLSX *******************");
            ReadExcelFile();

            Console.WriteLine("******************* Lectura del archivo .CSV *******************");
            ReadCSV();
        }

        /// <summary>
        /// Se encarga de almacenar los datos del esquema del archivo XML dentro de un objeto.
        /// </summary>
        public static void ReadXML()
        {
            schema = new Schema(); 

            XDocument doc = XDocument.Load(path + "schema.xml");
            // se carga todo el esquema en una variable
            var _schma = from sche in doc.Descendants("Schema") select sche; 
            
            // obtener informacion general del archivo XML
            string doctype = _schma.Elements("document").FirstOrDefault().Value;
            string userid = _schma.Elements("user").FirstOrDefault().Value;

            // almacenar info del esquema recibido al objeto
            schema.DocType = doctype;
            schema.UserID = userid;

            var columns = from colu in doc.Descendants("columns") select colu; // ahora en esta variable tenemos todas las columas

            // cada elemento dentro del esquema es un XElement, recorremos con foreach para obtener el documento
            foreach (XElement col in columns.Elements("column"))
            {
                // almacenar en el obejo schema el nombre de las etiquetas from y to
                schema.dataFROM.Add(col.Element("from").Value);
                schema.dataTO.Add(col.Element("to").Value);
            }
        }

        /// <summary>
        /// Se encarga de leer un archivo Excel obteniendo los campos de acuerdo al esquema XML cargado,
        /// almacena los datos en una lista de objetos y los envia para ser presentados por consola
        /// </summary>
        public static void ReadExcelFile()
        {
            dataList.Clear();

            //Ruta del fichero Excel
            string filePath = path + "Ejemplo.xlsx";

            SLDocument sl = new SLDocument(filePath);

            // almacenar en una lista solo los campos que sean validos de acuerdo al esquema XML
            GetValidFields_Excel(sl);

            Console.WriteLine("");

            // Mapear los datos a una lista de objetos
            int fileRow = 1; // fila desde donde empieza [Excel toma encuenta desde 1]
            while (!string.IsNullOrEmpty(sl.GetCellValueAsString(fileRow, 1)))
            {
                Object[] o = new Object[10];
                foreach (var i in validColumns)
                    o[i] = sl.GetCellValueAsString(fileRow, i + 1);

                dataList.Add(o);
                fileRow++;
            }

            ShowData_Console();
        }

        /// <summary>
        /// Se encarga de leer el archivo CSV obteniendo los campos de acuerdo al esquema XML cargado,
        /// almacena los datos en una lista de objetos y los envia para ser presentados por consola
        /// </summary>
        public static void ReadCSV()
        {
            dataList.Clear();

            string documentCSV = File.ReadAllText(path + "EjemploCSV.csv");

            documentCSV = documentCSV.Replace('\n', '\r');
            string[] rows = documentCSV.Split(new char[] { '\r' },
                StringSplitOptions.RemoveEmptyEntries);

            string[] headers = rows[0].Split(",");

            GetValidFields_CSV(headers);

            // Mapear los datos a una lista de objetos
            for (int i = 0 ; i < rows.Length; i++)
            {
                string[] data = rows[i].Split(",");

                Object[] o = new Object[10];
                foreach (var j in validColumns)
                    o[j] = data[j];

                dataList.Add(o);
            }

            ShowData_Console();
        }
        
        /// <summary>
        /// Se encarga de recorrer cada columna del archivo Excel y almacenar la posicion de los
        /// campos que si pertenezcan al esquema cargado.
        /// </summary>
        /// <param name="sl">obtiene informacion de las celdas del documento xlsx</param>
        public static void GetValidFields_Excel (SLDocument sl)
        {
            validColumns.Clear();

            // recorre y verifica que el campo del archivo XLS se encuentre dentro del esquema.
            for (int i = 0; i < 10; i++)
                if (schema.dataFROM.Contains(sl.GetCellValueAsString(1, i + 1)))
                    validColumns.Add(i);
        }

        /// <summary>
        /// Se encarga de recorrer cada columna del archivo CSV y almacenar la posicion de los
        /// campos que si pertenezcan al esquema cargado.
        /// </summary>
        /// <param name="headers">obtiene un arreglo con cada una de las columnas del .csv</param>
        public static void GetValidFields_CSV(string[] headers)
        {
            validColumns.Clear();

            // recorre y verifica que el campo del archivo CSV o se encuentre dentro del esquema.
            for (int i = 0; i < 10; i++)
                if (schema.dataFROM.Contains(headers[i]))
                    validColumns.Add(i);

            Console.WriteLine("");
        }

        /// <summary>
        /// Presenta la lista de datos por consola
        /// Primeramente ordena las columnas o variables de acuerdo al esquema recibido y lista las observaciones o filas
        /// </summary>
        public static void ShowData_Console()
        {
            List<int> listSort = new List<int>();
            // almacenar una lista con las posiciones de las columnas ordenadas como el esquema recibido
            for (int j = 0; j < schema.dataTO.Count; j++)
            {
                foreach (var d in validColumns)
                {
                    if (dataList[0][d].ToString() == schema.dataFROM[j])
                    {
                        dataList[0][d] = schema.dataTO[j];
                        listSort.Add(d);
                        break;
                    }
                }
            }
            // presentar contenido del archivo siguiendo el esquema
            for (int x = 0; x < dataList.Count; x++)
            {
                for (int i = 0; i < validColumns.Count; i++)
                {
                    if (!string.IsNullOrEmpty((string)dataList[x][listSort[i]]))
                        Console.Write(dataList[x][listSort[i]] + ", ");
                    else
                        Console.Write(", ");
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
        }
    }
}
