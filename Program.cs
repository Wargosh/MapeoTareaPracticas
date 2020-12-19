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
        public static string ruta = @"D:\Respaldo\ASP.NET\PracticasDesarrollo\MapeoTareaPracticas\";

        public static List<Object[]> list = new List<Object[]>();

        //public static List<Object[]> listCSV = new List<Object[]>();

        public static List<int> camposValidos = new List<int>();

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

        public static void ReadExcelFile()
        {
            list.Clear();

            //Ruta del fichero Excel
            string filePath = ruta + "Ejemplo.xlsx";

            SLDocument sl = new SLDocument(filePath);

            // almacenar en una lista solo los campos que sean validos de acuerdo al esquema XML
            GetValidFields_Excel(sl);

            Console.WriteLine("");

            // Mapear los datos a una lista de objetos
            int fileRow = 1; // fila desde donde empieza [Excel toma encuenta desde 1]
            while (!string.IsNullOrEmpty(sl.GetCellValueAsString(fileRow, 1)))
            {
                Object[] o = new Object[10];
                foreach (var i in camposValidos)
                {
                   o[i] = sl.GetCellValueAsString(fileRow, i + 1);
                }

                list.Add(o);
                fileRow++; 
            }

            ShowData_Console();
        }

        public static void ReadXML()
        {
            schema = new Schema(); 

            XDocument documento = XDocument.Load(ruta+"schema.xml");
            // desde nombre //en de donde //descendants el nombre de la etiqueta //selecciona cual
            var esquema = from sche in documento.Descendants("Schema") select sche;  //ahora en esta variable tenemos todo el esquema
            //obtenemos el valor almacenado en la etiqueta document del esquema, la primera o default porque siempre llegara una
            string doctype = esquema.Elements("document").FirstOrDefault().Value;
            //obtenemos el valor almacenado en la etiqueta user del esquema
            string userid = esquema.Elements("user").FirstOrDefault().Value;

            // almacenar info del esquema recibido al objeto
            schema.docType = doctype;
            schema.userID = userid;

            var columnas = from colu in documento.Descendants("columns") select colu;  //ahora en esta variable tenemos todas las columas

            //cada elemento dentro del esquema es un XElement, recorremos con foreach para obtener el documento
            foreach (XElement col in columnas.Elements("column"))
            {
                // almacenar en el obejo schema el nombre de las etiquetas from y to
                schema.dataFROM.Add(col.Element("from").Value);
                schema.dataTO.Add(col.Element("to").Value);
            }
        }
        
        public static void ReadCSV()
        {
            list.Clear();

            string documentoCSV = File.ReadAllText(ruta + "EjemploCSV.csv");

            documentoCSV = documentoCSV.Replace('\n', '\r');
            string[] rows = documentoCSV.Split(new char[] { '\r' },
                StringSplitOptions.RemoveEmptyEntries);

            string[] headers = rows[0].Split(",");

            GetValidFields_CSV(headers);

            // Mapear los datos a una lista de objetos
            for (int i = 0 ; i < rows.Length; i++)
            {
                string[] data = rows[i].Split(",");

                Object[] o = new Object[10];
                foreach (var j in camposValidos)
                {
                    o[j] = data[j];
                }

                list.Add(o);
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
            camposValidos.Clear();

            for (int i = 0; i < 10; i++)
            {
                // verifica que el campo del archivo CVS o XLSX se encuentre dentro del esquema.
                if (schema.dataFROM.Contains(sl.GetCellValueAsString(1, i + 1)))
                    camposValidos.Add(i);
            }
        }

        public static void GetValidFields_CSV(string[] headers)
        {
            camposValidos.Clear();

            for (int i = 0; i < 10; i++)
            {
                // verifica que el campo del archivo CVS o XLSX se encuentre dentro del esquema.
                if (schema.dataFROM.Contains(headers[i]))
                    camposValidos.Add(i);
            }
            Console.WriteLine("");
        }

        public static void ShowData_Console()
        {
            List<int> listSort = new List<int>();
            // almacenar una lista con las posiciones de las columnas ordenadas como el esquema recibido
            for (int j = 0; j < schema.dataTO.Count; j++)
            {
                foreach (var d in camposValidos)
                {
                    if (list[0][d].ToString() == schema.dataFROM[j])
                    {
                        list[0][d] = schema.dataTO[j];
                        listSort.Add(d);
                        break;
                    }
                }
            }
            // presentar contenido del archivo siguiendo el esquema
            for (int x = 0; x < list.Count; x++)
            {
                for (int i = 0; i < camposValidos.Count; i++)
                {
                    if (!string.IsNullOrEmpty((string)list[x][listSort[i]]))
                        Console.Write(list[x][listSort[i]] + ", ");
                    else
                        Console.Write(", ");
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
        }
    }
}
