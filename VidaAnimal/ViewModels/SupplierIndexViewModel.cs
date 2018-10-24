using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VidaAnimal.Models;

namespace VidaAnimal.ViewModels
{
    public class SupplierIndexViewModel
    {
        public int Total { get; set; }
        public IEnumerable<Supplier> Suppliers { get; set; }

    }
}
