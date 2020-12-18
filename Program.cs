using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SpreadsheetLight;
using MapeoTareaPracticas.Models;

namespace MapeoTareaPracticas
{
    class Program
    {
        public static string ruta = @"D:\Respaldo\ASP.NET\MapeoTareaPracticas\";
        public static List<Object[]> list = new List<Object[]>();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            GetExcelFile();
            //ReadXML();
            
        }

        public static void GetExcelFile()
        {
            //Ruta del fichero Excel
            string filePath = ruta + "Ejemplo.xlsx";

            SLDocument sl = new SLDocument(filePath);

            int fileRow = 1; // fila desde donde empieza [Excel toma encuenta desde 1]

            List<int> camposValidos = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                // almacena la posicion del campo que es valido de acuerso al esquema XML
                if (!string.IsNullOrEmpty(Mapeo(sl.GetCellValueAsString(1, i + 1)))) { 
                    camposValidos.Add(i);
                    Console.Write(Mapeo(sl.GetCellValueAsString(1, i + 1)) + ", ");
                }
            }
            Console.WriteLine("");

            fileRow = 2; // empezar a leer desde la segunda fila
            while (!string.IsNullOrEmpty(sl.GetCellValueAsString(fileRow, 1)))
            {
                Object[] o = new Object[10];
                // leer datos de excel
                Objeto obj = new Objeto();
                foreach (var i in camposValidos)
                {
                    if (!string.IsNullOrEmpty(Mapeo(sl.GetCellValueAsString(1, i + 1))))
                    {
                        o[i] = sl.GetCellValueAsString(fileRow, i + 1);
                    }

                    
                }

                list.Add(o);

                fileRow++; 
            }

            // presentar contenido del archivo siguiendo el esquema
            for (int x = 0; x < list.Count; x++)
            {
                foreach (var i in camposValidos)
                {
                    if (!string.IsNullOrEmpty((string)list[x][i]))
                    {
                        Console.Write(list[x][i] + ", ");
                    }
                    else
                    {
                        Console.Write(", ");
                    }
                }
                Console.WriteLine("");
            }
        }

        public static string Mapeo(string nameCol)
        {
            //aqui cargamos el documento con XDocument
            XDocument documento = XDocument.Load(ruta + "schema.xml");

            //            desde nombre //en de donde //descendants el nombre de la etiqueta //selecciona cual
            var esquema = from sche in documento.Descendants("Schema") select sche;  //ahora en esta variable tenemos todo el esquema
            //obtenemos el valor almacenado en la etiqueta document del esquema, la primera o default porque siempre llegara una
            string doctype = esquema.Elements("document").FirstOrDefault().Value;
            //Console.WriteLine(doctype);
            //obtenemos el valor almacenado en la etiqueta user del esquema
            string userid = esquema.Elements("user").FirstOrDefault().Value;
            //Console.WriteLine(userid);

            var columnas = from colu in documento.Descendants("columns") select colu;  //ahora en esta variable tenemos todas las columas
            //cada elemento dentro del esquema es un XElement, recorremos con foreach para obtener el documento

            foreach (XElement elemento in columnas.Elements("column"))
            {
                if (nameCol == elemento.Element("from").Value)
                {
                    return elemento.Element("to").Value;
                }
            }
            return null;

        }

        public static void ReadXML()
        {
            //aqui cargamos el documento con XDocument
            XDocument documento = XDocument.Load(ruta + "schema.xml");

            //            desde nombre //en de donde //descendants el nombre de la etiqueta //selecciona cual
            var esquema = from sche in documento.Descendants("Schema") select sche;  //ahora en esta variable tenemos todo el esquema
            //obtenemos el valor almacenado en la etiqueta document del esquema, la primera o default porque siempre llegara una
            string doctype = esquema.Elements("document").FirstOrDefault().Value;
            Console.WriteLine(doctype);
            //obtenemos el valor almacenado en la etiqueta user del esquema
            string userid = esquema.Elements("user").FirstOrDefault().Value;
            Console.WriteLine(userid);

            var columnas = from colu in documento.Descendants("columns") select colu;  //ahora en esta variable tenemos todas las columas
            //cada elemento dentro del esquema es un XElement, recorremos con foreach para obtener el documento

            foreach (XElement elemento in columnas.Elements("column"))
            {
                Console.WriteLine(elemento.Element("to").Value);

                Console.WriteLine(elemento.Element("from").Value);
            }
        }
    }
}
