using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VidaAnimal.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        public string ProductName { get; set; }
        public double Marked { get; set; }
        public double Cost { get; set; }
        public double IVA { get; set; }
        public double Discount { get; set; }
        public DateTime PriceLastUdate {get; set;}
        public int StockControl { get; set; }
        public int StockMinim { get; set; }
        public int Units { get; set; }
        [NotMapped]
        public double Precio { get { return Cost + 200; } }

    }
}
