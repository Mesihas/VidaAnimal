using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VidaAnimal.Models
{
    public class Client
    {
        public int ClientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Telephone { get; set; }
        public string Address { get; set; }
        public string CUIT { get; set; }

    }
}
