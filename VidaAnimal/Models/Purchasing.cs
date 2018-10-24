using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VidaAnimal.Models
{
    public class Purchasing
    {
        public int PurchasingId { get; set; }
        public int ProductId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double Total { get; set; }
        public DateTime PurchaseDate{ get; set; }
        public string Obs { get; set; }

    }
}
