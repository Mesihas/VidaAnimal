using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VidaAnimal.Models
{
    public class Stock
    {
        public int StockId { get; set; }
        public int ProductId { get; set; }
        public int StockControl { get; set; }
        public int CurrentStock { get; set; }
        public int StockMinim { get; set; }
        public string Unidades { get; set; }

    }
}
