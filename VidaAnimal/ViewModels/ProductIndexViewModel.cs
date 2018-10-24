using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VidaAnimal.Models;

namespace VidaAnimal.ViewModels
{
    public class ProductIndexViewModel
    {
        public int Total { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Supplier> Suppliers { get; set; }



    }
}
