using System;
using System.Collections.Generic;
using System.Text;

namespace MapeoTareaPracticas.Models
{
    public class Objeto
    {
        private string Doctype { get; set; }

        private string Userid { get; set; }

        public string ID { get; set; }

        private string Name { get; set; }

        private string Lastname { get; set; }

        private string Country { get; set; }

        private string Phone { get; set; }

        private string Address { get; set; }

        private string Email { get; set; }

        private string Date { get; set; }

        private string DNI { get; set; }

        private string PostalCode { get; set; }

        public Objeto ()
        {

        }

        public Objeto(string id, string name, string lastName, string country, string phone, string address, string email, string date, string dni, string postalcode)
        {
            this.ID = id;
            this.Name = name;
            this.Lastname = lastName;
            this.Country = country;
            this.Phone = phone;
            this.Address = address;
            this.Email = email;
            this.Date = date;
            this.DNI = dni;
            this.PostalCode = postalcode;
        }
    }
}
