using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VidaAnimal.Models;
using VidaAnimal.Services;
using VidaAnimal.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace VidaAnimal.Controllers
{
  public class ClientController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }
  }
}
