using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VidaAnimal.Models.DTO
{
  public class ClientDTO
  {
    public int ClientId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Telephone { get; set; }
    public string Address { get; set; }
    public string CUIT { get; set; }
  }
}
