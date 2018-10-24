using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VidaAnimal.Models
{
    public class Category
    {
        // Every Model must have OID that is the className followed by Id
        // the OID will become PK in the Database
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
      //  public string Type { get; set; }

        // one category has many SubCategories
      //  public ICollection<SubCategory> SubCategories { get; set; }
        public ICollection<Product> Products { get; set; }


    }
}
