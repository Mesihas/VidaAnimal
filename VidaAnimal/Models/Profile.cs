using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VidaAnimal.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace VidaAnimal.Models
{
    public class Profile
    {
        public int ProfileId { get; set; }
        public string id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public int BranchId { get; set; }
        public string Legajo { get; set; }
        public DateTime FechaIngreso { get; set; }
        public bool Preocupacional { get; set; }
        public DateTime VtoLibreta { get; set; }
        public bool Activo { get; set; }
        public DateTime Birthday { get; set; }
        public string CUIL { get; set; }
        public int Nacionalidad { get; set; }
        public string Direccion { get; set; }
        public string Localidad { get; set; }
        public string CP { get; set; }
        public int TelMobil { get; set; }
        public int LandLine { get; set; }
        public string EstadoCivil { get; set; }
        public string Hijos { get; set; }

    }
}
