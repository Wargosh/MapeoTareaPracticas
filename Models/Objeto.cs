using System;
using System.Collections.Generic;
using System.Text;

namespace MapeoTareaPracticas.Models
{
    public class Objeto
    {
        private string doctype;

        private string userid;

        public string id { get; set; }

        private string name { get; set; }

        private string Lastname { get; set; }

        private string country { get; set; }

        private string phone { get; set; }

        private string address { get; set; }

        private string email { get; set; }

        private string date { get; set; }

        private string dni { get; set; }

        private string postalcode { get; set; }

        public void SetDoctype(string doc)
        {
            this.doctype = doc;
        }
        
        public string GetDoctype()
        {
            return this.doctype;
        }

        public Objeto ()
        {

        }

        public Objeto(string id, string name, string lastName, string country, string phone, string address, string email, string date, string dni, string postalcode)
        {
            this.id = id;
            this.name = name;
            this.Lastname = lastName;
            this.country = country;
            this.phone = phone;
            this.address = address;
            this.email = email;
            this.date = date;
            this.dni = dni;
            this.postalcode = postalcode;
        }
    }
}
