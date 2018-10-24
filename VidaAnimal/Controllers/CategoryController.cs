using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VidaAnimal.Models;
using VidaAnimal.Services;
using VidaAnimal.ViewModels;
using Microsoft.AspNetCore.Authorization;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VidaAnimal.Controllers
{
    public class CategoryController : Controller
    {
        private IDataService<Category> _categoryDataService;
        public CategoryController(IDataService<Category> service)
        {
            _categoryDataService = service;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> list = _categoryDataService.GetAll();

            CategoryIndexViewModel vm = new CategoryIndexViewModel
            {
                Total = list.Count(),
                Categories = list
            };
            return View(vm);
        }

        //     [Authorize(Roles = "Administrator")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //       [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult Create(CategoryCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                //Map to object Order
                Category c = new Category
                {
                    CategoryId = vm.CategoryId,
                    Name = vm.Name,
                    Details = vm.Details
                };
                // save to db
                _categoryDataService.Create(c);
                // Go gome
                return RedirectToAction("Index", "Category");
            }
            // if not valid
            return View(vm);
        }
        [HttpGet]
        //       [Authorize(Roles = "Administrator")]
        public IActionResult Update(int id)
        {
            //get the Branch from database
            Category cat = _categoryDataService.GetSingle(c => c.CategoryId == id);
            CategoryUpdateViewModel vm = new CategoryUpdateViewModel
            {
                CategoryId = id,
                Name = cat.Name,
                Details = cat.Details,

            };
            return View(vm);
        }

        [HttpPost]
        //       [Authorize(Roles = "Administrator")]
        public IActionResult Update(CategoryUpdateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                Category c = new Category
                {
                    CategoryId = vm.CategoryId,
                    Name = vm.Name,
                    Details = vm.Details,
                };

                _categoryDataService.Update(c);
                return RedirectToAction("Index", "Category");
            }
            return View(vm);
        }
    }
}
