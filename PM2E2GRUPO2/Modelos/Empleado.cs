using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database.Query;
using Firebase.Database;

namespace PM2E2GRUPO2.Modelos
{
    public class Empleado
    {
        public string Id { get; set; } // Ajustar el nombre de la propiedad según el nombre en Firebase
        public string descripcion { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
        public string Urlfoto { get; set; }
    }
}
