using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VidaAnimal.Models;

namespace VidaAnimal.ViewModels
{
    public class ProductUpdateViewModel
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        public string ProductName { get; set; }
        public double Marked { get; set; }
        public double Cost { get; set; }
        public double IVA { get; set; }
        public double Discount { get; set; }
        public DateTime PriceLastUdate { get; set; }
        public int StockControl { get; set; }
        public int StockMinim { get; set; }
        public int Units { get; set; }
        public double Precio { get; set; }

        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Supplier> Suppliers { get; set; }

    }
}
