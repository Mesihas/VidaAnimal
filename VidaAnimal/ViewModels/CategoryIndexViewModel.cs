using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VidaAnimal.Models;

namespace VidaAnimal.ViewModels
{
    public class CategoryIndexViewModel
    {
        public int Total { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
