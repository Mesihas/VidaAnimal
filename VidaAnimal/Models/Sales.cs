using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VidaAnimal.Models
{
    public class Sales
    {
        public int SalesId { get; set; }
        public int ClientId { get; set; }       
        public decimal Total { get; set; }
        public DateTime SellingDate { get; set; }
        public int PayWayId { get; set; } 
        public string Obs { get; set; }

    }
}
