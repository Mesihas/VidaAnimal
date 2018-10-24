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
    public class ProductController : Controller
    {
        private IDataService<Product> _productDataService;
        private IDataService<Category> _categoryDataService;
        private IDataService<Supplier> _SupplierDataService;
        public ProductController(IDataService<Product> service, IDataService<Category> service1, IDataService<Supplier> service2)
        {
            _productDataService = service;
            _categoryDataService = service1;
            _SupplierDataService = service2;
        }
        public IActionResult Index()
        {
            //get the list of Categories 
            IEnumerable<Category> categories = _categoryDataService.GetAll();
            IEnumerable<Product> products = _productDataService.GetAll();
            IEnumerable<Supplier> suppliers = _SupplierDataService.GetAll();

            //IEnumerable<Product> productList = _productDataService.Query(p => p.SubCategoryId == Id);
            //SubCategory subcat = _subCategoryDataService.GetSingle(p => p.SubCategoryId == Id);
            ////    string CatName = TempData["catName"].ToString();
            //Category cat = _categoryDataService.GetSingle(p => p.CategoryId == subcat.CategoryId);
            // Create vm
            ProductIndexViewModel vm = new ProductIndexViewModel
            {
                Total = categories.Count(),
                Categories = categories,
                Products = products.OrderBy(p => p.CategoryId),
                Suppliers = suppliers
            };
            return View(vm);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            //get the list of Categories 
            IEnumerable<Category> categories = _categoryDataService.GetAll();
            IEnumerable<Supplier> suppliers = _SupplierDataService.GetAll();
            //get the product from database
            Product pro = _productDataService.GetSingle(p => p.ProductId == Id);

            //// Create vm
            ProductUpdateViewModel vm = new ProductUpdateViewModel
            {
                Categories = categories,
                Suppliers = suppliers,
                ProductId = pro.ProductId,
                CategoryId = pro.CategoryId,
                SupplierId = pro.SupplierId,
                ProductName = pro.ProductName,
                Marked = pro.Marked,
                Cost = pro.Cost,
                IVA = pro.IVA,
                Discount = pro.Discount,
                PriceLastUdate = pro.PriceLastUdate,
                StockControl = pro.StockControl,
                StockMinim = pro.StockMinim,
                Units = pro.Units,
                Precio = pro.Precio
            };
            return View(vm);
        }

        [HttpPost]
        // [Authorize(Roles = "Administrator")]
        public IActionResult Edit(ProductUpdateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                Product pro = new Product
                {
                    ProductId = vm.ProductId,
                    CategoryId = vm.CategoryId,
                    SupplierId = vm.SupplierId,
                    ProductName = vm.ProductName,
                    Marked = vm.Marked,
                    Cost = vm.Cost,
                    IVA = vm.IVA,
                    Discount = vm.Discount,
                    PriceLastUdate = vm.PriceLastUdate,
                    StockControl = vm.StockControl,
                    StockMinim = vm.StockMinim,
                    Units = vm.Units,
                };
                _productDataService.Update(pro);
                return RedirectToAction("Index", "Product");
            }
            return View(vm);
        }

        [HttpGet]
        public IActionResult Create()
        {
            //get the list of Categories 
            IEnumerable<Category> categories = _categoryDataService.GetAll();
            IEnumerable<Supplier> suppliers = _SupplierDataService.GetAll();
            //get the product from database


            //// Create vm
            ProductCreateViewModel vm = new ProductCreateViewModel
            {
                Categories = categories,
                Suppliers = suppliers
            };
            
            return View(vm);
        }

        [HttpPost]
       // [Authorize(Roles = "Administrator")]
        public IActionResult Create(ProductCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                Product pro = new Product
                {
                    CategoryId = vm.CategoryId,
                    SupplierId = vm.SupplierId,
                    ProductName = vm.ProductName,
                    Marked = vm.Marked,
                    Cost = vm.Cost,
                    IVA = vm.IVA,
                    Discount = vm.Discount,
                    PriceLastUdate = vm.PriceLastUdate,
                    StockControl = vm.StockControl,
                    StockMinim = vm.StockMinim,
                    Units = vm.Units,
                };
                _productDataService.Create(pro);
                return RedirectToAction("Index", "Product");
            }
            return View(vm);
        }
    }
}



   