using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VidaAnimal.Models;

namespace VidaAnimal.ViewModels
{
  public class ClientIndexViewModel
  {
    public int Total { get; set; }
    public IEnumerable<Client> Clients { get; set; }
  }
}
