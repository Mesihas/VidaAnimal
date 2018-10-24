using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VidaAnimal.Models
{
    public class ComplexProduct
    {
        public int ProductoId { get; set; }     
        public int ComponentProductId { get; set; }
        public int Quantity { get; set; }
    }
}
