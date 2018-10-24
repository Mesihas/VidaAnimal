using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VidaAnimal.Models;

namespace VidaAnimal.ViewModels
{
    public class SalesIndexViewModel
    {
        public double Total { get; set; }
        public IEnumerable<Sales> Sales { get; set; }
    }
}
