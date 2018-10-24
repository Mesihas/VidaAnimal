using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VidaAnimal.Models.DTO;

namespace VidaAnimal.ViewModels
{
  public class SalesAlgoViewModel
  {
    public IEnumerable<SalesDetailDTO> SalesDetaill { get; set; }
    public int Total { get; set; }
  }
}
