﻿using Microsoft.AspNetCore.Mvc;

namespace VidaAnimal.Controllers
{
  public class SalesController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }
    public IActionResult Create()
    {
      return View();
    }
  }
}
