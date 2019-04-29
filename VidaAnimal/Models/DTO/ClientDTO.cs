using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VidaAnimal.Models.DTO
{
  public class ClientDTO
  {
    public int ClientId { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string telephone { get; set; }
    public string address { get; set; }
    public string cuit { get; set; }
  }
}
