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
    public class SupplierController : Controller
    {
        private IDataService<Supplier> _supplierDataService;

        public SupplierController(IDataService<Supplier> service)
        {
            _supplierDataService = service;
        }

    //    [Authorize(Roles = "Administrator")]
        public IActionResult Index()
        {
            IEnumerable<Supplier> list = _supplierDataService.GetAll();

            SupplierIndexViewModel vm = new SupplierIndexViewModel
            {
                Total = list.Count(),
                Suppliers = list
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
        public IActionResult Create(SupplierCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                //Map to object Order
                Supplier s = new Supplier
                {
                    SupplierName = vm.SupplierName,
                    Address = vm.Address,
                    Telephone = vm.Telephone
                };
                // save to db
                _supplierDataService.Create(s);
                // Go gome
                return RedirectToAction("Index", "Supplier");
            }
            // if not valid
            return View(vm);
        }

        [HttpGet]
 //       [Authorize(Roles = "Administrator")]
        public IActionResult Update(int id)
        {
            //get the Branch from database
            Supplier sup = _supplierDataService.GetSingle(p => p.SupplierId == id);
            SupplierUpdateViewModel vm = new SupplierUpdateViewModel
            {
                SupplierId = id,
                SupplierName = sup.SupplierName,
                Address = sup.Address,
                Telephone = sup.Telephone,
            };
            return View(vm);
        }

        [HttpPost]
 //       [Authorize(Roles = "Administrator")]
        public IActionResult Update(SupplierUpdateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                Supplier s= new Supplier
                {
                    SupplierId = vm.SupplierId,
                    SupplierName = vm.SupplierName,
                    Address = vm.Address,
                    Telephone = vm.Telephone,
                };

                _supplierDataService.Update(s);
                return RedirectToAction("Index", "Supplier");
            }
            return View(vm);
        }


    }
}